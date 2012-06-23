Public Class Json
    Private json_ As Object

    Sub MergeFile(f As String)
        Dim new_json As Json = FromFile(f)
        MergeJson(new_json)
    End Sub

    Private Sub MergeJson(new_json As Json)
        If json_ Is Nothing Then
            json_ = new_json.json_
        ElseIf (TypeOf json_ Is Dictionary(Of String, Json) AndAlso
                TypeOf new_json.json_ Is Dictionary(Of String, Json)) Then
            Dim current_dict As Dictionary(Of String, Json) = DirectCast(json_, Dictionary(Of String, Json))
            Dim new_dict As Dictionary(Of String, Json) = DirectCast(new_json.json_, Dictionary(Of String, Json))
            For Each kvp As KeyValuePair(Of String, Json) In new_dict
                If Not current_dict.ContainsKey(kvp.Key) Then
                    current_dict.Add(kvp.Key, kvp.Value)
                Else
                    current_dict(kvp.Key).MergeJson(kvp.Value) 'need to merge the inner objects
                End If
            Next
        ElseIf (TypeOf json_ Is String AndAlso
                TypeOf new_json.json_ Is String) Then
            If CStr(json_) <> CStr(new_json.json_) Then
                Throw New ApplicationException("Can't merge dissimilar strings")
            End If
        Else
            Throw New ApplicationException("Unhandled Merge")
        End If
    End Sub

    Default Property Item(key As Object) As Json
        Get
            If (TypeOf key Is String AndAlso
                TypeOf json_ Is Dictionary(Of String, Json)) Then
                Dim current_dict As Dictionary(Of String, Json) = DirectCast(json_, Dictionary(Of String, Json))
                Return current_dict(CStr(key))
            Else
                Throw New ApplicationException("Unhandled Item()")
            End If
        End Get
        Set(value As Json)
            If (TypeOf key Is String AndAlso
                TypeOf json_ Is Dictionary(Of String, Json)) Then
                Dim current_dict As Dictionary(Of String, Json) = DirectCast(json_, Dictionary(Of String, Json))
                current_dict(CStr(key)) = value
            Else
                Throw New ApplicationException("Unhandled Item()")
            End If
        End Set
    End Property

    Sub Remove(key As Object)
        If (TypeOf key Is String AndAlso
            TypeOf json_ Is Dictionary(Of String, Json)) Then
            Dim current_dict As Dictionary(Of String, Json) = DirectCast(json_, Dictionary(Of String, Json))
            current_dict.Remove(CStr(key))
        End If
    End Sub

    ReadOnly Property IsObject() As Boolean
        Get
            Return TypeOf json_ Is Dictionary(Of String, Json)
        End Get
    End Property

    ReadOnly Property IsNothing As Boolean
        Get
            Return json_ Is Nothing
        End Get
    End Property

    Function ContainsKey(key As String) As Boolean
        If json_ Is Nothing Then
            Return False
        ElseIf TypeOf json_ Is Dictionary(Of String, Json) Then
            Dim dict As Dictionary(Of String, Json) = CType(json_, Dictionary(Of String, Json))
            Return dict.ContainsKey(key)
        Else
            Throw New ApplicationException("ContainsKey() on non-dictionary")
        End If
    End Function

    Public Shared Widening Operator CType(j As Json) As Dictionary(Of String, Json)
        If TypeOf j.json_ Is Dictionary(Of String, Json) And j.json_ IsNot Nothing Then
            Return CType(j.json_, Dictionary(Of String, Json))
        Else
            Throw New ApplicationException("Can't cast Json to Dictionary")
        End If
    End Operator

    Public Iterator Function Keys() As IEnumerable(Of String)
        If TypeOf json_ Is Dictionary(Of String, Json) Then
            Dim d As Dictionary(Of String, Json) = Me
            For Each key As String In d.Keys
                Yield key
            Next
        Else
            Throw New ApplicationException("Can't Keys()")
        End If
    End Function

    Public Function ToObject() As Object
        If TypeOf json_ Is Dictionary(Of String, Json) Then
            Dim ret As New Dictionary(Of String, Object)
            For Each key As String In Me.Keys
                ret.Add(key, Me(key).ToObject())
            Next
            Return ret
        ElseIf TypeOf json_ Is String Then
            Return CStr(json_)
        Else
            Throw New ApplicationException("Can't ToObject()")
        End If
    End Function

    Public Sub Add(key As String)
        If json_ Is Nothing Then
            json_ = New Dictionary(Of String, Json)
        End If
        If TypeOf json_ Is Dictionary(Of String, Json) Then
            Dim d As Dictionary(Of String, Json) = Me
            d.Add(key, New Json)
        Else
            Throw New ApplicationException("Can't Add() to non-Dictionary")
        End If
    End Sub

    Public Sub Add(key As String, j As Json)
        If json_ Is Nothing Then
            json_ = New Dictionary(Of String, Json)
        End If
        If TypeOf json_ Is Dictionary(Of String, Json) Then
            Dim d As Dictionary(Of String, Json) = Me
            d.Add(key, j)
        Else
            Throw New ApplicationException("Can't Add() to non-Dictionary")
        End If
    End Sub

    Public Overloads Iterator Function ToString(Optional prefix As String = "", Optional indent As String = "  ") As IEnumerable(Of String)
        If TypeOf json_ Is Dictionary(Of String, Json) Then
            Dim d As Dictionary(Of String, Json) = DirectCast(json_, Dictionary(Of String, Json))
            Yield prefix + "{"
            For Each e1 As IEnumerable(Of String) In d.Select(Iterator Function(kvp As KeyValuePair(Of String, Json))
                                                                  If TypeOf kvp.Value.json_ Is String AndAlso DirectCast(kvp.Value.json_, String) = "" Then
                                                                      Yield prefix + indent + kvp.Key
                                                                  Else
                                                                      Yield prefix + indent + kvp.Key + ":"
                                                                      For Each s As String In kvp.Value _
                                                                                              .ToString(prefix + indent, indent)
                                                                          Yield s
                                                                      Next
                                                                  End If
                                                              End Function)
                For Each s As String In e1.Select2(Function(s1 As String)
                                                       Return s1
                                                   End Function,
                                                   Function(s1 As String)
                                                       Return s1 + ","
                                                   End Function)
                    Yield s
                Next
            Next
            Yield prefix + "}"
        ElseIf TypeOf json_ Is String Then
            Yield prefix + CStr(json_)
        Else
            Throw New ApplicationException("Can't ToString()")
        End If
    End Function

