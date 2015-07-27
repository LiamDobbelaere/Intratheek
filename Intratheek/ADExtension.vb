Imports System.Runtime.CompilerServices
Imports System.Security.Principal
Imports System.DirectoryServices

Module ADExtension

    <Extension()>
    Public Function GetGUID(winID As WindowsIdentity) As String
        Dim guid As String = String.Empty

        If winID IsNot Nothing Then
            Dim sid As SecurityIdentifier = winID.User

            Using userDe As New DirectoryEntry("LDAP://<SID=" + sid.Value + ">")
                guid = userDe.NativeGuid
            End Using
        End If

        Return guid
    End Function

    Public Function UsernameFromGUID(GUID As String) As String
        Dim strGuidNew As String = String.Empty

        'litAangemeld.Text &= strGuid & Environment.NewLine

        Dim strPartA, strPartB, strPartC, strPartD As String
        strPartA = GUID.Substring(0, 2)
        strPartB = GUID.Substring(2, 2)
        strPartC = GUID.Substring(4, 2)
        strPartD = GUID.Substring(6, 2)

        strGuidNew &= strPartD & strPartC & strPartB & strPartA

        strPartA = GUID.Substring(8, 2)
        strPartB = GUID.Substring(10, 2)

        strGuidNew &= strPartB & strPartA

        strPartA = GUID.Substring(12, 2)
        strPartB = GUID.Substring(14, 2)

        strGuidNew &= strPartB & strPartA
        strGuidNew &= GUID.Substring(16, GUID.Length - 16)

        strGuidNew = strGuidNew.Insert(8, "-")
        strGuidNew = strGuidNew.Insert(13, "-")
        strGuidNew = strGuidNew.Insert(18, "-")
        strGuidNew = strGuidNew.Insert(23, "-")

        Dim user As DirectoryEntry = New DirectoryEntry("LDAP://<GUID=" & strGuidNew & ">")

        Return user.Name.Remove(0, 3)
    End Function

    <Extension()>
    Public Function IsInGroup(winID As WindowsIdentity, group As String)
        For Each ir As IdentityReference In winID.Groups
            Dim ntAccount As NTAccount = ir.Translate(GetType(NTAccount))
            Dim strGroupname As String = ntAccount.ToString()

            If strGroupname.EndsWith(group) Then
                Return True
            End If
        Next

        Return False
    End Function

    <Extension()>
    Public Function GetUsername(winID As WindowsIdentity)
        Dim strName As String = winID.Name
        Dim strSplitname As String() = strName.Split("\"c)

        strName = strSplitname.Last()

        Return strName
    End Function

End Module
