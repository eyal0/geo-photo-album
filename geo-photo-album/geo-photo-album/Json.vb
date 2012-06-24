Public Class Json
    Implements IDictionary(Of String, Json)
    Implements IList(Of Json)

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
    ReadOnly Property IsArray As Boolean
        Get
            Return TypeOf json_ Is List(Of Json)
        End Get
    End Property
    ReadOnly Property IsString As Boolean
        Get
            Return TypeOf json_ Is String
        End Get
    End Property
    ReadOnly Property IsDouble As Boolean
        Get
            Return TypeOf json_ Is Double
        End Get
    End Property
    ReadOnly Property IsBoolean As Boolean
        Get
            Return TypeOf json_ Is Boolean
        End Get
    End Property

    Private ReadOnly Property ToObject As IDictionary(Of String, Json)
        Get
            Return CType(json_, Dictionary(Of String, Json))
        End Get
    End Property

    Private ReadOnly Property ToArray As IList(Of Json)
        Get
            Return CType(json_, List(Of Json))
        End Get
    End Property

    Public Overloads Iterator Function ToString(Optional prefix As String = "", Optional indent As String = "  ") As IEnumerable(Of String)
        If IsObject Then
            Yield prefix + "{"

            For Each e1 As IEnumerable(Of String) In ToObject.Select2(Iterator Function(kvp As KeyValuePair(Of String, Json))
                                                                          If kvp.Value Is Nothing OrElse kvp.Value.IsNothing Then
                                                                              Yield prefix + indent + kvp.Key + ","
                                                                          Else
                                                                              Yield prefix + indent + kvp.Key + ":"
                                                                              For Each s As String In kvp.Value.ToString(prefix + indent, indent).Select2(Function(s1 As String)
                                                                                                                                                              Return s1
                                                                                                                                                          End Function,
                                                                                                                                                          Function(s1 As String)
                                                                                                                                                              Return s1 + ","
                                                                                                                                                          End Function)
                                                                                  Yield s
                                                                              Next
                                                                          End If
                                                                      End Function,
                                                                      Iterator Function(kvp As KeyValuePair(Of String, Json))
                                                                          If kvp.Value Is Nothing OrElse kvp.Value.IsNothing Then
                                                                              Yield prefix + indent + kvp.Key
                                                                          Else
                                                                              Yield prefix + indent + kvp.Key + ":"
                                                                              For Each s As String In kvp.Value.ToString(prefix + indent, indent)
                                                                                  Yield s
                                                                              Next
                                                                          End If
                                                                      End Function)
                For Each s As String In e1
                    Yield s
                Next
            Next
            Yield prefix + "}"
        ElseIf IsArray Then
            Yield prefix + "["
            For Each e1 As IEnumerable(Of String) In ToArray.Select2(Iterator Function(j As Json)
                                                                         For Each s As String In j.ToString(prefix + indent, indent).Select2(Function(s1 As String)
                                                                                                                                                 Return s1
                                                                                                                                             End Function,
                                                                                                                                             Function(s1 As String)
                                                                                                                                                 Return s1 + ","
                                                                                                                                             End Function)
                                                                             Yield s
                                                                         Next
                                                                     End Function,
                                                                     Iterator Function(j As Json)
                                                                         For Each s As String In j.ToString(prefix + indent, indent)
                                                                             Yield s
                                                                         Next
                                                                     End Function)
                For Each s As String In e1
                    Yield s
                Next
            Next
            Yield prefix + "]"
        ElseIf IsString OrElse IsDouble Then
            Yield prefix + CStr(json_)
        ElseIf IsBoolean Then
            Yield prefix + CStr(json_.ToString.ToLower)
        ElseIf IsNothing Then
            Yield prefix + "null"
        Else
            Throw New ApplicationException("Can't ToString()")
        End If
    End Function

    Public Iterator Function ToJson(Optional prefix As String = "", Optional indent As String = "  ") As IEnumerable(Of String)
        If IsObject Then
            Yield prefix + "{"
            For Each e1 As IEnumerable(Of String) In ToObject.Select2(Iterator Function(kvp As KeyValuePair(Of String, Json))
                                                                          Yield prefix + indent + """" + kvp.Key + """ :"
                                                                          If kvp.Value Is Nothing Then
                                                                              Yield prefix + indent + indent + "null"
                                                                          Else
                                                                              For Each s As String In kvp.Value.ToJson(prefix + indent + indent, indent).Select2(Function(s1 As String)
                                                                                                                                                                     Return s1
                                                                                                                                                                 End Function,
                                                                                                                                                                 Function(s1 As String)
                                                                                                                                                                     Return s1 + ","
                                                                                                                                                                 End Function)
                                                                                  Yield s
                                                                              Next
                                                                          End If
                                                                      End Function,
                                                                      Iterator Function(kvp As KeyValuePair(Of String, Json))
                                                                          Yield prefix + indent + """" + kvp.Key + """ :"
                                                                          If kvp.Value Is Nothing Then
                                                                              Yield prefix + indent + indent + "null"
                                                                          Else
                                                                              For Each s As String In kvp.Value.ToJson(prefix + indent + indent, indent)
                                                                                  Yield s
                                                                              Next
                                                                          End If
                                                                      End Function)
                For Each s As String In e1
                    Yield s
                Next
            Next
            Yield prefix + "}"
        ElseIf IsArray Then
            Yield prefix + "["
            For Each e1 As IEnumerable(Of String) In ToArray.Select2(Iterator Function(j As Json)
                                                                         For Each s As String In j.ToJson(prefix + indent, indent).Select2(Function(s1 As String)
                                                                                                                                               Return s1
                                                                                                                                           End Function,
                                                                                                                                           Function(s1 As String)
                                                                                                                                               Return s1 + ","
                                                                                                                                           End Function)
                                                                             Yield s
                                                                         Next
                                                                     End Function,
                                                                     Iterator Function(j As Json)
                                                                         For Each s As String In j.ToJson(prefix + indent, indent)
                                                                             Yield s
                                                                         Next
                                                                     End Function)
                For Each s As String In e1
                    Yield s
                Next
            Next
            Yield prefix + "]"
        ElseIf IsString OrElse IsDouble Then
            Yield prefix + """" + CStr(json_) + """"
        ElseIf IsBoolean Then
            Yield prefix + CStr(json_.ToString.ToLower)
        ElseIf IsNothing Then
            Yield prefix + "null"
        Else
            Throw New ApplicationException("Can't ToJson()")
        End If
    End Function

