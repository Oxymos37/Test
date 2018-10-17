
Imports System.Data.SqlClient
Imports MySql.Data.MySqlClient
Public Class FrmLivres
    Dim MonAuteur As String
    Dim Ajout As Boolean
    Dim MonAdapter As New MySqlDataAdapter
    Dim MaRequete As String
    Dim requeteAjoutAuteur As String
    Dim requeteModifAuteur As String
    Dim UnDataset As New DataSet
    Dim MaDataTable As DataTable
    Dim MesAuteurs As DataTable
    Dim maTableCombo As DataTable
    Dim cmd As MySqlCommand
    Dim requete As String
    Dim requeteAut As String
    Dim requeteAut2 As String
    Dim requetedoublon As String
    Dim reponse As String
    Dim Annee As String
    Dim Droit As String
    Dim verif As Integer
    Dim Id As String
    Dim Lid As Integer


    Private Sub FrmLivres_Load(sender As Object, e As EventArgs) Handles Me.Load
        MajEcran()
        rempDatagridConnec()
        RempCbxAnnee()
        btnSauvegarder.Tag = ""
    End Sub

#Region "DataGridView"

    'Remplissage du DataGrid
    Public Sub rempDatagridConnec()
        MaRequete = "SELECT DISTINCT (bd.bd_code), bd_titre, bd_num_tome, bd_annee_parution, bd_serie, edi_code FROM bd"
        MonAdapter.SelectCommand = New MySqlCommand(MaRequete, MaCxionMySql)
        MonAdapter.Fill(UnDataset, "livres")
        MaDataTable = UnDataset.Tables("livres")
        dtGrdLivres.DataSource = MaDataTable
        dtGrdLivres.Columns("bd_code").HeaderText = "Code"
        dtGrdLivres.Columns("bd_code").Width = 40
        dtGrdLivres.Columns("bd_titre").HeaderText = "Titre"
        dtGrdLivres.Columns("bd_titre").Width = 240
        dtGrdLivres.Columns("bd_num_tome").HeaderText = "Tome"
        dtGrdLivres.Columns("bd_num_tome").Width = 52
        dtGrdLivres.Columns("bd_annee_parution").HeaderText = "Parution"
        dtGrdLivres.Columns("bd_annee_parution").Width = 50
        dtGrdLivres.Columns("bd_serie").HeaderText = "Serie"
        dtGrdLivres.Columns("bd_serie").Width = 80
        dtGrdLivres.Columns("edi_code").HeaderText = "Editeur"
        dtGrdLivres.Columns("edi_code").Width = 90

        dtGrdLivres.Height = 200
    End Sub

    'Remplissage des champs *txt au click
    Private Sub dtGrdLivres_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dtGrdLivres.CellClick
        If btnSauvegarder.Tag <> "A" Then
            txtCode.Text = dtGrdLivres.CurrentRow.Cells("bd_code").Value
            txtTitre.Text = dtGrdLivres.CurrentRow.Cells("bd_titre").Value
            txtNumTome.Text = dtGrdLivres.CurrentRow.Cells("bd_num_tome").Value
            txtSerie.Text = dtGrdLivres.CurrentRow.Cells("bd_serie").Value
            txtEditeur.Text = dtGrdLivres.CurrentRow.Cells("edi_code").Value
            cbxAnnee.SelectedItem = Trim(dtGrdLivres.CurrentRow.Cells("bd_annee_parution").Value)
            Annee = dtGrdLivres.CurrentRow.Cells("bd_annee_parution").Value

            requete = "SELECT aut_code FROM ecrire WHERE bd_code like " & txtCode.Text
            MonAdapter.SelectCommand = New MySqlCommand(requete, MaCxionMySql)
            MonAdapter.Fill(UnDataset, "ecrire")
            MesAuteurs = UnDataset.Tables("ecrire")

            Dim nbRows As Integer = MesAuteurs.Rows.Count
            If nbRows = 0 Then
                txtAuteur.Text = ""
                txtAuteur2.Text = ""
            End If
            If nbRows = 1 Then
                txtAuteur.Text = MesAuteurs.Rows(0).Item("aut_code").ToString
                txtAuteur2.Text = ""
            End If
            If nbRows = 2 Then
                txtAuteur.Text = MesAuteurs.Rows(0).Item("aut_code").ToString
                txtAuteur2.Text = MesAuteurs.Rows(1).Item("aut_code").ToString
            End If
            MesAuteurs.Clear()

        End If
    End Sub

    'Rafraichissement des champs *txt au changement de ligne
    'Private Sub dtGrdLivres_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dtGrdLivres.CellEnter
    '    If btnSauvegarder.Tag <> "A" Then
    '        txtCode.Text = dtGrdLivres.CurrentRow.Cells("bd_code").Value
    '        txtTitre.Text = dtGrdLivres.CurrentRow.Cells("bd_titre").Value
    '        txtNumTome.Text = dtGrdLivres.CurrentRow.Cells("bd_num_tome").Value
    '        cbxAnnee.SelectedItem = Trim(dtGrdLivres.CurrentRow.Cells("bd_annee_parution").Value)
    '        txtSerie.Text = dtGrdLivres.CurrentRow.Cells("bd_serie").Value
    '        txtEditeur.Text = dtGrdLivres.CurrentRow.Cells("edi_code").Value

    '        requete = "SELECT aut_code FROM ecrire WHERE bd_code like '" & txtTitre.Text & "' "
    '        cmd = New MySqlCommand
    '        cmd.Connection = MaCxionMySql
    '        cmd.CommandText = requete
    '        cmd.ExecuteNonQuery()

    '        Dim nbRows As Integer = MaDataTable.Rows.Count
    '        If nbRows = 1 Then
    '            txtAuteur.Text = MaDataTable.Rows(0).Item("aut_code").ToString
    '        End If
    '        If nbRows = 2 Then
    '            txtAuteur2.Text = MaDataTable.Rows(1).Item("aut_code").ToString
    '        End If
    '    End If
    'End Sub

