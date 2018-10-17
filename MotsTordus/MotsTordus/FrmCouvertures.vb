
Imports System.Data.SqlClient
Imports MySql.Data.MySqlClient

Public Class FrmCouvertures
    Dim MonImage As Image
    Dim Ajout As Boolean
    Dim MonAdapter As New MySqlDataAdapter
    Dim MaRequete As String
    Dim UnDataset As New DataSet
    Dim MaDataTable As DataTable
    Dim maTableCombo As DataTable
    Dim cmd As MySqlCommand
    Dim requete As String
    Dim Droit As String
    Dim req As String
    Dim ImageBd As String



#Region "Gestion du DataGridView"

    Public Sub rempDatagridConnec()
        'Sélection des informations et remplissage du DataGridView
        MaRequete = "SELECT bd_code, bd_titre, bd_serie, bd_num_tome, bd_image, edi_code FROM bd ORDER BY bd_code"
        MonAdapter.SelectCommand = New MySqlCommand(MaRequete, MaCxionMySql)
        MonAdapter.Fill(UnDataset, "bd")
        MaDataTable = UnDataset.Tables("bd")
        dtGrdBd.DataSource = MaDataTable
        dtGrdBd.Columns("bd_code").Visible = False
        dtGrdBd.Columns("bd_titre").HeaderText = "Titre"
        dtGrdBd.Columns("bd_titre").Width = 127
        dtGrdBd.Columns("bd_serie").HeaderText = "Série"
        dtGrdBd.Columns("bd_serie").Width = 101
        dtGrdBd.Columns("bd_num_tome").HeaderText = "Tome"
        dtGrdBd.Columns("bd_num_tome").Width = 35
        dtGrdBd.Columns("bd_image").HeaderText = "Adresse de l'image"
        dtGrdBd.Columns("bd_image").Width = 215
        dtGrdBd.Columns("edi_code").HeaderText = "Editeur"
        dtGrdBd.Columns("edi_code").Width = 80
        dtGrdBd.Width = 600
        dtGrdBd.Height = 175
    End Sub

    Private Sub dtGrdBd_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dtGrdBd.CellEnter
        If btnSauvegarder.Tag <> "A" Then
            'Affichage des données dans le DataGridView
            txtCode.Text = dtGrdBd.CurrentRow.Cells("bd_code").Value
            txtTitre.Text = dtGrdBd.CurrentRow.Cells("bd_titre").Value
            txtNumTome.Text = dtGrdBd.CurrentRow.Cells("bd_num_tome").Value
            txtSerie.Text = dtGrdBd.CurrentRow.Cells("bd_serie").Value
            txtEditeur.Text = dtGrdBd.CurrentRow.Cells("edi_code").Value
            pictBoxImage.ImageLocation = "C:\images_mediatheque\" & dtGrdBd.CurrentRow.Cells("bd_image").Value & ".jpg"
        End If
    End Sub

#End Region

#Region "Gestion des boutons"

    Private Sub btnAjouter_Click(sender As Object, e As EventArgs)
        MasqueBouton(False)
        Raz()
        txtTitre.Enabled = True
        txtNumTome.Enabled = True

        btnSauvegarder.Location = New Point(15, 31)
        btnAnnuler.Location = New Point(15, 111)
        btnSauvegarder.Tag = "A"
    End Sub

    Private Sub btnRetour_Click(sender As Object, e As EventArgs) Handles btnRetour.Click
        Me.Close()
    End Sub

    Private Sub btnAnnuler_Click(sender As Object, e As EventArgs) Handles btnAnnuler.Click
        RazBouton(True)
        Raz()
        btnSauvegarder.Tag = ""
        MajEcran()
        MaDataTable.Clear()
        rempDatagridConnec()
        dtGrdBd.Refresh()
    End Sub

    Private Sub btnModifier_Click(sender As Object, e As EventArgs) Handles btnModifier.Click
        'On autorise la modification du texte
        MasqueBouton(False)
        btnSauvegarder.Tag = "M"
        btnParcourir.Visible = True
        btnSauvegarder.Enabled = False
    End Sub

    Private Sub btnRechercher_Click(sender As Object, e As EventArgs) Handles btnRechercher.Click
        'On autorise la saisie de texte dans les champs
        txtTitre.Enabled = True
        txtNumTome.Enabled = True
        txtSerie.Enabled = True
        txtEditeur.Enabled = True
        Raz()
        MasqueRecherche(False)
        btnValider.Visible = True
        btnSansCouv.Visible = True

        btnValider.Location = New Point(15, 31)
        btnAnnuler.Location = New Point(15, 111)
        btnSansCouv.Location = New Point(15, 191)
        btnValider.Tag = "V"
    End Sub

    Public Sub Raz()
        'On efface les champs texte
        txtCode.Text = ""
        txtTitre.Text = ""
        txtNumTome.Text = ""
        txtSerie.Text = ""
        txtEditeur.Text = ""

    End Sub

