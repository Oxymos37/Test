

Imports System.Data.SqlClient
Imports MySql.Data.MySqlClient

Module modUtil

    Public MaCxionMySql As New MySqlConnection
    'base mediathequemotstrodus en localhost
    Public BaseUtile As String = "Database=motsTordus_V0;Data Source=localhost;User Id=root;Password=;SslMode=none"
    Public MonReader As MySqlDataReader
    'il faut doubles les \ pour en conserver un en sql !
    Public cheminImage As String = "C:\\Images_Mediatheque\\"

    Public Erreur As Integer = 0
    Public MsgErreur As String

    'connexion à la base de données
    Public Sub Connexion()
        MaCxionMySql.ConnectionString = BaseUtile
        'on ouvre la connexion au serveur avec les paramètres spécifiés 
        Try
            MaCxionMySql.Open()
        Catch ex As Exception
            Erreur = 1
            MsgErreur = ex.Message
        End Try

    End Sub

    'deconnexion 
    Public Sub Deconnexion()
        MaCxionMySql.Close()
    End Sub

End Module