#End Region

#Region "Boutons"


    Private Sub btnAjouter_Click(sender As Object, e As EventArgs) Handles btnAjouter.Click


        Raz()

        unLock()

        txtTitre.Focus()

        Affichage()

        MasqueBouton(False)

        btnSauvegarder.Location = New Point(15, 31)

        btnAnnuler.Location = New Point(15, 111)

        btnSauvegarder.Tag = "A"

        RempCbxSerie()

        RempCbxEditeur()

        RempCbxAnnee()

        RempCbxAuteur()

        RempCbxAuteur2()



    End Sub

    Private Sub btnAnnuler_Click(sender As Object, e As EventArgs) Handles btnAnnuler.Click
        RazBouton(True)
        Raz()
        RempCbxAnnee()
        ResetColor()
        btnSauvegarder.Tag = ""
        Lock()
        MasqueChamp()
    End Sub

    Private Sub btnRetour_Click(sender As Object, e As EventArgs) Handles btnRetour.Click
        Me.Close()
        cbxAnnee.Items.Clear()
        RempCbxAnnee()
    End Sub

    Private Sub btnModifier_Click(sender As Object, e As EventArgs) Handles btnModifier.Click


        unLock()

        Affichage()

        MasqueBouton(False)

        btnSauvegarder.Tag = "M"

        RempCbxSerie()

        RempCbxAnnee()

        RempCbxEditeur()

        RempCbxAuteur()

        RempCbxAuteur2()


    End Sub

    Private Sub btnRechercher_Click(sender As Object, e As EventArgs) Handles btnRechercher.Click

        RempCbxAnnee()

        unLock()

        Affichage()
        lblAuteur.Visible = False
        txtAuteur.Visible = False
        cbxAuteur.Visible = False

        RempCbxEditeur()

        RempCbxSerie()

        RempCbxAuteur()

        RempCbxAuteur2()

        Raz()


        MasqueRecherche(False)


    End Sub

    Private Sub btnSauvegarder_Click(sender As Object, e As EventArgs) Handles btnSauvegarder.Click

        ResetColor()

        If (btnSauvegarder.Tag = "A") And ChampsValide() Then
            'gestion de l'échapement de l'apostrophe
            txtTitre.Text = txtTitre.Text.Replace("'", "\'")
            'gestion de la casse
            txtSerie.Text = txtSerie.Text.Substring(0, 1).ToUpper + txtSerie.Text.Substring(1).ToLower
            requeteAjoutAuteur = "INSERT INTO bd VALUES (0,'" & txtTitre.Text & "'," & txtNumTome.Text & "," & Annee & ",'','" & txtSerie.Text & "','" & txtEditeur.Text & "')"
            'requeteAjoutAuteur = "INSERT INTO Bd VALUES (NULL,'" & txtTitre.Text & "'," & txtNumTome.Text & "," & TxtAnnePar.Text & ",'','" & txtSerie.Text & "','" & txtEditeur.Text & "') "
            cmd = New MySqlCommand
            cmd.Connection = MaCxionMySql
            cmd.CommandText = requeteAjoutAuteur
            cmd.ExecuteNonQuery()

            'On fait la quete de recherche
            Try
                cmd.ExecuteNonQuery()
                MsgBox("Livre " & txtTitre.Text & " ajouté !")
                'Si l'auteur existe, une erreur est déclarée
            Catch ex As Exception
                MsgBox("ERREUR : Le tome " & txtNumTome.Text & " du livre " & txtTitre.Text & "  existe déjà !")
            End Try

        End If

        Dim Id As String
        Dim Lid As Integer

        Id = "SELECT LAST_INSERT_ID()"

        cmd = New MySqlCommand
        cmd.Connection = MaCxionMySql
        cmd.CommandText = Id
        Lid = cmd.ExecuteScalar()

        If (btnSauvegarder.Tag = "A") And ChampsValide() And txtAuteur.Text <> "" Then

            requeteAut = "INSERT INTO ecrire VALUES ( '" & Lid & "', '" & txtAuteur.Text & "')"

            cmd = New MySqlCommand
            cmd.Connection = MaCxionMySql
            cmd.CommandText = requeteAut
            cmd.ExecuteNonQuery()
        End If

        If (btnSauvegarder.Tag = "A") And ChampsValide() And txtAuteur2.Text <> "" Then

            requeteAut2 = "INSERT INTO ecrire VALUES ( '" & Lid & "', '" & txtAuteur2.Text & "')"

            cmd = New MySqlCommand
            cmd.Connection = MaCxionMySql
            cmd.CommandText = requeteAut2
            cmd.ExecuteNonQuery()
        End If

        MasqueBouton(True)
        MaDataTable.Clear()
        rempDatagridConnec()
        dtGrdLivres.Refresh()
        Lock()
        If (btnSauvegarder.Tag = "M") And ChampsValide() Then
            'gestion de l'échapement de l'apostrophe
            txtTitre.Text = txtTitre.Text.Replace("'", "\'")
            'gestion de la casse
            txtSerie.Text = txtSerie.Text.Substring(0, 1).ToUpper + txtSerie.Text.Substring(1).ToLower
            requeteModifAuteur = " UPDATE Bd SET bd_titre = '" & txtTitre.Text & "', bd_num_tome = " & txtNumTome.Text & ", bd_annee_parution = " & Annee & " , bd_serie = '" & txtSerie.Text & "', edi_code = '" & txtEditeur.Text & "' WHERE bd_code = '" & txtCode.Text & "'"
            cmd = New MySqlCommand
            cmd.Connection = MaCxionMySql
            cmd.CommandText = requeteModifAuteur
            cmd.ExecuteNonQuery()


            Id = "SELECT LAST_INSERT_ID()"

            cmd = New MySqlCommand
            cmd.Connection = MaCxionMySql
            cmd.CommandText = Id
            Lid = cmd.ExecuteScalar()

            If (btnSauvegarder.Tag = "M") And ChampsValide() And txtAuteur.Text <> "" Then

                requeteAut = "INSERT INTO ecrire VALUES ( '" & Lid & "', '" & txtAuteur.Text & "')"

                cmd = New MySqlCommand
                cmd.Connection = MaCxionMySql
                cmd.CommandText = requeteAut
                cmd.ExecuteNonQuery()
            End If

            If (btnSauvegarder.Tag = "M") And ChampsValide() And txtAuteur2.Text <> "" Then

                requeteAut2 = "INSERT INTO ecrire VALUES ( '" & Lid & "', '" & txtAuteur2.Text & "')"

                cmd = New MySqlCommand
                cmd.Connection = MaCxionMySql
                cmd.CommandText = requeteAut2
                cmd.ExecuteNonQuery()
            End If


            MasqueBouton(True)
            MaDataTable.Clear()
            rempDatagridConnec()
            dtGrdLivres.Refresh()
            Lock()
        End If
        btnSauvegarder.Tag = ""

    End Sub

    Private Sub btnSupprimer_Click(sender As Object, e As EventArgs) Handles btnSupprimer.Click
        If txtTitre.Text = "" Then
            MsgBox("Aucun livre selectionné ", MessageBoxButtons.OK + MessageBoxIcon.Warning, "IMPORTANT")

        Else
            Dim rep As Integer = MsgBox("Etes-vous sur de supprimer le livre  -- " & txtTitre.Text.ToUpper & "  -- ? ", MessageBoxButtons.YesNo + MessageBoxIcon.Warning, "IMPORTANT")

            If rep = 6 Then
                requete = "DELETE FROM bd WHERE bd_code Like '" & txtCode.Text & "'"
                cmd = New MySqlCommand
                cmd.Connection = MaCxionMySql
                cmd.CommandText = requete
                cmd.ExecuteNonQuery()
                MaDataTable.Clear()
                rempDatagridConnec()
                dtGrdLivres.Refresh()
                Raz()
                Dim reponse As Integer = MsgBox("Le livre a bien était supprimé", MessageBoxButtons.OK)
            End If

        End If


    End Sub

    Private Sub btnValider_Click(serder As Object, e As EventArgs) Handles btnValider.Click


        'requete selon le nom du livre
        requete = "SELECT bd_code, bd_titre, bd_num_tome, bd_annee_parution, bd_serie, edi_code FROM bd "

        'initialisation de la variable verifiant les champs remplis ou non
        verif = 0

        'si le textfield Auteur est rempli, alors la requete prends le where et la jointure

        If txtAuteur2.Text <> "" Then
            requete = requete + "WHERE aut_code like " & txtAuteur.Text & " "
            verif += 1
        End If

        'ajout du where si l'utilisateur n'a pas rempli le textfield ci dessus
        If txtTitre.Text <> "" Then
            If verif = 0 Then
                requete = requete + "WHERE upper(bd_Titre) like '%" & txtTitre.Text.ToUpper & "%' "
                verif += 1
            Else
                requete = requete + "AND bd_Titre like " & txtTitre.Text & " "
            End If

        End If
        If txtEditeur.Text <> "" Then
            If (verif = 0) Then
                requete = requete + "WHERE edi_code like " & txtEditeur.Text & " "
                verif += 1
            Else
                requete = requete + "AND edi_code like " & txtEditeur.Text & " "
            End If
        End If
        If cbxAnnee.Text <> "" Then
            If (verif = 0) Then
                requete = requete + "WHERE bd_annee_parution = " & cbxAnnee.Text & " "
                verif += 1
            Else
                requete = requete + "AND bd_annee_parution = " & cbxAnnee.Text & " "
            End If
        End If
        If txtSerie.Text <> "" Then
            If (verif = 0) Then
                requete = requete + "WHERE bd_serie like " & txtSerie.Text & " "
                verif += 1
            Else
                requete = requete + "AND bd_serie like " & txtSerie.Text & " "
            End If
            'Si toutes les conditions sont remplies, l'ajout est fait
        End If


        MaDataTable.Clear()
        MonAdapter.SelectCommand = New MySqlCommand(requete, MaCxionMySql)
        MonAdapter.Fill(UnDataset, "bd")
        MaDataTable = UnDataset.Tables("bd")
        'création du datagridview en fonction de la recherche
        dtGrdLivres.DataSource = MaDataTable
        dtGrdLivres.Columns("bd_code").HeaderText = "Code"
        dtGrdLivres.Columns("bd_code").Width = 40
        dtGrdLivres.Columns("bd_titre").HeaderText = "Titre"
        dtGrdLivres.Columns("bd_titre").Width = 240
        dtGrdLivres.Columns("bd_num_Tome").HeaderText = "Tome"
        dtGrdLivres.Columns("bd_num_Tome").Width = 52
        dtGrdLivres.Columns("bd_Annee_Parution").HeaderText = "Parution"
        dtGrdLivres.Columns("bd_Annee_Parution").Width = 50
        dtGrdLivres.Columns("bd_Serie").HeaderText = "Serie"
        dtGrdLivres.Columns("bd_Serie").Width = 80
        dtGrdLivres.Columns("edi_code").HeaderText = "Editeur"
        dtGrdLivres.Columns("edi_code").Width = 90
        'dtGrdLivres.Columns("bd_Etat").HeaderText = "Etat"
        'dtGrdLivres.Columns("bd_Etat").Width = 90
        'affichage de tous les champs

        MasqueRecherche(True)
    End Sub