#End Region

#Region "Procédures"

    'Affichage de certains boutons et on masque d'autres boutons
    Public Sub MasqueBouton(valider As Boolean)
        btnModifier.Visible = valider
        btnRechercher.Visible = valider
        btnSupprimer.Visible = valider
        btnSauvegarder.Visible = Not (valider)
        btnAnnuler.Visible = Not (valider)
        btnValider.Visible = valider
        btnSauvegarder.Location = New Point(15, 31)
        btnAnnuler.Location = New Point(15, 111)

    End Sub

    'Affichage des boutons à l'accueil de l'interface des couvertures
    Public Sub RazBouton(valider As Boolean)
        btnModifier.Visible = valider
        btnRechercher.Visible = valider
        btnSupprimer.Visible = valider
        btnSauvegarder.Visible = Not (valider)
        btnValider.Visible = Not (valider)
        btnAnnuler.Visible = Not (valider)
        btnParcourir.Visible = Not (valider)
        btnSansCouv.Visible = Not (valider)
    End Sub

    'Affichage de certains boutons et on masque d'autres boutons
    Public Sub MasqueRecherche(valider As Boolean)
        btnModifier.Visible = valider
        btnRechercher.Visible = valider
        btnSupprimer.Visible = valider
        btnSauvegarder.Visible = valider
        btnValider.Visible = valider
        btnAnnuler.Visible = Not (valider)
        btnSansCouv.Visible = valider
        btnValider.Location = New Point(15, 31)
        btnAnnuler.Location = New Point(15, 111)
        btnSansCouv.Location = New Point(15, 191)

    End Sub

    'Rafraichissement du DataGrid View au clic sur la cellule d'en-tête de la première colonne
    Private Sub dtGrdBd_MouseClick(sender As Object, e As MouseEventArgs) Handles dtGrdBd.MouseClick
        Dim hit As DataGridView.HitTestInfo = dtGrdBd.HitTest(e.X, e.Y)
        If (hit.Type = DataGridViewHitTestType.TopLeftHeader) Then
            MaDataTable.Clear()
            rempDatagridConnec()
        End If
    End Sub

    'Affichage des données et des boutons
    Private Sub FrmCouvertures_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MajEcran()

        btnSauvegarder.Visible = False
        btnAnnuler.Visible = False
        btnValider.Visible = False

        rempDatagridConnec()
    End Sub

    'On n'autorise pas la modification des champs texte et certains boutons n'apparaissent pas
    Private Sub MajEcran()
        txtCode.Enabled = False
        txtTitre.Enabled = False
        txtNumTome.Enabled = False
        txtSerie.Enabled = False
        txtEditeur.Enabled = False
        btnValider.Visible = False
        btnSauvegarder.Visible = False
        btnAnnuler.Visible = False
        btnSansCouv.Visible = False
    End Sub

#End Region

