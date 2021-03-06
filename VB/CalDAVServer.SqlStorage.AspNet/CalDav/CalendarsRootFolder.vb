Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports System.Threading.Tasks
Imports ITHit.WebDAV.Server
Imports ITHit.WebDAV.Server.CalDav
Imports ITHit.WebDAV.Server.Class1

Namespace CalDav

    ''' <summary>
    ''' Folder that contains calendars.
    ''' Instances of this class correspond to the following path: [DAVLocation]/calendars/
    ''' </summary>
    Public Class CalendarsRootFolder
        Inherits LogicalFolder
        Implements IFolderAsync

        ''' <summary>
        ''' This folder name.
        ''' </summary>
        Private Shared ReadOnly calendarsRootFolderName As String = "calendars"

        ''' <summary>
        ''' Path to this folder.
        ''' </summary>
        Public Shared CalendarsRootFolderPath As String = DavLocationFolder.DavLocationFolderPath & calendarsRootFolderName & "/"c

        Public Sub New(context As DavContext)
            MyBase.New(context, CalendarsRootFolderPath)
        End Sub

        ''' <summary>
        ''' Retrieves children of this folder.
        ''' </summary>
        ''' <param name="propNames">Properties requested by client application for each child.</param>
        ''' <returns>Children of this folder.</returns>
        Public Overrides Async Function GetChildrenAsync(propNames As IList(Of PropertyName)) As Task(Of IEnumerable(Of IHierarchyItemAsync)) Implements IItemCollectionAsync.GetChildrenAsync
            ' Here we list calendars from back-end storage. 
            ' You can filter calendars if requied and return only calendars that user has access to.
            Return(Await CalendarFolder.LoadAllAsync(Context)).OrderBy(Function(x) x.Name)
        End Function

        Public Function CreateFileAsync(name As String) As Task(Of IFileAsync) Implements IFolderAsync.CreateFileAsync
            Throw New DavException("Not implemented.", DavStatus.NOT_IMPLEMENTED)
        End Function

        ''' <summary>
        ''' Creates a new calendar.
        ''' </summary>
        ''' <param name="name">Name of the new calendar.</param>
        Public Async Function CreateFolderAsync(name As String) As Task Implements IFolderAsync.CreateFolderAsync
            Await CalendarFolder.CreateCalendarFolderAsync(Context, name, "")
        End Function
    End Class
End Namespace