#Region "FromJson"
    Private Shared Function ParseNumber(e1 As IEnumerator(Of String)) As Double
        Dim ret As Double = Double.Parse(e1.Current)
        e1.MoveNext()
        Return ret
    End Function

    Private Shared Function ParseValue(e1 As IIterator(Of String)) As Json
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

    Private Shared Function ParseArray(e1 As IIterator(Of String)) As Json
        If e1.Current = "[" Then
            e1.MoveNext() 'generous parser
        End If
        Dim ret As New Json
        ret.json_ = New List(Of Json)
        Do Until Not e1.HasCurrent OrElse e1.Current = "]" ' generous
            Dim value As Json = ParseValue(e1)
            ret.Add(value)
            If e1.HasCurrent AndAlso e1.Current <> "," AndAlso e1.Current <> "]" Then
                Throw New ApplicationException("Expected another value or end of array")
            End If
            Do While e1.HasCurrent AndAlso e1.Current = ","
                e1.MoveNext()
            Loop
        Loop
        If e1.HasCurrent AndAlso e1.Current = "]" Then e1.MoveNext()
        Return ret
    End Function

    Private Shared Function ParseObject(e1 As IIterator(Of String)) As Json
        If e1.Current = "{" Then
            e1.MoveNext() 'generous parser
        End If
        Dim ret As New Json
        ret.json_ = New Dictionary(Of String, Json)
        Do Until Not e1.HasCurrent OrElse e1.Current = "}" 'generous
            Dim key As String = ParseString(e1)
            If Not e1.HasCurrent OrElse e1.Current = "," OrElse e1.Current = "}" Then
                ret.Add(key, Nothing)
            ElseIf Not e1.Current = ":" Then
                Throw New ApplicationException("Expected ':'")
            Else 'must be a :
                e1.MoveNext()
                Dim value As Json = ParseValue(e1)
                ret.Add(key, value)
            End If
            If e1.HasCurrent AndAlso e1.Current <> "," AndAlso e1.Current <> "}" Then
                Throw New ApplicationException("Expected another pair or end of Object")
            End If
            Do While e1.HasCurrent AndAlso e1.Current = ","
                e1.MoveNext() 'generous parser
            Loop
        Loop
        If e1.HasCurrent AndAlso e1.Current = "}" Then e1.MoveNext()
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
        Dim e1 As Iterator(Of String) = elements.GetIterator
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
        Dim e1 As IIterator(Of Char) = charStream.GetIterator
        e1.MoveNext()
        Do While e1.HasCurrent
            If "{}[],:".Contains(e1.Current) Then
                Yield e1.Next
            ElseIf e1.Current = """"c Then
                Dim ret As String = e1.next
                Do Until e1.Current = """"c
                    If e1.Current <> "\"c Then
                        ret += e1.Next
                    Else
                        ret += e1.Next()
                        If e1.Current <> "u"c Then
                            ret += e1.Next()
                        Else
                            For i As Integer = 1 To 4
                                ret += e1.Next()
                            Next
                        End If
                    End If
                Loop
                ret += e1.Next
                Yield ret
            ElseIf "+-0123456789".Contains(e1.Current) Then
                Dim ret As String = e1.Next()
                Do While e1.HasCurrent AndAlso "0123456789.eE+-".Contains(e1.Current)
                    ret += e1.Next()
                Loop
                Yield ret
            ElseIf Char.IsWhiteSpace(e1.Current) Then
                e1.MoveNext()
                'yield nothing
            Else 'generous strings
                Dim ret As String = ""
                Do Until e1.HasCurrent = False OrElse "{}[],:".Contains(e1.Current) OrElse Char.IsWhiteSpace(e1.Current)
                    ret += e1.Next()
                Loop
                Yield ret
            End If
        Loop
    End Function
