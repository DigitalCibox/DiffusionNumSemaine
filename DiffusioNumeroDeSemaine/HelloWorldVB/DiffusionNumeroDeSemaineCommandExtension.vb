'=====================================================================
'  
'  This file is part of the Autodesk Vault API Code Samples.
'
'  Copyright (C) Autodesk Inc.  All rights reserved.
'
'THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
'KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
'IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
'PARTICULAR PURPOSE.
'=====================================================================


Imports System.Collections.Generic
Imports System.Linq
Imports System.Reflection
Imports System.Data
Imports System.Text
Imports System.Windows.Forms
Imports System.ComponentModel

Imports Autodesk.Connectivity.Explorer.Extensibility
Imports Autodesk.DataManagement.Client.Framework.Vault.Currency.Entities

Imports Autodesk.Connectivity.WebServices
Imports Autodesk.Connectivity.WebServicesTools
Imports VDF = Autodesk.DataManagement.Client.Framework


' These 5 assembly attributes must be specified or your extension will not load. 
<Assembly: AssemblyCompany("Cibox")>
<Assembly: AssemblyProduct("Cibox2022DiffusionNSExtension")>
<Assembly: AssemblyDescription("Cibox App")>

' The extension ID needs to be unique for each extension.  
' Make sure to generate your own ID when writing your own extension. 
<Assembly: Autodesk.Connectivity.Extensibility.Framework.ExtensionId("7ADC0766-F085-46d7-A2EB-C68F79CBF4E7")> 

' This number gets incremented for each Vault release.
<Assembly: Autodesk.Connectivity.Extensibility.Framework.ApiVersion("13.0")>


Namespace DiffusionNumeroDeSemaine

    ''' <summary>
    ''' This class implements the IExtension interface, which means it tells Vault Explorer what 
    ''' commands and custom tabs are provided by this extension.
    ''' </summary>
    Public Class DiffusionNumeroDeSemaineCommandExtension
        Implements IExplorerExtension