#Region "FromJson"
    Private Shared Function ParseNumber(e1 As IEnumerator(Of String)) As Double
        Dim ret As Double = Double.Parse(e1.Current)
        e1.MoveNext()
        Return ret
    End Function

    Private Shared Function ParseValue(e1 As IEnumerator(Of String)) As Json
        If e1.Current.StartsWith(""""c) Then
            Dim ret As New Json
            ret.json_ = ParseString(e1)
            Return ret
        ElseIf "-+0123456789".Contains(e1.Current(0)) Then
            Dim ret As New Json
            ret.json_ = ParseNumber(e1)
            Return ret
        ElseIf e1.Current = "{" Then
            Return ParseObject(e1)
        ElseIf e1.Current = "[" Then
            Return ParseArray(e1)
        ElseIf e1.Current = "true" Then
            Dim ret As New Json
            ret.json_ = True
            e1.MoveNext()
            Return ret
        ElseIf e1.Current = "false" Then
            Dim ret As New Json
            ret.json_ = False
            e1.MoveNext()
            Return ret
        ElseIf e1.Current = "null" Then
            Dim ret As New Json
            ret.json_ = Nothing
            e1.MoveNext()
            Return ret
        Else 'generous
            Dim ret As New Json
            ret.json_ = ParseString(e1)
            Return ret
            'Throw New ApplicationException("Can't ParseValue, unrecognized")
        End If
    End Function

    Private Shared Function ParseArray(e1 As IEnumerator(Of String)) As Json
        If Not e1.Current = "[" Then
            e1.MoveNext() 'generous parser
        End If
        Dim ret As New Json
        ret.json_ = New List(Of Json)
        Do While Not e1.Current = "]" OrElse e1.Current = Nothing ' generous
            Dim value As Json = ParseValue(e1)
            If e1.Current <> "," AndAlso e1.Current <> "]" AndAlso e1.Current <> Nothing Then
                Throw New ApplicationException("Expected another value or end of array")
            End If
            Do While e1.Current = ","
                e1.MoveNext()
            Loop
        Loop
        If e1.Current = "]" Then e1.MoveNext()
        Return ret
    End Function

    Private Shared Function ParseObject(e1 As IEnumerator(Of String)) As Json
        If e1.Current = "{" Then
            e1.MoveNext() 'generous parser
        End If
        Dim ret As New Json
        ret.json_ = New Dictionary(Of String, Json)
        Do Until e1.Current = "}" OrElse e1.Current = Nothing 'generous
            Dim key As String = ParseString(e1)
            If e1.Current = "," Then
                e1.MoveNext()
                ret.Add(key, Nothing)
            Else
                If Not e1.Current = ":" Then
                    Throw New ApplicationException("Expected ':'")
                Else
                    e1.MoveNext()
                    Dim value As Json = ParseValue(e1)
                    ret.Add(key, value)
                End If
            End If
            If e1.Current <> "," AndAlso e1.Current <> "}" Then
                Throw New ApplicationException("Expected another pair or end of Object")
            End If
            Do While e1.Current = ","
                e1.MoveNext() 'generous parser
            Loop
        Loop
        If e1.Current = "}" Then e1.MoveNext()
        Return ret
    End Function

    Private Shared Function ParseString(e1 As IEnumerator(Of String)) As String
        Dim with_quotes As String = e1.Current
        Dim without_quotes As String = with_quotes.Trim(""""c) 'if no quotes, no problem
        'handle special characters at some point
        e1.MoveNext()
        Return without_quotes
    End Function

    Private Shared Function FromJson(elements As IEnumerable(Of String)) As Json
        Dim e1 As IEnumerator(Of String) = elements.GetEnumerator
        e1.MoveNext()
        Return ParseObject(e1)
    End Function

    Private Shared Iterator Function TextIterator(sr As System.IO.StreamReader) As IEnumerable(Of Char)
        Do Until sr.EndOfStream
            Yield ChrW(sr.Read())
        Loop
    End Function

    Public Shared Function FromString(s As String) As Json
        Dim e As IEnumerable(Of String) = TokenizeStream(s)
        Return FromJson(e)
    End Function

    Public Shared Function FromFile(filename As String) As Json
        Dim f As System.IO.StreamReader = System.IO.File.OpenText(filename)
        Dim e As IEnumerable(Of String) = TokenizeStream(TextIterator(f))
        Return FromJson(e)
    End Function

    Private Shared Iterator Function TokenizeStream(charStream As IEnumerable(Of Char)) As IEnumerable(Of String)
        Dim e1 As IEnumerator(Of Char) = charStream.GetEnumerator
        Do While e1.MoveNext
            If "{}[],:".Contains(e1.Current) Then
                Yield e1.Current
            ElseIf e1.Current = """"c Then
                Dim ret As String = e1.Current
                e1.MoveNext()
                Do Until e1.Current = """"c
                    If e1.Current <> "\"c Then
                        ret += e1.Current
                        e1.MoveNext()
                    Else
                        ret += e1.Current
                        e1.MoveNext()
                        If e1.Current <> "u"c Then
                            ret += e1.Current
                            e1.MoveNext()
                        Else
                            For i As Integer = 1 To 4
                                ret += e1.Current
                                e1.MoveNext()
                            Next
                        End If
                    End If
                Loop
                ret += e1.Current
                Yield ret
            ElseIf "+-0123456789".Contains(e1.Current) Then
                Dim ret As String = e1.Current
                e1.MoveNext()
                Do While "0123456789.eE+-".Contains(e1.Current)
                    ret += e1.Current
                    e1.MoveNext()
                Loop
                Yield ret
            ElseIf Char.IsWhiteSpace(e1.Current) Then
                'yield nothing
            Else 'generous strings
                Dim ret As String = ""
                Dim moveSuccess As Boolean = True
                Do Until moveSuccess = False OrElse "{}[],:".Contains(e1.Current) OrElse Char.IsWhiteSpace(e1.Current)
                    ret += e1.Current
                    moveSuccess = e1.MoveNext
                Loop
                Yield ret

                If moveSuccess AndAlso "{}[],:".Contains(e1.Current) Then 'have to catch this before the movenext
                    Yield e1.Current
                ElseIf moveSuccess AndAlso Char.IsWhiteSpace(e1.Current) Then
                    'yield nothing
                Else
                    Return
                End If
            End If
        Loop
    End Function
#End Region

    Public Iterator Function ToJson(Optional prefix As String = "", Optional indent As String = "  ") As IEnumerable(Of String)
        If TypeOf json_ Is Dictionary(Of String, Json) Then
            Dim d As Dictionary(Of String, Json) = DirectCast(json_, Dictionary(Of String, Json))
            Yield prefix + "{"
            For Each e1 As IEnumerable(Of String) In d.Select(Iterator Function(kvp As KeyValuePair(Of String, Json))
                                                                  Yield prefix + indent + """" + kvp.Key + """:"
                                                                  For Each s As String In kvp.Value _
                                                                                          .ToJson(prefix + indent, indent)
                                                                      Yield s
                                                                  Next
                                                              End Function)
                For Each s As String In e1.Select2(Function(s1 As String)
                                                       Return s1
                                                   End Function,
                                                   Function(s1 As String)
                                                       Return s1 + ","
                                                   End Function)
                    Yield s
                Next
            Next
            Yield prefix + "}"
        ElseIf TypeOf json_ Is String Then
            Yield prefix + """" + CStr(json_) + """"
        Else
            Throw New ApplicationException("Can't ToJson()")
        End If
    End Function

    Public Function Count() As Integer
        If TypeOf json_ Is Dictionary(Of String, Json) Then
            Dim d As Dictionary(Of String, Json) = Me
            Return d.Count
        Else
            Throw New ApplicationException("Can't Count()")
        End If
    End Function
End Class