#End Region
    Public Sub Add(item As KeyValuePair(Of String, Json)) Implements ICollection(Of KeyValuePair(Of String, Json)).Add
        If IsNothing Then json_ = New Dictionary(Of String, Json)
        ToObject.Add(item)
    End Sub

    Public Sub Clear() Implements ICollection(Of KeyValuePair(Of String, Json)).Clear, ICollection(Of Json).Clear
        If Not IsNothing Then ToObject.Clear()
    End Sub

    Public Function Contains(item As KeyValuePair(Of String, Json)) As Boolean Implements ICollection(Of KeyValuePair(Of String, Json)).Contains
        If IsNothing Then Return False Else Return ToObject.Contains(item)
    End Function

    Public Sub CopyTo(array() As KeyValuePair(Of String, Json), arrayIndex As Integer) Implements ICollection(Of KeyValuePair(Of String, Json)).CopyTo
        If Not IsNothing Then ToObject.CopyTo(array, arrayIndex)
    End Sub

    Public ReadOnly Property Count As Integer Implements ICollection(Of KeyValuePair(Of String, Json)).Count, ICollection(Of Json).Count
        Get
            If IsNothing Then Return 0 Else If IsObject Then Return ToObject.Count Else Return ToArray.Count
        End Get
    End Property

    Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of KeyValuePair(Of String, Json)).IsReadOnly, ICollection(Of Json).IsReadOnly
        Get
            If IsNothing Then
                Return False
            ElseIf IsObject Then
                Return ToObject.IsReadOnly
            ElseIf IsArray Then
                Return ToArray.IsReadOnly
            Else
                Throw New ArgumentException("Can't IsReadOnly")
            End If
        End Get
    End Property

    Public Function Remove(item As KeyValuePair(Of String, Json)) As Boolean Implements ICollection(Of KeyValuePair(Of String, Json)).Remove
        If IsNothing Then Return False Else Return ToObject.Remove(item)
    End Function

    Public Sub Add(key As String, value As Json) Implements IDictionary(Of String, Json).Add
        If IsNothing Then json_ = New Dictionary(Of String, Json)
        ToObject.Add(key, value)
    End Sub

    Public Function ContainsKey(key As String) As Boolean Implements IDictionary(Of String, Json).ContainsKey
        If IsNothing Then Return False Else Return ToObject.ContainsKey(key)
    End Function

    Default Public Overloads Property Item(key As String) As Json Implements IDictionary(Of String, Json).Item
        Get
            If IsNothing Then Throw New KeyNotFoundException() Else Return ToObject.Item(key)
        End Get
        Set(value As Json)
            If IsNothing Then json_ = New Dictionary(Of String, Json)
            ToObject.Item(key) = value
        End Set
    End Property

    Public ReadOnly Property Keys As ICollection(Of String) Implements IDictionary(Of String, Json).Keys
        Get
            If IsNothing Then Return New List(Of String) Else Return ToObject.Keys
        End Get
    End Property

    Public Function Remove(key As String) As Boolean Implements IDictionary(Of String, Json).Remove
        If IsNothing Then Return False Else Return ToObject.Remove(key)
    End Function

    Public Function TryGetValue(key As String, ByRef value As Json) As Boolean Implements IDictionary(Of String, Json).TryGetValue
        If IsNothing Then Return False Else Return ToObject.TryGetValue(key, value)
    End Function

    Public ReadOnly Property Values As ICollection(Of Json) Implements IDictionary(Of String, Json).Values
        Get
            If IsNothing Then Return New List(Of Json) Else Return ToObject.Values
        End Get
    End Property

    Public Function GetObjectEnumerator() As IEnumerator(Of KeyValuePair(Of String, Json)) Implements IEnumerable(Of KeyValuePair(Of String, Json)).GetEnumerator
        If IsNothing Then Return New List(Of KeyValuePair(Of String, Json)).Enumerator Else Return ToObject.GetEnumerator
    End Function

    Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        If IsNothing Then
            Return New List(Of Object).Enumerator
        ElseIf IsObject Then
            Return ToObject.GetEnumerator
        ElseIf IsArray Then
            Return ToObject.GetEnumerator
        Else
            Throw New ArgumentException("Can't get enumerator")
        End If
    End Function

    Public Sub Add(item As Json) Implements ICollection(Of Json).Add
        If IsNothing Then json_ = New List(Of Json)
        ToArray.Add(item)
    End Sub

    Public Function Contains(item As Json) As Boolean Implements ICollection(Of Json).Contains
        If IsNothing Then Return False Else Return ToArray.Contains(item)
    End Function

    Public Sub CopyTo(array() As Json, arrayIndex As Integer) Implements ICollection(Of Json).CopyTo
        If IsNothing Then Return Else ToArray.CopyTo(array, arrayIndex)
    End Sub

    Public Function Remove(item As Json) As Boolean Implements ICollection(Of Json).Remove
        If IsNothing Then Return False Else Return ToArray.Remove(item)
    End Function

    Public Function GetArrayEnumerator() As IEnumerator(Of Json) Implements IEnumerable(Of Json).GetEnumerator
        If IsNothing Then Return New List(Of Json).Enumerator Else Return ToArray.GetEnumerator
    End Function

    Public Function IndexOf(item As Json) As Integer Implements IList(Of Json).IndexOf
        If IsNothing Then Return -1 Else Return ToArray.IndexOf(item)
    End Function

    Public Sub Insert(index As Integer, item As Json) Implements IList(Of Json).Insert
        If IsNothing Then json_ = New List(Of Json)
        ToArray.Insert(index, item)
    End Sub

    Default Public Overloads Property Item(index As Integer) As Json Implements IList(Of Json).Item
        Get
            Return ToArray.Item(index)
        End Get
        Set(value As Json)
            If IsNothing Then Throw New ArgumentOutOfRangeException() Else ToArray.Item(index) = value
        End Set
    End Property

    Public Sub RemoveAt(index As Integer) Implements IList(Of Json).RemoveAt
        If IsNothing Then Throw New ArgumentOutOfRangeException() Else ToArray.RemoveAt(index)
    End Sub
End Class