#End Region

#Region "Procédures publiques"

    Public Sub Raz()
        txtCode.Text = ""
        txtTitre.Text = ""
        txtNumTome.Text = ""
        txtSerie.Text = ""
        txtEditeur.Text = ""
        txtAuteur.Text = ""
        txtAuteur2.Text = ""
        cbxAnnee.Items.Clear()

    End Sub

    Public Sub MasqueBouton(valider As Boolean)
        btnModifier.Visible = valider
        btnRechercher.Visible = valider
        btnSupprimer.Visible = valider
        btnAjouter.Visible = valider
        btnSauvegarder.Visible = Not (valider)
        btnAnnuler.Visible = Not (valider)
        btnSauvegarder.Location = New Point(15, 31)
        btnAnnuler.Location = New Point(15, 111)

    End Sub

    Public Sub MasqueRecherche(valider As Boolean)
        btnModifier.Visible = valider
        btnRechercher.Visible = valider
        btnSupprimer.Visible = valider
        btnAjouter.Visible = valider
        btnSauvegarder.Visible = False
        btnValider.Visible = Not (valider)
        btnAnnuler.Visible = Not (valider)
        btnValider.Location = New Point(15, 31)
        btnAnnuler.Location = New Point(15, 111)

    End Sub

    Public Sub RazBouton(valider As Boolean)
        btnModifier.Visible = valider
        btnRechercher.Visible = valider
        btnSupprimer.Visible = valider
        btnAjouter.Visible = valider
        btnSauvegarder.Visible = Not (valider)
        btnValider.Visible = Not (valider)
        btnAnnuler.Visible = Not (valider)

    End Sub

    Public Sub RempCbxAnnee()
        'Remplissage de la combobox Année
        For i As Integer = 1940 To Date.Now.Year
            cbxAnnee.Items.Add(i.ToString)
        Next i
        cbxAnnee.Items.Add("")
        cbxAnnee.Refresh()
    End Sub

    Public Sub RempCbxSerie()
        'remplissage des combo
        requete = "SELECT DISTINCT(bd_serie) FROM BD ORDER BY 1"
        MonAdapter.SelectCommand = New MySqlCommand(requete, MaCxionMySql)
        MonAdapter.Fill(UnDataset, "bd_serie")
        'maTableCombo.Clear()
        maTableCombo = UnDataset.Tables("bd_serie")
        cbxSerie.DataSource = maTableCombo
        ''champ affiché
        cbxSerie.DisplayMember = "bd_serie"
        cbxSerie.ValueMember = "bd_serie"
    End Sub

    Public Sub RempCbxEditeur()
        requete = "SELECT edi_code, edi_nom FROM EDITEUR"
        MonAdapter.SelectCommand = New MySqlCommand(requete, MaCxionMySql)
        MonAdapter.Fill(UnDataset, "editeur")
        maTableCombo = UnDataset.Tables("editeur")
        cbxEditeur.DataSource = maTableCombo
        'champ affiché
        cbxEditeur.DisplayMember = "NomEditeur"
        'valeur du champ caché, ici le code
        cbxEditeur.ValueMember = "edi_code"
    End Sub

    Public Sub RempCbxAuteur()
        requete = "SELECT aut_code, aut_nom FROM AUTEUR ORDER BY 1"
        MonAdapter.SelectCommand = New MySqlCommand(requete, MaCxionMySql)
        MonAdapter.Fill(UnDataset, "auteur")
        maTableCombo = UnDataset.Tables("auteur")
        cbxAuteur.DataSource = maTableCombo
        'champ affiché
        cbxAuteur.DisplayMember = "aut_nom"
        'valeur du champ caché, ici le code
        cbxAuteur.ValueMember = "aut_code"

    End Sub

    Public Sub RempCbxAuteur2()
        requete = "SELECT aut_code, aut_nom FROM AUTEUR ORDER BY 1"
        MonAdapter.SelectCommand = New MySqlCommand(requete, MaCxionMySql)
        MonAdapter.Fill(UnDataset, "auteur")
        maTableCombo = UnDataset.Tables("auteur")
        cbxAuteur2.DataSource = maTableCombo
        'champ affiché
        cbxAuteur2.DisplayMember = "aut_nom"
        'valeur du champ caché, ici le code
        cbxAuteur2.ValueMember = "aut_code"

    End Sub

