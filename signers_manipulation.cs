Try
  Dim assinantesString As String = ArgArrayAssinantes
  Dim assinantesArray As String() = assinantesString.Split(";"c)
  Dim electronicSigners As New JArray()
  Dim signers As New JArray()
  Dim tags As New JArray()
  'Dim previouscpf As String = String.Empty
  'Dim signersInfo As New JArray()

  JsonObject.Add("documents", JDocuments)

  Dim sender As New JObject()
  sender.Add("name", "email")
  'sender.Add("nameCompany", "SEDE")
  'sender.Add("companyIdentificationCode", "0000")
  sender.Add("email", "email")
  'sender.Add("individualIdentificationCode", "")
  JsonObject.Add("sender", sender)

  For Each assinanteString In assinantesArray
    If String.IsNullOrWhiteSpace(assinanteString) Then
      Continue For

    End If

      Dim campos As String() = assinanteString.Split("&"c)
      Dim assinante As New Dictionary(Of String, String)

      For Each campo In campos
          Dim keyValue As String() = campo.Split("="c)
      If keyValue.Length = 2 Then 
            assinante.Add(keyValue(0), keyValue(1))
      End If
      Next

    If assinante.Count = 0 Then
      Continue For

    End If

      Dim participantSettings As New JObject()
      Dim signatyInfo As New JObject()
      signatyInfo.Add("step", "1")
      signatyInfo.Add("title", assinante("ipttpassinantes"))


    Dim nomeOriginal As String = assinante("iptnome").ToString().ToLower()
    Dim nomeFormatado As String = New CultureInfo("pt-BR").TextInfo.ToTitleCase(nomeOriginal)

      signatyInfo.Add("name", nomeFormatado)
      signatyInfo.Add("email", assinante("iptemail").ToString().ToLower())
      signatyInfo.Add("individualIdentificationCode", assinante("iptcpf"))

    Dim assinaturaPresencial As Boolean = assinante("ipttpassinatura").Equals("Assinatura presencial", StringComparison.OrdinalIgnoreCase)

      signatyInfo.Add("inPerson", assinaturaPresencial)

    Dim cpf As String = assinante("iptcpf")
    cpf = System.Text.RegularExpressions.Regex.Replace(cpf, "[^\d]", "")
    tags.Add(cpf)
    Dim acessCode As String = If(cpf.Length >= 4, cpf.Substring(0,4), cpf)
      signatyInfo.Add("accessCode", acessCode)

    Dim telefone As String = assinante("ipttelefone")
    telefone = System.Text.RegularExpressions.Regex.Replace(telefone, "[^\d]", "")
      signatyInfo.Add("cellphone", telefone)


      Dim identificationType As New JObject()

    'Verificação de campo de código de acesso
    Dim codacesso As Boolean = assinante("chkcodacesso").Equals("1")
      identificationType.Add("accessCode", codacesso)

    ' Verificação do campo de sms
    Dim smsEnabled As Boolean = assinante("chksms").Equals("1")
      identificationType.Add("sms", smsEnabled)

      signatyInfo.Add("identificationType", identificationType)

    Dim assinaturaComClick As Boolean = assinante("ipttpassinatura").Equals("Assinar com click", StringComparison.OrdinalIgnoreCase)

      Dim signOneClick As New JObject()
      signOneClick.Add("enable", assinaturaComClick)
      'signOneClick.Add("title", "signatário")
      signOneClick.Add("approveText", "Assinar")
      signOneClick.Add("rejectText", "Não Assinar")

      participantSettings.Add("signOneClick", signOneClick)
     signatyInfo.Add("signOneClick", signOneClick)
      signatyInfo.Add("participantSettings", participantSettings)

    'Verificação do tipo de assinatura ELETRONICA ou DIGITAL 

    If assinante("ipteletronica").Equals("Assinatura Eletrônica", 
      StringComparison.OrdinalIgnoreCase) Then 
      electronicSigners.Add(signatyInfo)

    End If

    If assinante("ipteletronica").Equals("Assinatura digital", 
      StringComparison.OrdinalIgnoreCase) Then
      Dim signerDigital As New JObject()
        signerDigital.Add("step", "1")
        signerDigital.Add("title", assinante("ipttpassinantes"))
        signerDigital.Add("name", assinante("iptnome"))
        signerDigital.Add("email", assinante("iptemail"))
        signerDigital.Add("individualIdentificationCode", assinante("iptcpf"))
      signers.Add(signerDigital)
    End If

    Console.WriteLine($"Assinante: {assinante("iptnome")}, CPF/CNPJ: {assinante("iptcpf")}, Título do assinante: Signatário")
  Next

  JsonObject.Add("signers", signers)
  JsonObject.Add("electronicSigners", electronicSigners)  



  Dim folderId As New JArray()
  Dim signatureStandard As New JArray()


  Dim observers As New JArray()
  Dim observersId As New JObject()

  If NomeEquipe.Contains("Equipe CSC") Then 
    JsonObject.Add("observers", EquipeCSC)
  ElseIf NomeEquipe.Contains("Equipe Processos") Then   
    JsonObject.Add("observers", EquipeProcessos)

  End If
  'observersId.Add("name", "rpa_user")
  'observersId.Add("email", observador)
  'observersId.Add("individualIdentificationCode", "00000000000")
  observers.Add(observersId)
  'JsonObject.Add("observers", observers)

  Dim representatives As New JArray()
  JsonObject.Add("representatives", representatives)

  tags.Add("RPA")
  tags.Add(idprocesso)
  'signatureStandard.Add(2)
  JsonObject.Add("signatureStandard", 1)
  JsonObject.Add("tags", tags)

  Dim recipients As New JObject()
  recipients.Add("emails", "email")
  recipients.Add("attached", True)
  JsonObject.Add("recipients", observador)

  Dim deadline As New JObject()
  deadline.Add("signatureDeadlineDate", Dtblockdocument)
  deadline.Add("blockDocumentAfterLimit", True)

  'JsonObject.Add("deadline", deadline)
  JsonObject.Add("sendSubscribersNotified", True)

  If (signers.Count > 0)
    JsonObject.Add("signers", signers)
  End If
  If (folderId.Count > 0)
    JsonObject.Add("folderId", folderId)
  End If


Catch ex As Exception
  Console.WriteLine("Erro: " & ex.Message)
End Try