#Region "IExtension Members"

        ''' <summary>
        ''' This function tells Vault Explorer what custom commands this extension provides.
        ''' Part of the IExtension interface.
        ''' </summary>
        ''' <returns>A collection of CommandSites, which are collections of custom commands.</returns>
        Public Function CommandSites() As IEnumerable(Of CommandSite) Implements IExplorerExtension.CommandSites
            ' Create the Hello World command object.


            ' this command is not active if there are multiple entities selected
            Dim DiffusionNsemaineCmdItem As New CommandItem("DiffusionNuméroDeSemaine", "Diffusion N° de semaine...") With {
             .NavigationTypes = New SelectionTypeId() {SelectionTypeId.Folder},
             .MultiSelectEnabled = False
            }

            ' The HelloWorldCommandHandler function is called when the custom command is executed.
            AddHandler DiffusionNsemaineCmdItem.Execute, AddressOf DiffusionNsemainemCommandHandler

            ' Create a command site to hook the command to the Advanced toolbar

            Dim toolbarCmdSite As New CommandSite("DiffusionNuméroDeSemaine", "Diffusion N° de semaine...") With {
            .Location = CommandSiteLocation.AdvancedToolbar,
            .DeployAsPulldownMenu = False
             }
            toolbarCmdSite.AddCommand(DiffusionNsemaineCmdItem)

            ' Create another command site to hook the command to the right-click menu for Files.

            Dim fileContextCmdSite As New CommandSite("DiffusionNuméroDeSemaine", "Diffusion N° de semaine...") With {
             .Location = CommandSiteLocation.FileContextMenu,
             .DeployAsPulldownMenu = False
            }
            fileContextCmdSite.AddCommand(DiffusionNsemaineCmdItem)

            ' Now the custom command is available in 2 places.

            ' Gather the sites in a List.
            Dim sites As New List(Of CommandSite)()
            sites.Add(toolbarCmdSite)
            sites.Add(fileContextCmdSite)

            ' Return the list of CommandSites.
            Return sites
        End Function


        ''' <summary>
        ''' This function tells Vault Explorer what custom tabs this extension provides.
        ''' Part of the IExtension interface.
        ''' </summary>
        ''' <returns>A collection of DetailTabs, each object represents a custom tab.</returns>
        Public Function DetailTabs() As IEnumerable(Of DetailPaneTab) Implements IExplorerExtension.DetailTabs
            ' Create a DetailPaneTab list to return from method
            Dim fileTabs As New List(Of DetailPaneTab)()

            ' Create Selection Info tab for Files
            Dim filePropertyTab As New DetailPaneTab("File.Tab.PropertyGrid", "Selection Info", SelectionTypeId.File, GetType(MyCustomTabControl))

            ' The propertyTab_SelectionChanged is called whenever our tab is active and the selection changes in the 
            ' main grid.
            AddHandler filePropertyTab.SelectionChanged, AddressOf propertyTab_SelectionChanged
            fileTabs.Add(filePropertyTab)

            ' Create Selection Info tab for Folders
            Dim folderPropertyTab As New DetailPaneTab("Folder.Tab.PropertyGrid", "Selection Info", SelectionTypeId.Folder, GetType(MyCustomTabControl))
            AddHandler folderPropertyTab.SelectionChanged, AddressOf propertyTab_SelectionChanged
            fileTabs.Add(folderPropertyTab)

            ' Create Selection Info tab for Items
            Dim itemPropertyTab As New DetailPaneTab("Item.Tab.PropertyGrid", "Selection Info", SelectionTypeId.Item, GetType(MyCustomTabControl))
            AddHandler itemPropertyTab.SelectionChanged, AddressOf propertyTab_SelectionChanged
            fileTabs.Add(itemPropertyTab)

            ' Create Selection Info tab for Change Orders
            Dim coPropertyTab As New DetailPaneTab("Co.Tab.PropertyGrid", "Selection Info", SelectionTypeId.ChangeOrder, GetType(MyCustomTabControl))
            AddHandler coPropertyTab.SelectionChanged, AddressOf propertyTab_SelectionChanged
            fileTabs.Add(coPropertyTab)

            ' Return tabs
            Return fileTabs
        End Function




        ''' <summary>
        ''' This function is called after the user logs in to the Vault Server.
        ''' Part of the IExtension interface.
        ''' </summary>
        ''' <param name="application">Provides information about the running application.</param>
        Public Sub OnLogOn(application As IApplication) Implements IExplorerExtension.OnLogOn

        End Sub

        ''' <summary>
        ''' This function is called after the user is logged out of the Vault Server.
        ''' Part of the IExtension interface.
        ''' </summary>
        ''' <param name="application">Provides information about the running application.</param>
        Public Sub OnLogOff(application As IApplication) Implements IExplorerExtension.OnLogOff

        End Sub

        ''' <summary>
        ''' This function is called before the application is closed.
        ''' Part of the IExtension interface.
        ''' </summary>
        ''' <param name="application">Provides information about the running application.</param>
        Public Sub OnShutdown(application As IApplication) Implements IExplorerExtension.OnShutdown
            ' Although this function is empty for this project, it's still needs to be defined 
            ' because it's part of the IExtension interface.
        End Sub

        ''' <summary>
        ''' This function is called after the application starts up.
        ''' Part of the IExtension interface.
        ''' </summary>
        ''' <param name="application">Provides information about the running application.</param>
        Public Sub OnStartup(application As IApplication) Implements IExplorerExtension.OnStartup
            ' Although this function is empty for this project, it's still needs to be defined 
            ' because it's part of the IExtension interface.
        End Sub

        ''' <summary>
        ''' This function tells Vault Exlorer which default commands should be hidden.
        ''' Part of the IExtension interface.
        ''' </summary>
        ''' <returns>A collection of command names.</returns>
        Public Function HiddenCommands() As IEnumerable(Of String) Implements IExplorerExtension.HiddenCommands
            ' This extension does not hide any commands.
            Return Nothing
        End Function

        ''' <summary>
        ''' This function allows the extension to define special behavior for Custom Entity types.
        ''' Part of the IExtension interface.
        ''' </summary>
        ''' <returns>A collection of CustomEntityHandler objects.  Each object defines special behavior
        ''' for a specific Custom Entity type.</returns>
        Public Function CustomEntityHandlers() As IEnumerable(Of CustomEntityHandler) Implements IExplorerExtension.CustomEntityHandlers
            ' This extension does not provide special Custom Entity behavior.
            Return Nothing
        End Function

#End Region


        ''' <summary>
        ''' This is the function that is called whenever the custom command is executed.
        ''' </summary>
        ''' <param name="s">The sender object.  Usually not used.</param>
        ''' <param name="e">The event args.  Provides additional information about the environment.</param>
        Private Sub DiffusionNsemainemCommandHandler(s As Object, e As CommandItemEventArgs)

            Dim PF_Nom(0) As Long
            Dim PF_Gam(0) As Long
            Dim PF_Nco(0) As Long
            Dim PF_Qua(0) As Long

            Dim Quantite As Integer = 0

            Dim PCF_Nom(0) As Long
            Dim PCF_Gam(0) As Long
            Dim PCF_Nco(0) As Long
            Dim PCF_Qua(0) As Long

            Dim propInstParamArray_new As PropInstParamArray()

            Dim LogFile As String = "C:\FichierLog_Vault_DiffusionPropriete2020.txt"
            If My.Computer.FileSystem.FileExists(LogFile) Then
                My.Computer.FileSystem.DeleteFile(LogFile)
            End If
            My.Computer.FileSystem.WriteAllText(LogFile, "TRACE D'EXECUTION DE L'EXTENSION : DIFFUSION N° DE LA SEMAINE V(10/06/2022)" + vbCrLf, True)

            Dim connection As VDF.Vault.Currency.Connections.Connection = e.Context.Application.Connection
            Dim checkedOutFile As File
            Dim loadedFile As FileIteration

            Try
                ' The Context part of the event args tells us information about what is selected.
                ' Run some checks to make sure that the selection is valid.

                Dim SelectedFoldersIds(0) As Long
                Dim P_ID(0) As Long
                Dim PF_ID(0) As Long
                Dim PCF_ID(0) As Long

                If e.Context.CurrentSelectionSet.Count() = 0 Then

                    MessageBox.Show("Rien n'est sélectionné")
                    My.Computer.FileSystem.WriteAllText(LogFile, "-->Rien n'est sélectionné" + vbCrLf, True)

                Else
                    ' we only have one item selected, which is the expected behavior

                    Dim selection As ISelection = e.Context.CurrentSelectionSet.First()
                    Dim mgr As WebServiceManager = connection.WebServiceManager

                    ' Look of the Folder object.  How we do this depends on what is selected.
                    Dim selectedFolder As Autodesk.Connectivity.WebServices.Folder = Nothing

                    If selection.TypeId = SelectionTypeId.Folder Then

                        ' our ISelection.Id is really a File.MasterId

                        selectedFolder = mgr.DocumentService.GetFolderById(selection.Id)

                        SelectedFoldersIds(0) = selection.Id
                        'MessageBox.Show("Le dossier sélectionné est : " & selectedFolder.Name)
                        My.Computer.FileSystem.WriteAllText(LogFile, "-->Le dossier sélectionné est :" + selectedFolder.Name + vbCrLf, True)

                    End If

                    If selectedFolder Is Nothing Or selectedFolder.Name.Contains("-") Or selectedFolder.Name.Contains("_") Then
                        MessageBox.Show("Veuillez sélectionner le dossier de la commande")
                        My.Computer.FileSystem.WriteAllText(LogFile, "-->Le dossier sélectionné n'est pas un dossier" + vbCrLf, True)
                    Else
                        ' this is the message we hope to see 
                        Dim Nsemaine As String

                        Dim formulaire As New Form2()
                        formulaire.ShowDialog()
                        If Convert.ToInt32(formulaire._numSem, 10) <= 9 Then
                            Nsemaine = "0" + Convert.ToInt32(formulaire._numSem, 10).ToString
                        Else
                            Nsemaine = Convert.ToInt32(formulaire._numSem, 10).ToString
                        End If


                        Nsemaine = "S" + Nsemaine + "-" + formulaire._numAnnee.Substring(2)


                        'La recherche de l'ID de la proriété N° de semaine 

                        Dim entityClassId_folder As String = Autodesk.DataManagement.Client.Framework.Vault.Currency.Entities.EntityClassIds.Folder
                        Dim folderProperties() As PropDef = mgr.PropertyService.GetPropertyDefinitionsByEntityClassId(entityClassId_folder)


                        Dim i As Integer = 0

                        For i = 0 To folderProperties.Length - 1

                            Select Case folderProperties(i).DispName
                                Case "N° semaine d'expédition"
                                    P_ID(0) = folderProperties(i).Id


                                Case "Nom client"
                                    PCF_Nom(0) = folderProperties(i).Id

                                Case "Gamme"
                                    PCF_Gam(0) = folderProperties(i).Id


                                Case "N° Commande"
                                    PCF_Nco(0) = folderProperties(i).Id


                                Case "Quantité baies"

                                    PCF_Qua(0) = folderProperties(i).Id

                            End Select


                        Next



                        Dim propInstParamArray As PropInstParamArray() = New PropInstParamArray() {New PropInstParamArray() With
                         {.Items = New PropInstParam() {New PropInstParam() With {.PropDefId = P_ID(0), .Val = Nsemaine}}}}

                        Try
                            mgr.DocumentServiceExtensions.UpdateFolderProperties(SelectedFoldersIds, propInstParamArray)
                            My.Computer.FileSystem.WriteAllText(LogFile, "-->La propriété : N° de la semaine a été mise à jour pour le dossier :  " + selectedFolder.Name + vbCrLf, True)

                        Catch ex As Exception
                            MessageBox.Show("Error: " & ex.Message)
                            My.Computer.FileSystem.WriteAllText(LogFile, "-->ERREUR : " + ex.Message.ToString + vbCrLf, True)
                        End Try


                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''Parcourir le dossier
                        Dim childFoldersIds() As Long = mgr.DocumentService.GetFolderIdsByParentIds(SelectedFoldersIds, False)
                        Dim childFolderFiles() As File
                        Dim entityClassId_file As String = Autodesk.DataManagement.Client.Framework.Vault.Currency.Entities.EntityClassIds.Files
                        Dim fileProperties() As PropDef = connection.WebServiceManager.PropertyService.GetPropertyDefinitionsByEntityClassId(entityClassId_file)

                        '''''''''''''''''''''''''''''''''''''Propriété N° semaine


                        For Each childFolderId As Long In childFoldersIds


                            Dim longchildFolderId(0) As Long
                            longchildFolderId(0) = childFolderId
                            Dim childFolder As Autodesk.Connectivity.WebServices.Folder = connection.WebServiceManager.DocumentService.GetFolderById(childFolderId)

                            Dim entityClassId_childFolder As String = Autodesk.DataManagement.Client.Framework.Vault.Currency.Entities.EntityClassIds.Folder
                            Dim childFolderProperties() As PropDef = mgr.PropertyService.GetPropertyDefinitionsByEntityClassId(entityClassId_folder)

                            i = 0
                            While childFolderProperties(i).DispName <> "N° semaine d'expédition"
                                i = i + 1
                            End While
                            PCF_ID(0) = childFolderProperties(i).Id




                            ' My.Computer.FileSystem.WriteAllText(LogFile, "-->La propriété : " + childFolderProperties(i).DispName + " a été trouvée" + vbCrLf, True)

                            Dim childFolderpropInstParamArray As PropInstParamArray() = New PropInstParamArray() {New PropInstParamArray() With
                         {.Items = New PropInstParam() {New PropInstParam() With {.PropDefId = PCF_ID(0), .Val = Nsemaine}}}}

                            Try
                                mgr.DocumentServiceExtensions.UpdateFolderProperties(longchildFolderId, childFolderpropInstParamArray)
                                My.Computer.FileSystem.WriteAllText(LogFile, "-->La propriété : " + childFolderProperties(i).DispName + " a été mise à jour pour le dossier : " + childFolder.Name + vbCrLf, True)

                            Catch ex As Exception
                                MessageBox.Show("Error: " & ex.Message)
                                My.Computer.FileSystem.WriteAllText(LogFile, "-->ERREUR : " + ex.Message.ToString + vbCrLf, True)
                            End Try


                            If childFolder.Name.Contains("_DOC") <> True Then

                                For i = 0 To fileProperties.Length - 1

                                    Select Case fileProperties(i).DispName
                                        Case "Description"
                                            PF_ID(0) = fileProperties(i).Id


                                        Case "Nom client"
                                            PF_Nom(0) = fileProperties(i).Id

                                        Case "Gamme"
                                            PF_Gam(0) = fileProperties(i).Id


                                        Case "N° Commande"
                                            PF_Nco(0) = fileProperties(i).Id


                                        Case "Quantité baies"

                                            PF_Qua(0) = fileProperties(i).Id

                                    End Select


                                Next

                                childFolderFiles = mgr.DocumentService.GetLatestFilesByFolderId(childFolderId, False)
                                Dim nbrFiles As Integer = childFolderFiles.Length

                                My.Computer.FileSystem.WriteAllText(LogFile, "-->Le dossier : " + childFolder.Name.ToString + " contient : " + nbrFiles.ToString + " fichiers" + vbCrLf, True)

                                For Each file As File In childFolderFiles


                                    Dim fileIds(0) As Long
                                    fileIds(0) = file.Id

                                    Dim filePropertyValue_desc As PropInst = connection.WebServiceManager.PropertyService.GetProperties(entityClassId_file, fileIds, PF_ID.ToArray()).First()

                                    If filePropertyValue_desc.Val <> Nothing Then

                                        If filePropertyValue_desc.Val.ToString = "HALL D'ENTREE" And file.Name.Contains(".iam") Then

                                            My.Computer.FileSystem.WriteAllText(LogFile, "-->Le fichier : " + file.Name.ToString + " a bien été trouvé" + vbCrLf, True)

                                            Dim filePropertyValueNotDoc_NomC As PropInst = connection.WebServiceManager.PropertyService.GetProperties(entityClassId_file, fileIds, PF_Nom.ToArray()).First()
                                            Dim filePropertyValueNotDoc_Gamm As PropInst = connection.WebServiceManager.PropertyService.GetProperties(entityClassId_file, fileIds, PF_Gam.ToArray()).First()
                                            Dim filePropertyValueNotDoc_Ncom As PropInst = connection.WebServiceManager.PropertyService.GetProperties(entityClassId_file, fileIds, PF_Nco.ToArray()).First()
                                            Dim filePropertyValueNotDoc_Quan As PropInst = connection.WebServiceManager.PropertyService.GetProperties(entityClassId_file, fileIds, PF_Qua.ToArray()).First()


                                            'Mettre les données dans le dossier principal 

                                            If filePropertyValueNotDoc_NomC.Val <> Nothing Then


                                                propInstParamArray_new = New PropInstParamArray() {New PropInstParamArray() With
                                                            {.Items = New PropInstParam() {New PropInstParam() With {.PropDefId = PCF_Nom(0), .Val = filePropertyValueNotDoc_NomC.Val.ToString}}}}
                                                Try
                                                    mgr.DocumentServiceExtensions.UpdateFolderProperties(SelectedFoldersIds, propInstParamArray_new)

                                                Catch ex As Exception
                                                    MessageBox.Show("Error: " & ex.Message)
                                                    My.Computer.FileSystem.WriteAllText(LogFile, "-->ERREUR : " + ex.Message.ToString + vbCrLf, True)
                                                End Try
                                                My.Computer.FileSystem.WriteAllText(LogFile, "-->Nom clien :" + filePropertyValueNotDoc_NomC.Val.ToString + vbCrLf, True)
                                            End If

                                            If filePropertyValueNotDoc_Gamm.Val <> Nothing Then

                                                If filePropertyValueNotDoc_Gamm.Val.ToString.Contains("CIB'SLIDE") Then
                                                    propInstParamArray_new = New PropInstParamArray() {New PropInstParamArray() With
                                                                    {.Items = New PropInstParam() {New PropInstParam() With {.PropDefId = PCF_Gam(0), .Val = "CIB'SLIDE"}}}}


                                                Else
                                                    propInstParamArray_new = New PropInstParamArray() {New PropInstParamArray() With
                                                                 {.Items = New PropInstParam() {New PropInstParam() With {.PropDefId = PCF_Gam(0), .Val = filePropertyValueNotDoc_Gamm.Val.ToString}}}}

                                                End If

                                                Try
                                                    mgr.DocumentServiceExtensions.UpdateFolderProperties(SelectedFoldersIds, propInstParamArray_new)

                                                Catch ex As Exception
                                                    MessageBox.Show("Error: " & ex.Message)
                                                    My.Computer.FileSystem.WriteAllText(LogFile, "-->ERREUR : " + ex.Message.ToString + vbCrLf, True)
                                                End Try

                                                My.Computer.FileSystem.WriteAllText(LogFile, "-->Gamme :" + filePropertyValueNotDoc_Gamm.Val.ToString + vbCrLf, True)


                                            End If

                                            If filePropertyValueNotDoc_Ncom.Val <> Nothing Then


                                                propInstParamArray_new = New PropInstParamArray() {New PropInstParamArray() With
                                                          {.Items = New PropInstParam() {New PropInstParam() With {.PropDefId = PCF_Nco(0), .Val = filePropertyValueNotDoc_Ncom.Val.ToString}}}}
                                                Try
                                                    mgr.DocumentServiceExtensions.UpdateFolderProperties(SelectedFoldersIds, propInstParamArray_new)

                                                Catch ex As Exception
                                                    MessageBox.Show("Error: " & ex.Message)
                                                    My.Computer.FileSystem.WriteAllText(LogFile, "-->ERREUR : " + ex.Message.ToString + vbCrLf, True)
                                                End Try
                                                My.Computer.FileSystem.WriteAllText(LogFile, "-->N° Commande :" + filePropertyValueNotDoc_Ncom.Val.ToString + vbCrLf, True)
                                            End If

                                            If filePropertyValueNotDoc_Quan.Val <> Nothing Then
                                                My.Computer.FileSystem.WriteAllText(LogFile, "-->Quantité :" + filePropertyValueNotDoc_Quan.Val.ToString + vbCrLf, True)

                                                Quantite = Quantite + filePropertyValueNotDoc_Quan.Val
                                                My.Computer.FileSystem.WriteAllText(LogFile, "-->Quantité T:" + Quantite.ToString + vbCrLf, True)
                                                propInstParamArray_new = New PropInstParamArray() {New PropInstParamArray() With
                                                          {.Items = New PropInstParam() {New PropInstParam() With {.PropDefId = PCF_Qua(0), .Val = Quantite.ToString}}}}

                                                Try
                                                    mgr.DocumentServiceExtensions.UpdateFolderProperties(SelectedFoldersIds, propInstParamArray_new)

                                                Catch ex As Exception

                                                    MessageBox.Show("Error: " & ex.Message)
                                                    My.Computer.FileSystem.WriteAllText(LogFile, "-->ERREUR : " + ex.Message.ToString + vbCrLf, True)
                                                End Try

                                            End If



                                            checkedOutFile = Nothing
                                            loadedFile = Nothing

                                            If file.CheckedOut Then
                                                loadedFile = New FileIteration(connection, checkedOutFile)
                                                connection.FileManager.UndoCheckoutFile(loadedFile)
                                            End If

                                            'Checkout the File 
                                            loadedFile = New FileIteration(connection, file)
                                            Dim localPath As String = connection.WorkingFoldersManager.GetWorkingFolder(childFolder.FullName).FullPath
                                            Dim downloadTicket As ByteArray

                                            checkedOutFile = connection.WebServiceManager.DocumentService.CheckoutFile(loadedFile.EntityIterationId, CheckoutFileOptions.Master, Environment.MachineName, "c:\temp", "Mise à jour du N° semaine dans le fichier", downloadTicket)

                                            'Le fichier a été extrait


                                            Dim downloadTicket1 As ByteArray = connection.WebServiceManager.DocumentService.GetDownloadTicketsByFileIds(ToSingleArray(loadedFile.EntityIterationId)).First()
                                            Dim fileProps() As CtntSrcPropDef = connection.WebServiceManager.FilestoreService.GetContentSourcePropertyDefinitions(
                                                    downloadTicket1.Bytes, True)
                                            Dim selectedProp_numSem As CtntSrcPropDef

                                            Dim propSet() As CtntSrcPropDef = fileProps

                                            If Not propSet.Any() Then
                                                'MessageBox.Show("No editable properties")
                                                loadedFile = New FileIteration(connection, checkedOutFile)
                                                connection.FileManager.UndoCheckoutFile(loadedFile)
                                            End If


                                            i = 0
                                            While propSet(i).DispName <> "N° SEMAINE EXPEDITION"
                                                i = i + 1
                                                'MessageBox.Show(propSet(i).DispName)
                                            End While
                                            selectedProp_numSem = propSet(i)
                                            'MessageBox.Show(selectedProp_numSem.DispName)



                                            Dim list As New List(Of PropWriteReq)()

                                            If (selectedProp_numSem Is Nothing) <> True Then

                                                Dim propWrite_numSem As New PropWriteReq() With {
                                                   .CanCreate = False,
                                                   .Moniker = selectedProp_numSem.Moniker,
                                                   .Val = Nsemaine
                                                 }
                                                list.Add(propWrite_numSem)

                                            End If

                                            Dim results As PropWriteResults
                                            Dim requests() As PropWriteReq = list.ToArray()

                                            Dim uploadTicket As Byte() = connection.WebServiceManager.FilestoreService.CopyFile(
                                                        downloadTicket.Bytes, file.Name.Substring(file.Name.Length - 4), True, requests, results)

                                            Dim new_file As File = connection.WebServiceManager.DocumentService.CheckinUploadedFile(loadedFile.EntityMasterId, "update", False, DateTime.Now, Nothing, Nothing,
                                             True, loadedFile.EntityName, loadedFile.FileClassification, loadedFile.IsHidden, ToByteArray(uploadTicket))
                                        End If
                                    End If
                                Next
                            End If
                        Next

                        My.Computer.FileSystem.WriteAllText(LogFile, "-->Fin traitement !" + vbCrLf, True)
                        'MessageBox.Show([String].Format("Fin traitement ! 1"))



                        MessageBox.Show([String].Format("Fin traitement ! "))


                    End If
                End If
            Catch ex As Exception
                ' If something goes wrong, we don't want the exception to bubble up to Vault Explorer.
                MessageBox.Show("Error: " & ex.Message)
                loadedFile = New FileIteration(connection, checkedOutFile)
                connection.FileManager.UndoCheckoutFile(loadedFile)

            End Try
        End Sub

        Public Function ToSingleArray(Of T)(obj As T) As T()
            Return New T() {obj}

        End Function

        Public Function ToByteArray(inputArray As Byte()) As ByteArray
            Return New ByteArray() With {
                .Bytes = inputArray
            }

        End Function


        ''' <summary>
        ''' This function is called whenever our custom tab is active and the selection has changed in the main grid.
        ''' </summary>
        ''' <param name="sender">The sender object.  Usually not used.</param>
        ''' <param name="e">The event args.  Provides additional information about the environment.</param>
        Private Sub propertyTab_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
            Try
                ' The event args has our custom tab object.  We need to cast it to our type.
                Dim tabControl As MyCustomTabControl = TryCast(e.Context.UserControl, MyCustomTabControl)

                ' Send selection to the tab so that it can display the object.
                tabControl.SetSelectedObject(e.Context.SelectedObject)
            Catch ex As Exception
                ' If something goes wrong, we don't want the exception to bubble up to Vault Explorer.
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Sub

    End Class

End Namespace