#End Region

#Region "Procédures privées"

    Private Sub Lock()
        txtTitre.Enabled = False
        txtNumTome.Enabled = False
        txtSerie.Enabled = False
        cbxAnnee.Enabled = False
        txtEditeur.Enabled = False
        cbxEditeur.Enabled = False
        cbxSerie.Enabled = False

    End Sub

    Private Sub unLock()
        txtTitre.Enabled = True
        txtNumTome.Enabled = True
        txtSerie.Enabled = True
        cbxAnnee.Enabled = True
        txtEditeur.Enabled = False
        cbxEditeur.Enabled = True
        cbxSerie.Enabled = True
    End Sub

    Private Sub Affichage()
        txtTitre.Visible = True
        txtNumTome.Visible = True
        txtSerie.Visible = True
        cbxAnnee.Visible = True
        txtEditeur.Visible = True
        cbxEditeur.Visible = True
        cbxSerie.Visible = True
        cbxAuteur.Visible = True
        cbxAuteur2.Visible = True
    End Sub

    Private Sub MasqueChamp()
        cbxEditeur.Visible = False
        cbxSerie.Visible = False
        cbxAuteur.Visible = False
        cbxAuteur2.Visible = False
    End Sub

    Private Sub MajEcran()
        txtCode.Enabled = False
        txtTitre.Enabled = False
        txtNumTome.Enabled = False
        txtAuteur.Enabled = False
        txtAuteur2.Enabled = False
        cbxAnnee.Enabled = False
        txtSerie.Enabled = False
        txtEditeur.Enabled = False
        btnValider.Visible = False
        btnSauvegarder.Visible = False
        btnAnnuler.Visible = False
        cbxEditeur.Visible = False
        cbxSerie.Visible = False
        cbxAuteur.Visible = False
        cbxAuteur2.Visible = False
    End Sub

    Private Sub ResetColor()
        txtTitre.BackColor = Color.White
        txtSerie.BackColor = Color.White
        txtEditeur.BackColor = Color.White
    End Sub

    Private Sub cbxSerie_DropDownClosed(sender As Object, e As EventArgs) Handles cbxSerie.DropDownClosed
        txtSerie.Text = DirectCast(cbxSerie.SelectedItem, DataRowView).Item("bd_serie")
    End Sub

    Private Sub cbxAuteur_DropDownClosed(sender As Object, e As EventArgs) Handles cbxAuteur.DropDownClosed
        txtAuteur.Text = DirectCast(cbxAuteur.SelectedItem, DataRowView).Item("aut_nom")
    End Sub

    Private Sub cbxAuteur2_DropDownClosed(sender As Object, e As EventArgs) Handles cbxAuteur2.DropDownClosed
        txtAuteur2.Text = DirectCast(cbxAuteur2.SelectedItem, DataRowView).Item("aut_nom")
    End Sub

    Private Sub cbxEditeur_DropDownClosed(sender As Object, e As EventArgs) Handles cbxEditeur.DropDownClosed
        txtEditeur.Text = CType(cbxEditeur.SelectedValue, String)
    End Sub

    Private Sub txtNumTome_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtNumTome.KeyPress
        If Char.IsDigit(e.KeyChar) = False And Asc((e.KeyChar)) <> System.Windows.Forms.Keys.Back Then
            e.Handled = True
        End If
    End Sub

#End Region

#Region "Fonction Privées"

    Private Function ChampsValide() As Boolean
        Dim valide As Boolean = True
        If (txtTitre.Text = "") Then
            txtTitre.BackColor = Color.Yellow
            valide = False
        End If
        If (txtSerie.Text = "") Then
            txtSerie.BackColor = Color.Yellow
            valide = False
        End If
        If (txtEditeur.Text = "") Then
            txtEditeur.BackColor = Color.Yellow
            valide = False
        End If
        Return valide
    End Function

    Private Sub ComboBox1_DropDownClosed(sender As Object, e As EventArgs) Handles cbxAnnee.DropDownClosed
        Annee = Me.cbxAnnee.SelectedItem
    End Sub


#End Region

End Class