#Region "Gestion des données"
    'Au clic sur le bouton Sauvegarder
    Private Sub btnSauvegarder_Click(sender As Object, e As EventArgs) Handles btnSauvegarder.Click
        If (btnSauvegarder.Tag = "M") Then
            Dim numTome As String = dtGrdBd.CurrentRow.Cells("bd_num_tome").Value
            Dim titreBd = dtGrdBd.CurrentRow.Cells("bd_serie").Value
            titreBd = titreBd.replace(" ", "")
            Dim editeurBd = dtGrdBd.CurrentRow.Cells("edi_code").Value
            Dim cheminFinal = editeurBd + "\" + titreBd + "\" + titreBd + "_" + numTome
            cheminFinal = cheminFinal.ToLower()
            My.Computer.FileSystem.CopyFile(ImageBd, "C:\Images_Mediatheque\" & cheminFinal & ".jpg", True)
            If dtGrdBd.CurrentRow.Cells("bd_image").Value = "" Then
                cheminFinal = cheminFinal.replace("\", "\\")
                cmd = New MySqlCommand
                cmd.Connection = MaCxionMySql
                cmd.CommandText = "UPDATE Bd SET bd_image = '" & cheminFinal & "' WHERE bd_code Like '" & dtGrdBd.CurrentRow.Cells("bd_code").Value & "'"
                cmd.ExecuteNonQuery()
            End If
            MaDataTable.Clear()
            rempDatagridConnec()
            dtGrdBd.Refresh()
            Console.WriteLine(cheminFinal)
            MasqueBouton(True)
            btnValider.Visible = False
            btnParcourir.Visible = False
        End If
    End Sub

    Private Sub btnSupprimer_Click(sender As Object, e As EventArgs) Handles btnSupprimer.Click
        'Message de confirmation
        Dim rep As Integer = MsgBox("Etes-vous sûr de supprimer la couverture de  -- " & txtTitre.Text.ToUpper & "  -- ? ", MessageBoxButtons.YesNo + MessageBoxIcon.Warning, "IMPORTANT")

        If rep = 6 Then
            'Suppression de la couverture.
            requete = "UPDATE bd SET bd_image = '' WHERE bd_code = '" & txtCode.Text & "'"
            cmd = New MySqlCommand
            cmd.Connection = MaCxionMySql
            cmd.CommandText = requete
            cmd.ExecuteNonQuery()
            MaDataTable.Clear()
            rempDatagridConnec()
            dtGrdBd.Refresh()
            Raz()
        End If
    End Sub

#End Region

#Region "Recherche"
    'Au clic sur le bouton Chercher
    Private Sub btnValider_Click(sender As Object, e As EventArgs) Handles btnValider.Click

        req = "SELECT bd_code, bd_titre, bd_serie, bd_num_tome, bd_image, edi_code FROM BD "

        'Requêtes différentes selon le(s) critère(s) de recherche, donc le(s) champs rempli(s)
        If txtTitre.Text <> "" Then
            requete = req + "WHERE UPPER(bd_titre) LIKE '%" & txtTitre.Text.ToUpper & "%'"

            If txtNumTome.Text <> "" Then
                requete = requete + " AND bd_num_tome = '" & txtNumTome.Text & "'"

            ElseIf txtSerie.Text <> "" Then
                requete = requete + " AND UPPER(bd_serie) LIKE '%" & txtSerie.Text.ToUpper & "%'"

            ElseIf txtEditeur.Text <> "" Then
                requete = requete + " AND UPPER(edi_code) LIKE '%" & txtEditeur.Text.ToUpper & "%'"
            End If
        End If

        If txtNumTome.Text <> "" Then
            requete = req + "WHERE bd_num_tome = '" & txtNumTome.Text & "'"

            If txtTitre.Text <> "" Then
                requete = requete + " AND bd_titre = '" & txtTitre.Text & "'"

            ElseIf txtSerie.Text <> "" Then
                requete = requete + " AND UPPER(bd_serie) LIKE '%" & txtSerie.Text.ToUpper & "%'"

            ElseIf txtEditeur.Text <> "" Then
                requete = requete + " AND UPPER(edi_code) LIKE '%" & txtEditeur.Text.ToUpper & "%'"
            End If
        End If

        If txtSerie.Text <> "" Then
            requete = req + "WHERE UPPER(bd_serie) LIKE '%" & txtSerie.Text.ToUpper & "%'"

            If txtNumTome.Text <> "" Then
                requete = requete + " AND bd_num_tome = '" & txtNumTome.Text & "'"

            ElseIf txtTitre.Text <> "" Then
                requete = requete + " AND UPPER(bd_titre) LIKE '%" & txtTitre.Text.ToUpper & "%'"

            ElseIf txtEditeur.Text <> "" Then
                requete = requete + " AND UPPER(edi_code) LIKE '%" & txtEditeur.Text.ToUpper & "%'"
            End If
        End If

        If txtEditeur.Text <> "" Then
            requete = req + "WHERE UPPER(edi_code) LIKE '%" & txtEditeur.Text.ToUpper & "%'"

            If txtNumTome.Text <> "" Then
                requete = requete + " AND bd_num_tome = '" & txtNumTome.Text & "'"

            ElseIf txtSerie.Text <> "" Then
                requete = requete + " AND UPPER(bd_serie) LIKE '%" & txtSerie.Text.ToUpper & "%'"

            ElseIf txtTitre.Text <> "" Then
                requete = requete + " AND UPPER(bd_titre) LIKE '%" & txtTitre.Text.ToUpper & "%'"
            End If
        End If

        'Remplissage du DataGridView en fonction de la recherche effectuée
        cmd = New MySqlCommand
        cmd.Connection = MaCxionMySql
        cmd.CommandText = requete
        cmd.ExecuteNonQuery()
        MaDataTable.Clear()
        MonAdapter.SelectCommand = New MySqlCommand(requete, MaCxionMySql)
        MonAdapter.Fill(UnDataset, "bd")
        MaDataTable = UnDataset.Tables("bd")
        dtGrdBd.DataSource = MaDataTable
        dtGrdBd.Columns("bd_code").Visible = False
        dtGrdBd.Columns("bd_titre").HeaderText = "Titre"
        dtGrdBd.Columns("bd_titre").Width = 127
        dtGrdBd.Columns("bd_serie").HeaderText = "Série"
        dtGrdBd.Columns("bd_serie").Width = 101
        dtGrdBd.Columns("bd_num_tome").HeaderText = "Tome"
        dtGrdBd.Columns("bd_num_tome").Width = 35
        dtGrdBd.Columns("bd_image").HeaderText = "Adresse de l'image"
        dtGrdBd.Columns("bd_image").Width = 215
        dtGrdBd.Columns("edi_code").HeaderText = "Editeur"
        dtGrdBd.Columns("edi_code").Width = 80

        RazBouton(True)
        txtTitre.Enabled = False
        txtNumTome.Enabled = False
        txtSerie.Enabled = False
        txtEditeur.Enabled = False
    End Sub

    'Bouton qui permet de rechercher les livres qui n'ont pas de couverture
    Private Sub btnSansCouv_Click(sender As Object, e As EventArgs) Handles btnSansCouv.Click
        requete = "SELECT bd_code, bd_titre, bd_serie, bd_num_tome, bd_image, edi_code FROM BD WHERE bd_image LIKE ''"
        Console.WriteLine(requete)
        cmd = New MySqlCommand
        cmd.Connection = MaCxionMySql
        cmd.CommandText = requete
        cmd.ExecuteNonQuery()
        MaDataTable.Clear()
        MonAdapter.SelectCommand = New MySqlCommand(requete, MaCxionMySql)
        MonAdapter.Fill(UnDataset, "bd")
        MaDataTable = UnDataset.Tables("bd")
        dtGrdBd.DataSource = MaDataTable
        dtGrdBd.Columns("bd_code").Visible = False
        dtGrdBd.Columns("bd_titre").HeaderText = "Titre"
        dtGrdBd.Columns("bd_titre").HeaderText = "Titre"
        dtGrdBd.Columns("bd_titre").Width = 127
        dtGrdBd.Columns("bd_serie").HeaderText = "Série"
        dtGrdBd.Columns("bd_serie").Width = 101
        dtGrdBd.Columns("bd_num_tome").HeaderText = "Tome"
        dtGrdBd.Columns("bd_num_tome").Width = 35
        dtGrdBd.Columns("bd_image").HeaderText = "Adresse de l'image"
        dtGrdBd.Columns("bd_image").Width = 215
        dtGrdBd.Columns("edi_code").HeaderText = "Editeur"
        dtGrdBd.Columns("edi_code").Width = 80

        RazBouton(True)
        txtTitre.Enabled = False
        txtNumTome.Enabled = False
        txtSerie.Enabled = False
        txtEditeur.Enabled = False
    End Sub

    'Clic sur le bouton Parcourir : recherche d'image dans l'explorateur
    Private Sub btnParcourir_Click(sender As Object, e As EventArgs) Handles btnParcourir.Click
        Dim OpenFile As New OpenFileDialog
        OpenFile.InitialDirectory = "C:\Images_Mediatheque\dispo"
        OpenFile.FileName = ""
        OpenFile.Filter = "Fichier Image (*.jpg)|*.jpg"
        OpenFile.ShowDialog()
        ImageBd = OpenFile.FileName
        If ImageBd = "" Then

        Else
            btnSauvegarder.Enabled = True
            pictBoxImage.ImageLocation = ImageBd
        End If
    End Sub

#End Region

#Region "Contrôle de saisie"
    'N'autorise que la saisie de chiffres pour le numéro de tome
    Private Sub txtNumTome_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtNumTome.KeyPress
        If Char.IsDigit(e.KeyChar) = False And Asc((e.KeyChar)) <> System.Windows.Forms.Keys.Back Then
            e.Handled = True
        End If
    End Sub

#End Region

End Class