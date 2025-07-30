Dim observerArray As JArray = JArray.Parse(JObserver.ToString())

' Converter o JToken para uma lista de strings
Dim emailsParaExcluirList As New List(Of String) 
For Each item As JObject In observerArray
  If item("email") IsNot Nothing Then
    emailsParaExcluirList.Add(item("email").ToString())
  End If
Next

' Remover os e-mails da lista EmailSing e os URLs correspondentes da UrlSing
For i As Integer = EmailSing.Count - 1 To 0 Step -1
    If emailsParaExcluirList.Contains(EmailSing(i)) Then
        EmailSing.RemoveAt(i)
        UrlSing.RemoveAt(i)
    End If
Next

' Verificar se as listas têm o mesmo tamanho
If EmailSing.Count = UrlSing.Count Then
    ' Inicializar a string formatada
    Dim formattedString As String = "Documentos enviados: " & vbCrLf &
  vbCrLf

    ' Iterar sobre as listas e adicionar os valores à string formatada
    For i As Integer = 0 To EmailSing.Count - 1

        formattedString &= $"Email: {EmailSing(i)}" & vbCrLf & "URL:" & vbCrLf
        ' Remover os colchetes e aspas extras da string JSON
        Dim urls As String = UrlSing(i).Replace("[", "").Replace("]", "").Replace("""", "")
        ' Dividir a string em URLs individuais
        Dim urlArray As String() = urls.Split(New String() {","}, StringSplitOptions.RemoveEmptyEntries)
        ' Adicionar cada URL à string formatada
        For Each url In urlArray
            formattedString &= $" ""{url.Trim()}"" " & vbCrLf
        Next
        formattedString &= vbCrLf

    Next

  formattedStringOut = formattedString
    ' Exibir a string formatada (ou enviar por email)
    Console.WriteLine(formattedString)
Else
    Console.WriteLine("As listas não têm o mesmo tamanho.")
End If