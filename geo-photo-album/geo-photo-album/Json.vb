
Public Class Json
    Implements IDictionary(Of String, Json)
    Implements IList(Of Json)

    Private json_ As Object

    Sub New()
        json_ = Nothing
    End Sub

    Sub New(s As String)
        json_ = s
    End Sub

    Sub New(d As Double)
        json_ = d
    End Sub

    Sub New(b As Boolean)
        json_ = b
    End Sub

    Sub New(kvp As KeyValuePair(Of String, Json))
        json_ = New Dictionary(Of String, Json)
        Me.Add(kvp)
    End Sub

    Sub New(ParamArray jsons() As Json)
        json_ = New List(Of Json)(jsons)
    End Sub

    Sub New(jsons As IEnumerable(Of Json))
        json_ = New List(Of Json)(jsons)
    End Sub


    Sub MergeFile(f As String)
        Dim new_json As Json = FromFile(f)
        MergeJson(new_json)
    End Sub

    Public Sub MergeJson(new_json As Json)
        If json_ Is Nothing Then
            json_ = new_json.json_
        ElseIf IsObject AndAlso new_json.IsObject Then
            For Each kvp As KeyValuePair(Of String, Json) In new_json.ToObject
                If Not ToObject.ContainsKey(kvp.Key) Then
                    ToObject.Add(kvp.Key, kvp.Value)
                Else
                    ToObject(kvp.Key).MergeJson(kvp.Value) 'need to merge the inner objects
                End If
            Next
        ElseIf IsString AndAlso new_json.IsString Then
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
            Return DirectCast(json_, Dictionary(Of String, Json))
        End Get
    End Property

    Private ReadOnly Property ToArray As IList(Of Json)
        Get
            Return DirectCast(json_, List(Of Json))
        End Get
    End Property

    Public ReadOnly Property ToBoolean As Boolean
        Get
            Return DirectCast(json_, Boolean)
        End Get
    End Property

    Public ReadOnly Property ToDouble As Double
        Get
            Return DirectCast(json_, Double)
        End Get
    End Property

    Public Overrides Function ToString() As String
        If IsArray Or IsObject Then
            Return String.Join("", ToString("", "").Skip(1).Select2(Function(s As String)
                                                                        Return s
                                                                    End Function,
                                                                        Function(s As String)
                                                                            Return ""
                                                                        End Function))
        Else
            Return DirectCast(json_, String)
        End If
    End Function

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
        ElseIf IsDouble Then
            Yield prefix + CStr(json_)
        ElseIf IsString Then
            If CStr(json_).All(Function(c As Char) "{}[],:".Contains(c) OrElse Char.IsWhiteSpace(c)) Then
                Yield prefix + CStr(json_)
            Else
                Yield prefix + """" + String.Join("", CStr(json_).Replace("\", "\\").Replace(vbNewLine, "\n").Replace(vbCr, "\r") _
                                                             .Replace(vbFormFeed, "\f").Replace(vbTab, "\t") _
                                                             .Select(Function(c As Char)
                                                                         If AscW(c) > &HFF Then
                                                                             Return "\u" + AscW(c).ToString("04x")
                                                                         Else
                                                                             Return c
                                                                         End If
                                                                     End Function)) + """"
            End If
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
        ElseIf IsString Then
            Yield prefix + """" + String.Join("", CStr(json_).Replace("\", "\\").Replace(vbNewLine, "\n").Replace(vbCr, "\r") _
                                                             .Replace(vbFormFeed, "\f").Replace(vbTab, "\t") _
                                                             .Select(Function(c As Char)
                                                                         If AscW(c) > &HFF Then
                                                                             Return "\u" + AscW(c).ToString("04x")
                                                                         Else
                                                                             Return c
                                                                         End If
                                                                     End Function)) + """"
        ElseIf IsDouble Then
            Yield prefix + CStr(json_)
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
                ret.Add(key, New Json)
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
        FromFile = FromJson(e)
        f.Close()
    End Function

    Private Shared Iterator Function TokenizeStream(charStream As IEnumerable(Of Char)) As IEnumerable(Of String)
        Dim e1 As IIterator(Of Char) = charStream.GetIterator
        e1.MoveNext()
        Do While e1.HasCurrent
            If "{}[],:".Contains(e1.Current) Then
                Yield e1.Next
            ElseIf e1.Current = """"c Then
                Dim ret As String = e1.Next
                Do Until e1.Current = """"c
                    If e1.Current <> "\"c Then
                        ret += e1.Next
                    Else
                        e1.MoveNext()
                        Select Case e1.Current
                            Case """"c, "\"c, "/"c
                                ret += e1.Next
                            Case "b"c
                                e1.MoveNext()
                                ret += vbBack
                            Case "f"c
                                e1.MoveNext()
                                ret += vbFormFeed
                            Case "n"c
                                e1.MoveNext()
                                ret += vbNewLine
                            Case "r"c
                                e1.MoveNext()
                                ret += vbCr
                            Case "t"c
                                e1.MoveNext()
                                ret += vbTab
                            Case "u"c
                                e1.MoveNext()
                                Dim i As Integer = Integer.Parse(e1.Next + e1.Next + e1.Next + e1.Next, Globalization.NumberStyles.HexNumber)
                                ret += ChrW(i)
                            Case Else
                                Throw New ArgumentException("Can't parse special character in string")
                        End Select
                    End If
                Loop
                ret += e1.Next
                Yield ret
            ElseIf Char.IsWhiteSpace(e1.Current) Then
                e1.MoveNext()
                'yield nothing
            Else 'get a bunch of characters
                Dim ret As String = ""
                Do Until e1.HasCurrent = False OrElse "{}[],:".Contains(e1.Current) OrElse Char.IsWhiteSpace(e1.Current)
                    ret += e1.Next()
                Loop
                Yield ret
            End If
        Loop
    End Function
#End Region

    Shared Operator =(a As Json, b As Json) As Boolean
        If a.IsBoolean AndAlso b.IsBoolean Then
            Return a.ToBoolean = b.ToBoolean
        ElseIf a.IsDouble AndAlso b.IsDouble Then
            Return a.ToDouble = b.ToDouble
        ElseIf a.IsNothing AndAlso b.IsNothing Then
            Return True
        ElseIf a.IsString AndAlso b.IsString Then
            Return a.ToString = b.ToString
        ElseIf a.IsArray AndAlso b.IsArray AndAlso a.ToArray.Count = b.ToArray.Count Then
            For index As Integer = 0 To a.ToArray.Count - 1
                If a.ToArray(index) <> b.ToArray(index) Then
                    Return False
                End If
            Next
            Return True
        ElseIf a.IsObject AndAlso b.IsObject AndAlso a.ToObject.Count = b.ToObject.Count Then
            For Each key As String In a.ToObject.Keys
                If Not b.ToObject.ContainsKey(key) OrElse a.ToObject(key) <> b.ToObject(key) Then
                    Return False
                End If
            Next
            Return True
        Else
            Return False
        End If
    End Operator

    Shared Operator <>(a As Json, b As Json) As Boolean
        Return Not a = b
    End Operator

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
            Return ToArray.GetEnumerator
        Else
            Throw New ArgumentException("Can't get enumerator")
        End If
    End Function

    Public Sub Add(item As Json) Implements ICollection(Of Json).Add
        If IsNothing Then json_ = New List(Of Json)
        ToArray.Add(item)
    End Sub

    Public Sub Add(ParamArray items() As Json)
        For Each Item As Json In items
            Me.Add(Item)
        Next
    End Sub

    Public Sub Add(items As IEnumerable(Of Json))
        For Each Item As Json In items
            Me.Add(Item)
        Next
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

    Public Overrides Function Equals(obj As Object) As Boolean
        If TypeOf obj Is Json Then
            Return Me = CType(obj, Json)
        Else
            Return False
        End If
    End Function

    Private Function CombineHash(e As IEnumerable(Of Integer)) As Integer
        Dim key As Integer = 1
        For Each i As Integer In e
            For shift As Integer = 0 To 3
                Dim b As Byte = CByte((i >> shift) And &HFF)
                key += b
                key += key << 12
                key = key Xor (key >> 22)
                key += key << 4
                key = key Xor (key >> 9)
                key += key << 10
                key = key Xor (key >> 2)
                key += key << 7
                key = key Xor (key >> 12)
            Next
        Next
        Return key
    End Function

    Public Overrides Function GetHashCode() As Integer
        If IsNothing OrElse IsBoolean OrElse IsDouble OrElse IsString Then
            Return json_.GetHashCode
        ElseIf IsArray Then
            Return CombineHash(ToArray.Select(Function(j As Json)
                                                  Return j.GetHashCode
                                              End Function))
        ElseIf IsObject Then
            Return CombineHash(ToObject.Select(Function(kvp As KeyValuePair(Of String, Json))
                                                   Return CombineHash({kvp.Key.GetHashCode, kvp.Value.GetHashCode})
                                               End Function))
        Else
            Throw New ArgumentException("Can't GetHashCode")
        End If
    End Function
End Class