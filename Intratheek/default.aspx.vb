'  _____       _             _   _               _    
' |_   _|     | |           | | | |             | |   
'   | |  _ __ | |_ _ __ __ _| |_| |__   ___  ___| | __
'   | | | '_ \| __| '__/ _` | __| '_ \ / _ \/ _ \ |/ /
'  _| |_| | | | |_| | | (_| | |_| | | |  __/  __/   < 
' |_____|_| |_|\__|_|  \__,_|\__|_| |_|\___|\___|_|\_\
'
' De digitale schoolbibliotheek 
'
' Eindwerk Tom Dobbelaere 2014 - 2015

Imports System.Security.Principal

Public Class _default
    Inherits System.Web.UI.Page

    Dim wiGebruiker As WindowsIdentity = WindowsIdentity.GetCurrent()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        PageLoadOperations(litAangemeld, Session)
    End Sub

End Class