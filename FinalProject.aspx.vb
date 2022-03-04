Imports System.Data
Imports System.Data.SqlClient
Partial Class MIS_325_Final_Project_FinalProject
    Inherits System.Web.UI.Page

    'connection variable hidden
    
#Region "Other Variables"
    Public Shared helpmechooseActiveViewSelect As Integer 'we need this for our recommendation function 
    Public Shared gintloopcounter As Integer = 0 'needed for our login page, where it locks the user if password entered incorrectly to many times
#End Region

#Region "Component Data Adapters"
    'these are all needed to fill in the dropdownlists for our components
    'Variables for Basic Components
    Public Shared gdaBasicProcessorDDL As New SqlDataAdapter("Select * from POSBasicProcessor", con)
    Public Shared cbBasicProcessor As New SqlCommandBuilder(gdaBasicProcessorDDL)
    Public Shared gdtBasicProcessorDDL As New DataTable

    Public Shared gdaBasicMemoryDDL As New SqlDataAdapter("Select * from POSBasicMemory", con)
    Public Shared cbBasicMemory As New SqlCommandBuilder(gdaBasicMemoryDDL)
    Public Shared gdtBasicMemoryDDL As New DataTable

    Public Shared gdaBasicStorageDDL As New SqlDataAdapter("Select * from POSBasicStorage", con)
    Public Shared cbBasicStorage As New SqlCommandBuilder(gdaBasicStorageDDL)
    Public Shared gdtBasicStorageDDL As New DataTable

    'Variables for Workstation Components
    Public Shared gdaWorkstationProcessorDDL As New SqlDataAdapter("Select * from  POSProcessors ", con)
    Public Shared cbWorkstationProcessor As New SqlCommandBuilder(gdaWorkstationProcessorDDL)
    Public Shared gdtWorkstationProcessorDDL As New DataTable

    Public Shared gdaWorkstationVideoCardDDL As New SqlDataAdapter("Select * from POSVideoCard", con)
    Public Shared cbWorkstationVideoCard As New SqlCommandBuilder(gdaWorkstationVideoCardDDL)
    Public Shared gdtWorkstationVideoCardDDL As New DataTable

    Public Shared gdaWorkstationMemoryDDL As New SqlDataAdapter("Select * from POSMemory", con)
    Public Shared cbWorkstationMemory As New SqlCommandBuilder(gdaWorkstationMemoryDDL)
    Public Shared gdtWorkstationMemoryDDL As New DataTable

    Public Shared gdaWorkstationStorageDDL As New SqlDataAdapter("Select * from POSStorage", con)
    Public Shared cbWorkstationStorage As New SqlCommandBuilder(gdaWorkstationStorageDDL)
    Public Shared gdtWorkstationStorageDDL As New DataTable

    'Variables for Server Components
    Public Shared gdaServProcessorDDL As New SqlDataAdapter("Select * from  POSServerProcessor ", con)
    Public Shared cbServerProcessor As New SqlCommandBuilder(gdaServProcessorDDL)
    Public Shared gdtServerProcessorDDL As New DataTable

    Public Shared gdaServVideoCardDDL As New SqlDataAdapter("Select * from POSServerVideoCard", con)
    Public Shared cbServVideoCard As New SqlCommandBuilder(gdaServVideoCardDDL)
    Public Shared gdtServVideoCardDDL As New DataTable

    Public Shared gdaServMemoryDDL As New SqlDataAdapter("Select * from POSServerMemory", con)
    Public Shared cbServMemory As New SqlCommandBuilder(gdaServMemoryDDL)
    Public Shared gdtServMemoryDDL As New DataTable

    Public Shared gdaServStorageDDL As New SqlDataAdapter("Select * from POSServerStorage", con)
    Public Shared cbServStorage As New SqlCommandBuilder(gdaServStorageDDL)
    Public Shared gdtServStorageDDL As New DataTable
#End Region

#Region "Customer and Price related Variables"
    Public Shared decTotal As Decimal 'Needed for our total price label
    Public Shared gdecBasicTotal As Decimal
    Public Shared gdecWorkstationTotal As Decimal
    Public Shared gdecServerTotal As Decimal

    'these variables are used to determine who's totals are updated.
    Public Shared strBasicUserID As String
    Public Shared strWorkstationUserID As String
    Public Shared strServerUserID As String

    Public Shared gdaCustomerDDL As New SqlDataAdapter("Select * from RegisteredUsers", con)
    Public Shared cbCustomerDDL As New SqlCommandBuilder(gdaCustomerDDL)
    Public Shared gdtCustomerDDL As New DataTable

    Public Shared gdtCurrentBasicCost As New DataTable
    Public Shared gdtCurrentWorkstationCost As New DataTable
    Public Shared gdtCurrentServerCost As New DataTable

    'Customers
    Public Shared gdaGetCustomers As New SqlDataAdapter("SELECT * FROM RegisteredUsers", con)
    Public Shared cbCustomers As New SqlCommandBuilder(gdaGetCustomers)
    Public Shared gdtCustomers As New DataTable
#End Region

#Region "Variables for Updating Transaction & Inventory Tables"
    'Fill Transactions for Basic Computer
    Public Shared gdaBTransactions As New SqlDataAdapter("SELECT * FROM POSBasicTransactions", con)
    Public Shared gdaBasicCustomer As New SqlDataAdapter("SELECT * FROM RegisteredUsers WHERE UserID = @p1", con)
    Public Shared cbBTransactions As New SqlCommandBuilder(gdaBTransactions)
    Public Shared gdtBTransactions As New DataTable

    'Fill Transactions for Workstation 
    Public Shared gdaWSTransactions As New SqlDataAdapter("SELECT * FROM POSWorkstationTransactions", con)
    Public Shared gdaWorkstationCustomer As New SqlDataAdapter("SELECT * FROM RegisteredUsers WHERE UserID = @p1", con)
    Public Shared cbWSTransactions As New SqlCommandBuilder(gdaWSTransactions)
    Public Shared gdtWSTransactions As New DataTable
    Public Shared gdtDisplayWorkstationTransactions As New DataTable 'will display the user and count of workstations sold

    'Fill Transactions for Server
    Public Shared gdaSTransactions As New SqlDataAdapter("SELECT * FROM POSServerTransactions", con)
    Public Shared gdaServerCustomer As New SqlDataAdapter("SELECT * FROM RegisteredUsers WHERE UserID = @p1", con)
    Public Shared cbSTransactions As New SqlCommandBuilder(gdaSTransactions)
    Public Shared gdtSTransactions As New DataTable

    'Inventory Updates
    Public Shared gdaInventory As New SqlDataAdapter("Select * From POSInventory", con)
    Public Shared CBInventory As New SqlCommandBuilder(gdaInventory)
    Public Shared gdtInventory As New DataTable
#End Region

#Region "Initialization"
    Private Sub MIS_325_Final_Project_FinalProject_Init(sender As Object, e As EventArgs) Handles Me.Init
        panelAdmin.Visible = False 'make the admin login invisible until a user logs in.
        panelLaunch.Visible = True

        mvLaunch.ActiveViewIndex = 0

        decTotal = 0

        'Fillschema for our basic part dropdownlists
        gdaBasicProcessorDDL.FillSchema(gdtBasicProcessorDDL, SchemaType.Mapped)
        gdaBasicMemoryDDL.FillSchema(gdtBasicMemoryDDL, SchemaType.Mapped)
        gdaBasicStorageDDL.FillSchema(gdtBasicStorageDDL, SchemaType.Mapped)

        'Fillschema for our workstation part dropdownlists
        gdaWorkstationProcessorDDL.FillSchema(gdtWorkstationProcessorDDL, SchemaType.Mapped)
        gdaWorkstationVideoCardDDL.FillSchema(gdtWorkstationVideoCardDDL, SchemaType.Mapped)
        gdaWorkstationMemoryDDL.FillSchema(gdtWorkstationMemoryDDL, SchemaType.Mapped)
        gdaWorkstationStorageDDL.FillSchema(gdtWorkstationStorageDDL, SchemaType.Mapped)

        'Fillschema for our server part dropdownlists
        gdaServProcessorDDL.FillSchema(gdtServerProcessorDDL, SchemaType.Mapped)
        gdaServVideoCardDDL.FillSchema(gdtServVideoCardDDL, SchemaType.Mapped)
        gdaServMemoryDDL.FillSchema(gdtServMemoryDDL, SchemaType.Mapped)
        gdaServStorageDDL.FillSchema(gdtServStorageDDL, SchemaType.Mapped)

        'Fillschema for our basic transaction table
        gdaBTransactions.FillSchema(gdtBTransactions, SchemaType.Mapped)

        'Fillschema for our workstation transaction table
        gdaWSTransactions.FillSchema(gdtWSTransactions, SchemaType.Mapped)
        gdaWSTransactions.FillSchema(gdtDisplayWorkstationTransactions, SchemaType.Mapped)

        'Fillschema for our server transaction table
        gdaSTransactions.FillSchema(gdtSTransactions, SchemaType.Mapped)

        gdaInventory.FillSchema(gdtInventory, SchemaType.Mapped)
        gdaGetCustomers.FillSchema(gdtCustomers, SchemaType.Mapped)
        Call UpdateDDL()
    End Sub
#End Region

#Region "Called Procedures"
    Private Sub UpdateDDL()
        'Here are the sqlcommands that are used for filling in our dropdownlists for the pc parts.
        Dim BasicProcessorDDL As New SqlCommand("Select * from POSBasicProcessor Where BProcessorID = @p9 ", con)
        Dim BasicMemoryDDL As New SqlCommand("Select * from POSBasicMemory where BMemoryID = @p10", con)
        Dim BasicStorageDDL As New SqlCommand("Select * from POSBasicStorage where BStorageID = @p11", con)

        Dim WorkstationProcessorDDL As New SqlCommand("Select * from POSProcessors Where ProcessorID = @p1 ", con)
        Dim WorkstationVideoCardDDL As New SqlCommand("Select * from POSVideoCard where VideoCardID = @p2", con)
        Dim WorkstationMemoryDDL As New SqlCommand("Select * from POSMemory where MemoryID = @p3", con)
        Dim WorkstationStorageDDL As New SqlCommand("Select * from POSStorage where StorageID = @p4", con)

        Dim ServerProcessorDDL As New SqlCommand("Select * from POSServerProcessor Where SProcessorID = @p5 ", con)
        Dim ServerVideoCardDDL As New SqlCommand("Select * from POSServerVideoCard where SVideoCardID = @p6", con)
        Dim ServerMemoryDDL As New SqlCommand("Select * from POServerSMemory where SMemoryID = @p7", con)
        Dim ServerStorageDDL As New SqlCommand("Select * from POSServerStorage where SStorageID = @p8", con)

        'this will fill in all of the ddls with the customers
        Dim CustomerDDL As New SqlCommand("Select * from RegisteredUsers Where UserID = @pcustomer", con) 'used for the customer ddl 

        'these will be used for our separate inventories
        Dim daProcessorInventory As New SqlCommand("Select ItemName, StockRemaining FROM POSInventory where BProcessorID_Fk = @p12", con)
        Dim daMemoryInventory As New SqlCommand("Select ItemName, StockRemaining FROM POSInventory where BMemoryID_Fk = @p13", con)
        Dim daStorageInventory As New SqlCommand("Select ItemName, StockRemaining FROM POSInventory where StorageID_Fk = @p14", con)

        Dim daVideoCardInventory As New SqlCommand("Select ItemName, StockRemaining FROM POSInventory where BVideoCardID_Fk = @p15", con)

        Dim daServerProcessorInventory As New SqlCommand("Select ItemName, StockRemaining FROM POSInventory where SProcessorID_Fk = @p16", con)
        Dim daServerVideoCardInventory As New SqlCommand("Select ItemName, StockRemaining FROM POSInventory where SSVideoCard_Fk = @p17", con)
        Dim daServerMemoryInventory As New SqlCommand("Select ItemName, StockRemaining FROM POSInventory where SMemoryID_Fk = @p18", con)
        Dim daServerStorageInventory As New SqlCommand("Select ItemName, StockRemaining FROM POSInventory where SStorageID_Fk = @p19", con)

        If gdtCustomerDDL.Rows.Count > 0 Then gdtCustomerDDL.Rows.Clear()

        If gdtBasicProcessorDDL.Rows.Count > 0 Then gdtBasicProcessorDDL.Rows.Clear()
        If gdtBasicMemoryDDL.Rows.Count > 0 Then gdtBasicMemoryDDL.Rows.Clear()
        If gdtBasicStorageDDL.Rows.Count > 0 Then gdtBasicStorageDDL.Rows.Clear()

        If gdtWorkstationProcessorDDL.Rows.Count > 0 Then gdtWorkstationProcessorDDL.Rows.Clear()
        If gdtWorkstationVideoCardDDL.Rows.Count > 0 Then gdtWorkstationVideoCardDDL.Rows.Clear()
        If gdtWorkstationMemoryDDL.Rows.Count > 0 Then gdtWorkstationMemoryDDL.Rows.Clear()
        If gdtWorkstationStorageDDL.Rows.Count > 0 Then gdtWorkstationStorageDDL.Rows.Clear()

        If gdtServerProcessorDDL.Rows.Count > 0 Then gdtServerProcessorDDL.Rows.Clear()
        If gdtServVideoCardDDL.Rows.Count > 0 Then gdtServVideoCardDDL.Rows.Clear()
        If gdtServMemoryDDL.Rows.Count > 0 Then gdtServMemoryDDL.Rows.Clear()
        If gdtServStorageDDL.Rows.Count > 0 Then gdtServStorageDDL.Rows.Clear()

        If gdtInventory.Rows.Count > 0 Then gdtInventory.Rows.Clear()

        '-----------------------------------------------------------------Customer-----------------------------------------------------------------
        With CustomerDDL.Parameters
            .Clear()
            .AddWithValue("@pcustomer", ddlServerCustomer.SelectedValue)
        End With
        gdaCustomerDDL.Fill(gdtCustomerDDL)

        With ddlBasicCustomer
            .DataSource = gdtCustomerDDL
            .DataTextField = "UserID"
            .DataValueField = "UserID"
            .DataBind()
            .Items.Insert(0, "Select a Customer")
        End With

        With ddlWorkstationCustomer
            .DataSource = gdtCustomerDDL
            .DataTextField = "UserID"
            .DataValueField = "UserID"
            .DataBind()
            .Items.Insert(0, "Select a Customer")
        End With

        With ddlServerCustomer
            .DataSource = gdtCustomerDDL
            .DataTextField = "UserID"
            .DataValueField = "UserID"
            .DataBind()
            .Items.Insert(0, "Select a Customer")
        End With

        '-----------------------------------------------------------------Basic-----------------------------------------------------------------
        'BASIC PROCESSOR
        With BasicProcessorDDL.Parameters
            .Clear()
            .AddWithValue("@p1", ddlBasicProcessor.SelectedValue)
        End With
        gdaBasicProcessorDDL.Fill(gdtBasicProcessorDDL)

        With ddlBasicProcessor
            .DataSource = gdtBasicProcessorDDL
            .DataTextField = "BProcessorName"
            .DataValueField = ("BProcessorID")
            .DataBind()
            .Items.Insert(0, "Select a Processor")
        End With

        'BASIC MEMORY
        With BasicMemoryDDL.Parameters
            .Clear()
            .AddWithValue("@p3", ddlBasicMemory.SelectedValue)
        End With
        gdaBasicMemoryDDL.Fill(gdtBasicMemoryDDL)

        With ddlBasicMemory
            .DataSource = gdtBasicMemoryDDL
            .DataTextField = "BMemoryName"
            .DataValueField = ("BMemoryID")
            .DataBind()
            .Items.Insert(0, "Select a Memory Size")
        End With

        'BASIC STORAGE
        With BasicStorageDDL.Parameters
            .Clear()
            .AddWithValue("@p4", ddlBasicStorage.SelectedValue)
        End With
        gdaBasicStorageDDL.Fill(gdtBasicStorageDDL)

        With ddlBasicStorage
            .DataSource = gdtBasicStorageDDL
            .DataTextField = "BStorageName"
            .DataValueField = ("BStorageID")
            .DataBind()
            .Items.Insert(0, "Select a Storage Size")
        End With

        '-----------------------------------------------------------------Workstation-----------------------------------------------------------------
        ' WORK STATION processor
        With WorkstationProcessorDDL.Parameters
            .Clear()
            .AddWithValue("@p1", ddlWorkstationProcessor.SelectedValue)
        End With
        gdaWorkstationProcessorDDL.Fill(gdtWorkstationProcessorDDL)

        With ddlWorkstationProcessor
            .DataSource = gdtWorkstationProcessorDDL
            .DataTextField = "ProcessorName"
            .DataValueField = ("ProcessorID")
            .DataBind()
            .Items.Insert(0, "Select a Processor")
        End With

        ' workstation VIDEO CARD   
        With WorkstationVideoCardDDL.Parameters
            .Clear()
            .AddWithValue("@p2", ddlWorkstationVideoCard.SelectedValue)
        End With
        gdaWorkstationVideoCardDDL.Fill(gdtWorkstationVideoCardDDL)

        With ddlWorkstationVideoCard
            .DataSource = gdtWorkstationVideoCardDDL
            .DataTextField = "VideoCardName"
            .DataValueField = ("VideoCardID")
            .DataBind()
            .Items.Insert(0, "Select a Video Card")
        End With

        'workstation MEMORY
        With WorkstationMemoryDDL.Parameters
            .Clear()
            .AddWithValue("@p3", ddlWorkstationMemory.SelectedValue)
        End With
        gdaWorkstationMemoryDDL.Fill(gdtWorkstationMemoryDDL)

        With ddlWorkstationMemory
            .DataSource = gdtWorkstationMemoryDDL
            .DataTextField = "MemoryName"
            .DataValueField = ("MemoryID")
            .DataBind()
            .Items.Insert(0, "Select a Memory Size")
        End With

        'workstation STORAGE
        With WorkstationStorageDDL.Parameters
            .Clear()
            .AddWithValue("@p4", ddlWorkstationStorage.SelectedValue)
        End With
        gdaWorkstationStorageDDL.Fill(gdtWorkstationStorageDDL)

        With ddlWorkstationStorage
            .DataSource = gdtWorkstationStorageDDL
            .DataTextField = "StorageName"
            .DataValueField = ("StorageID")
            .DataBind()
            .Items.Insert(0, "Select a Storage Size")
        End With

        '-----------------------------------------------------------------Server-----------------------------------------------------------------
        ' Server processor
        With ServerProcessorDDL.Parameters
            .Clear()
            .AddWithValue("@p5", ddlServerProcessor.SelectedValue)
        End With
        gdaServProcessorDDL.Fill(gdtServerProcessorDDL)

        With ddlServerProcessor
            .DataSource = gdtServerProcessorDDL
            .DataTextField = "SProcessorName"
            .DataValueField = ("SProcessorID")
            .DataBind()
            .Items.Insert(0, "Select a Processor")
        End With

        ' Server VIDEO CARD   
        With ServerVideoCardDDL.Parameters
            .Clear()
            .AddWithValue("@p6", ddlServerVideoCard.SelectedValue)
        End With
        gdaServVideoCardDDL.Fill(gdtServVideoCardDDL)

        With ddlServerVideoCard
            .DataSource = gdtServVideoCardDDL
            .DataTextField = "SVideoCardName"
            .DataValueField = ("SVideoCardID")
            .DataBind()
            .Items.Insert(0, "Select a Video Card")
        End With

        'Server MEMORY
        With ServerMemoryDDL.Parameters
            .Clear()
            .AddWithValue("@p7", ddlServerMemory.SelectedValue)
        End With
        gdaServMemoryDDL.Fill(gdtServMemoryDDL)

        With ddlServerMemory
            .DataSource = gdtServMemoryDDL
            .DataTextField = "SMemoryName"
            .DataValueField = ("SMemoryID")
            .DataBind()
            .Items.Insert(0, "Select a Memory Size")
        End With

        'Server STORAGE
        With ServerStorageDDL.Parameters
            .Clear()
            .AddWithValue("@p8", ddlServerStorage.SelectedValue)
        End With
        gdaServStorageDDL.Fill(gdtServStorageDDL)

        With ddlServerStorage
            .DataSource = gdtServStorageDDL
            .DataTextField = "SStorageName"
            .DataValueField = ("SStorageID")
            .DataBind()
            .Items.Insert(0, "Select a Storage Size")
        End With

        Try
            If con.State = ConnectionState.Closed Then con.Open()

            '-----------------------------------------------------------------Filling dropdownlists-----------------------------------------------------------------
            gdaBasicProcessorDDL.Fill(gdtBasicProcessorDDL)
            gdaBasicMemoryDDL.Fill(gdtBasicMemoryDDL)
            gdaBasicStorageDDL.Fill(gdtBasicStorageDDL)

            gdaWorkstationProcessorDDL.Fill(gdtWorkstationProcessorDDL)
            gdaWorkstationVideoCardDDL.Fill(gdtWorkstationVideoCardDDL)
            gdaWorkstationMemoryDDL.Fill(gdtWorkstationMemoryDDL)
            gdaWorkstationStorageDDL.Fill(gdtWorkstationStorageDDL)

            gdaServProcessorDDL.Fill(gdtServerProcessorDDL)
            gdaServVideoCardDDL.Fill(gdtServVideoCardDDL)
            gdaServMemoryDDL.Fill(gdtServMemoryDDL)
            gdaServStorageDDL.Fill(gdtServStorageDDL)

            '------------------------------------Checking Rows---------------------------------------------
            If gdtBasicProcessorDDL.Rows.Count > 0 Then gdtBasicProcessorDDL.Rows.Clear()
            gdaWorkstationProcessorDDL.Fill(gdtBasicProcessorDDL)
            If gdtBasicMemoryDDL.Rows.Count > 0 Then gdtBasicMemoryDDL.Rows.Clear()
            gdaBasicMemoryDDL.Fill(gdtBasicMemoryDDL)
            If gdtBasicStorageDDL.Rows.Count > 0 Then gdtBasicStorageDDL.Rows.Clear()
            gdaBasicStorageDDL.Fill(gdtBasicStorageDDL)

            If gdtWorkstationProcessorDDL.Rows.Count > 0 Then gdtWorkstationProcessorDDL.Rows.Clear()
            gdaWorkstationProcessorDDL.Fill(gdtWorkstationProcessorDDL)
            If gdtWorkstationVideoCardDDL.Rows.Count > 0 Then gdtWorkstationVideoCardDDL.Rows.Clear()
            gdaWorkstationVideoCardDDL.Fill(gdtWorkstationVideoCardDDL)
            If gdtWorkstationMemoryDDL.Rows.Count > 0 Then gdtWorkstationMemoryDDL.Rows.Clear()
            gdaWorkstationMemoryDDL.Fill(gdtWorkstationMemoryDDL)
            If gdtWorkstationStorageDDL.Rows.Count > 0 Then gdtWorkstationStorageDDL.Rows.Clear()
            gdaWorkstationStorageDDL.Fill(gdtWorkstationStorageDDL)


            If gdtServerProcessorDDL.Rows.Count > 0 Then gdtServerProcessorDDL.Rows.Clear()
            gdaServProcessorDDL.Fill(gdtServerProcessorDDL)
            If gdtServVideoCardDDL.Rows.Count > 0 Then gdtServVideoCardDDL.Rows.Clear()
            gdaServVideoCardDDL.Fill(gdtServVideoCardDDL)
            If gdtServMemoryDDL.Rows.Count > 0 Then gdtServMemoryDDL.Rows.Clear()
            gdaServMemoryDDL.Fill(gdtServMemoryDDL)
            If gdtServStorageDDL.Rows.Count > 0 Then gdtServStorageDDL.Rows.Clear()
            gdaServStorageDDL.Fill(gdtServStorageDDL)

        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
            con.Close()
        End Try
    End Sub

    Private Sub getBasicCost()
        'this function is used for getting the costs of the products, and puts them into the label.
        Dim daBasicProcessorCost As New SqlDataAdapter("Select BProcessorCost from POSBasicProcessor where BProcessorID = @p1", con)
        Dim daBasicMemoryCost As New SqlDataAdapter("Select BMemoryCost from POSBasicMemory where BMemoryID = @p2", con)
        Dim daBasicStorageCost As New SqlDataAdapter("Select BStorageCost from POSBasicStorage where BStorageID = @p3", con)

        If ddlBasicProcessor.SelectedIndex <= 0 OrElse ddlBasicMemory.SelectedIndex <= 0 OrElse ddlBasicStorage.SelectedIndex <= 0 Then Exit Sub

        With daBasicProcessorCost.SelectCommand.Parameters
            .Clear()
            .AddWithValue("@p1", ddlBasicProcessor.SelectedValue)
        End With

        With daBasicMemoryCost.SelectCommand.Parameters
            .Clear()
            .AddWithValue("@p2", ddlBasicMemory.SelectedValue)
        End With

        With daBasicStorageCost.SelectCommand.Parameters
            .Clear()
            .AddWithValue("@p3", ddlBasicStorage.SelectedValue)
        End With

        Try
            If gdtCurrentBasicCost.Rows.Count > 0 Then gdtCurrentBasicCost.Rows.Clear()

            daBasicProcessorCost.Fill(gdtCurrentBasicCost)
            daBasicMemoryCost.Fill(gdtCurrentBasicCost)
            daBasicStorageCost.Fill(gdtCurrentBasicCost)

            With gdtCurrentBasicCost.Rows(0)
                labelBasicPrice.Text = .Item("BProcessorCost")
            End With

            With gdtCurrentBasicCost.Rows(1)
                labelBasicPrice.Text += .Item("BMemoryCost")
            End With

            With gdtCurrentBasicCost.Rows(2)
                labelBasicPrice.Text += .Item("BStorageCost")
            End With

            gdecBasicTotal = gdtCurrentBasicCost.Compute("SUM(BProcessorCost)", Nothing) + gdtCurrentBasicCost.Compute("SUM(BMemoryCost)", Nothing) + gdtCurrentBasicCost.Compute("SUM(BStorageCost)", Nothing)

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Private Sub getWorkstationCost()
        'this function is used for getting the costs of the products, and puts them into the label.
        Dim daWorkstationProcessorCost As New SqlDataAdapter("Select ProcessorCost from POSProcessors where ProcessorID = @p1", con)
        Dim daWorkstationMemoryCost As New SqlDataAdapter("Select MemoryCost from POSMemory where MemoryID = @p2", con)
        Dim daWorkstationVideoCardCost As New SqlDataAdapter("Select VideoCardCost from POSVideoCard where VideoCardID = @p3", con)
        Dim daWorkstationStorageCost As New SqlDataAdapter("Select StorageCost from POSStorage where StorageID = @p4", con)

        If ddlWorkstationProcessor.SelectedIndex <= 0 OrElse ddlWorkstationMemory.SelectedIndex <= 0 OrElse ddlWorkstationVideoCard.SelectedIndex <= 0 OrElse ddlWorkstationStorage.SelectedIndex <= 0 Then Exit Sub

        With daWorkstationProcessorCost.SelectCommand.Parameters
            .Clear()
            .AddWithValue("@p1", ddlWorkstationProcessor.SelectedValue)
        End With

        With daWorkstationMemoryCost.SelectCommand.Parameters
            .Clear()
            .AddWithValue("@p2", ddlWorkstationMemory.SelectedValue)
        End With

        With daWorkstationVideoCardCost.SelectCommand.Parameters
            .Clear()
            .AddWithValue("@p3", ddlWorkstationVideoCard.SelectedValue)
        End With

        With daWorkstationStorageCost.SelectCommand.Parameters
            .Clear()
            .AddWithValue("@p4", ddlWorkstationStorage.SelectedValue)
        End With

        Try
            If gdtCurrentWorkstationCost.Rows.Count > 0 Then gdtCurrentWorkstationCost.Rows.Clear()

            daWorkstationProcessorCost.Fill(gdtCurrentWorkstationCost)
            daWorkstationMemoryCost.Fill(gdtCurrentWorkstationCost)
            daWorkstationVideoCardCost.Fill(gdtCurrentWorkstationCost)
            daWorkstationStorageCost.Fill(gdtCurrentWorkstationCost)

            With gdtCurrentWorkstationCost.Rows(0)
                labelWorkstationPrice.Text = .Item("ProcessorCost")
            End With

            With gdtCurrentWorkstationCost.Rows(1)
                labelWorkstationPrice.Text += .Item("MemoryCost")
            End With

            With gdtCurrentWorkstationCost.Rows(2)
                labelWorkstationPrice.Text += .Item("VideoCardCost")
            End With

            With gdtCurrentWorkstationCost.Rows(3)
                labelWorkstationPrice.Text += .Item("StorageCost")
            End With

            gdecWorkstationTotal = gdtCurrentWorkstationCost.Compute("SUM(ProcessorCost)", Nothing) + gdtCurrentWorkstationCost.Compute("SUM(MemoryCost)", Nothing) + gdtCurrentWorkstationCost.Compute("SUM(VideoCardCost)", Nothing) + gdtCurrentWorkstationCost.Compute("SUM(StorageCost)", Nothing)

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Private Sub getServerCost()
        'this function is used for getting the costs of the products, and puts them into the label.
        Dim daServerProcessorCost As New SqlDataAdapter("Select SProcessorCost from POSServerProcessor where SProcessorID = @p1", con)
        Dim daServerMemoryCost As New SqlDataAdapter("Select SMemoryCost from POSServerMemory where SMemoryID = @p2", con)
        Dim daServerVideoCardCost As New SqlDataAdapter("Select SVideoCardCost from POSServerVideoCard where SVideoCardID = @p3", con)
        Dim daServerStorageCost As New SqlDataAdapter("Select SStorageCost from POSServerStorage where SStorageID = @p4", con)

        If ddlServerProcessor.SelectedIndex <= 0 OrElse ddlServerMemory.SelectedIndex <= 0 OrElse ddlServerVideoCard.SelectedIndex <= 0 OrElse ddlServerStorage.SelectedIndex <= 0 Then Exit Sub

        With daServerProcessorCost.SelectCommand.Parameters
            .Clear()
            .AddWithValue("@p1", ddlServerProcessor.SelectedValue)
        End With

        With daServerMemoryCost.SelectCommand.Parameters
            .Clear()
            .AddWithValue("@p2", ddlServerMemory.SelectedValue)
        End With

        With daServerVideoCardCost.SelectCommand.Parameters
            .Clear()
            .AddWithValue("@p3", ddlServerVideoCard.SelectedValue)
        End With

        With daServerStorageCost.SelectCommand.Parameters
            .Clear()
            .AddWithValue("@p4", ddlServerStorage.SelectedValue)
        End With

        Try
            If gdtCurrentServerCost.Rows.Count > 0 Then gdtCurrentServerCost.Rows.Clear()

            daServerProcessorCost.Fill(gdtCurrentServerCost)
            daServerMemoryCost.Fill(gdtCurrentServerCost)
            daServerVideoCardCost.Fill(gdtCurrentServerCost)
            daServerStorageCost.Fill(gdtCurrentServerCost)

            With gdtCurrentServerCost.Rows(0)
                labelServerPrice.Text = .Item("SProcessorCost")
            End With

            With gdtCurrentServerCost.Rows(1)
                labelServerPrice.Text += .Item("SMemoryCost")
            End With

            With gdtCurrentServerCost.Rows(2)
                labelServerPrice.Text += .Item("SVideoCardCost")
            End With

            With gdtCurrentServerCost.Rows(3)
                labelServerPrice.Text += .Item("SStorageCost")
            End With

            gdecServerTotal = gdtCurrentServerCost.Compute("SUM(SProcessorCost)", Nothing) + gdtCurrentServerCost.Compute("SUM(SMemoryCost)", Nothing) + gdtCurrentServerCost.Compute("SUM(SVideoCardCost)", Nothing) + gdtCurrentServerCost.Compute("SUM(SStorageCost)", Nothing)

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Private Sub UpdateCustomerBasicPurchase()
        Dim daGetCustomerInfo As New SqlDataAdapter("SELECT * FROM RegisteredUsers", con)
        Dim cbGetCustomerInfo As New SqlCommandBuilder(daGetCustomerInfo)
        Dim cmdUpdateCustomerInfo As New SqlCommand("UPDATE RegisteredUsers SET LastPurchase = @p1, NumPurchases += 1, TotalSales += @p2 WHERE UserID = @p3", con)
        Dim dtCustomerBasic As New DataTable

        strBasicUserID = ddlBasicCustomer.SelectedItem.Text

        With cmdUpdateCustomerInfo.Parameters
            .Clear()
            .AddWithValue("@p1", DateTime.Today)
            .AddWithValue("@p2", CDec(gdecBasicTotal))
            .AddWithValue("@p3", strBasicUserID)
        End With

        Try
            If con.State = ConnectionState.Closed Then con.Open()
            If dtCustomerBasic.Rows.Count > 0 Then dtCustomerBasic.Rows.Clear()

            cmdUpdateCustomerInfo.ExecuteNonQuery()

            daGetCustomerInfo.Update(dtCustomerBasic)
            daGetCustomerInfo.Fill(dtCustomerBasic)
        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
            con.Close()
        End Try

        gvCustomers.DataSource = dtCustomerBasic
        gvCustomers.DataBind()
    End Sub

    Private Sub UpdateCustomerWorkstationPurchase()
        Dim daGetCustomerInfo As New SqlDataAdapter("SELECT * FROM RegisteredUsers", con)
        Dim cmdUpdateCustomerInfo As New SqlCommand("UPDATE RegisteredUsers SET LastPurchase = @p1, NumPurchases += 1, TotalSales += @p2 WHERE UserID = @p3", con)
        Dim dtCustomerWorkstation As New DataTable

        strWorkstationUserID = ddlWorkstationCustomer.SelectedItem.Text

        With cmdUpdateCustomerInfo.Parameters
            .Clear()
            .AddWithValue("@p1", DateTime.Today)
            .AddWithValue("@p2", CDec(gdecWorkstationTotal))
            .AddWithValue("@p3", strWorkstationUserID)
        End With

        Try
            If con.State = ConnectionState.Closed Then con.Open()
            If dtCustomerWorkstation.Rows.Count > 0 Then dtCustomerWorkstation.Rows.Clear()

            cmdUpdateCustomerInfo.ExecuteNonQuery()

            daGetCustomerInfo.Update(dtCustomerWorkstation)
            daGetCustomerInfo.Fill(dtCustomerWorkstation)
        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
            con.Close()
        End Try

        gvCustomers.DataSource = dtCustomerWorkstation
        gvCustomers.DataBind()
    End Sub

    Private Sub UpdateCustomerServerPurchase()
        Dim daGetCustomerInfo As New SqlDataAdapter("SELECT * FROM RegisteredUsers", con)
        Dim cmdUpdateCustomerInfo As New SqlCommand("UPDATE RegisteredUsers SET LastPurchase = @p1, NumPurchases += 1, TotalSales += @p2 WHERE UserID = @p3", con)
        Dim dtCustomerServer As New DataTable

        strServerUserID = ddlServerCustomer.SelectedItem.Text

        With cmdUpdateCustomerInfo.Parameters
            .Clear()
            .AddWithValue("@p1", DateTime.Today)
            .AddWithValue("@p2", CDec(gdecServerTotal))
            .AddWithValue("@p3", strServerUserID)
        End With

        Try
            If con.State = ConnectionState.Closed Then con.Open()
            If dtCustomerServer.Rows.Count > 0 Then dtCustomerServer.Rows.Clear()

            cmdUpdateCustomerInfo.ExecuteNonQuery()

            daGetCustomerInfo.Update(dtCustomerServer)
            daGetCustomerInfo.Fill(dtCustomerServer)
        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
            con.Close()
        End Try

        gvCustomers.DataSource = dtCustomerServer
        gvCustomers.DataBind()
    End Sub

    Private Sub getAllCustomers()
        Dim daGetCustomers As New SqlDataAdapter("SELECT * FROM RegisteredUsers", con)
        Dim dtCustomers As New DataTable

        daGetCustomers.FillSchema(dtCustomers, SchemaType.Mapped)

        Try
            If dtCustomers.Rows.Count > 0 Then dtCustomers.Rows.Clear()

            daGetCustomers.Fill(dtCustomers)

            Call UpdateDDL()
            Call getBasicCost()
            Call getWorkstationCost()
            Call getServerCost()

            gvCustomers.DataSource = dtCustomers
            gvCustomers.DataBind()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
#End Region

#Region "Add New Customer"
    Protected Sub btnCreateNewCustomer_Click(sender As Object, e As EventArgs) Handles btnCreateNewCustomer.Click
        Dim daCheckCustomers As New SqlDataAdapter("SELECT * FROM RegisteredUsers WHERE UserID = @p1", con)
        Dim CheckCustomersTable As New DataTable

        With daCheckCustomers.SelectCommand.Parameters
            .Clear()
            .AddWithValue("@p1", txtNewUserID.Text)
        End With

        If txtNewUserID.Text = Nothing OrElse txtNewUserPassword.Text = Nothing OrElse txtNewUserRegistrantName.Text = Nothing OrElse txtNewUserAddress.Text = Nothing OrElse txtNewUserCity.Text = Nothing OrElse txtNewUserState.Text = Nothing OrElse txtNewUserZIP.Text = Nothing Then
            Response.Write("Please Check for Valid Data Entry")
            Exit Sub
        Else
            Response.Clear()
        End If

        Try
            daCheckCustomers.Fill(CheckCustomersTable)

            If CheckCustomersTable.Rows.Count >= 1 Then
                txtPopup.Visible = True
                txtPopup.Text = "USER ID ALREADY IN USE"
                Exit Sub
            End If

            'If gdtCustomers.Rows.Count > 0 Then gdtCustomers.Rows.Clear()
            Dim dr As DataRow = gdtCustomers.NewRow

            dr("UserID") = txtNewUserID.Text
            dr("Password") = txtNewUserPassword.Text
            dr("RegistrantName") = txtNewUserRegistrantName.Text
            dr("Address") = txtNewUserAddress.Text
            dr("City") = txtNewUserCity.Text
            dr("State") = txtNewUserState.Text
            dr("ZIP") = txtNewUserZIP.Text
            dr("LastPurchase") = DateTime.Parse(Today)
            dr("NumPurchases") = 0
            dr("TotalSales") = 0

            gdtCustomers.Rows.Add(dr)
            gdaGetCustomers.Update(gdtCustomers)
            gdaGetCustomers.Fill(gdtCustomers)

            'Update the DDLS
            Call getAllCustomers()
            Call UpdateDDL()

            'refresh gridviews
            gvCustomers.DataSource = gdtCustomers
            gvCustomers.DataBind()

            gdtCustomers.Rows.Clear()
            txtPopup.Visible = False
            txtPopup.Text = Nothing
        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
            con.Close()
        End Try
    End Sub
#End Region

#Region "Basic Computing Purchase"
    Protected Sub btnSubmitPurchaseBasic_Click(sender As Object, e As EventArgs) Handles btnSubmitPurchaseBasic.Click
        Dim cmdUpdateBasicTransactions As New SqlCommand("UPDATE POSBasicTransactions SET UserID = @BUser, BProcessorID = @p1, BMemoryID = @p2, BStorageID = @p3 WHERE BasicID = @p4", con)
        Dim daUpdateTransaction As New SqlDataAdapter("SELECT * FROM POSBasicTransactions", con)
        Dim datarowbasictransaction As Integer 'this is needed for our BasicID, so that every transaction is unique. Had a problem where every single value in a table was changed to the most recent

        'needed three separate commands to be able to edit the inventory number
        Dim cmdUpdateProcessorInventory As New SqlCommand("UPDATE POSInventory SET StockRemaining -=1 WHERE ItemName = @p1", con)
        Dim cmdUpdateMemoryInventory As New SqlCommand("UPDATE POSInventory SET StockRemaining -=1 WHERE ItemName = @p1", con)
        Dim cmdUpdateStorageInventory As New SqlCommand("UPDATE POSInventory SET StockRemaining -=1 WHERE ItemName = @p1", con)

        'INVENTORY
        With cmdUpdateProcessorInventory.Parameters
            .Clear()
            .AddWithValue("@p1", ddlBasicProcessor.SelectedItem.Text)
        End With

        With cmdUpdateMemoryInventory.Parameters
            .Clear()
            .AddWithValue("@p1", ddlBasicMemory.SelectedItem.Text)
        End With

        With cmdUpdateStorageInventory.Parameters
            .Clear()
            .AddWithValue("@p1", ddlBasicStorage.SelectedItem.Text)
        End With

        'Processors
        With gdaBasicProcessorDDL.SelectCommand.Parameters
            .Clear()
            .AddWithValue("@p9", (ddlBasicProcessor.SelectedValue))
        End With

        'Memory
        With gdaBasicMemoryDDL.SelectCommand.Parameters
            .Clear()
            .AddWithValue("@p10", (ddlBasicMemory.SelectedValue))
        End With

        'Storage
        With gdaBasicStorageDDL.SelectCommand.Parameters
            .Clear()
            .AddWithValue("@p11", (ddlBasicMemory.SelectedValue))
        End With

        'User DDL ID - retieves the user id
        With gdaBasicCustomer.SelectCommand.Parameters
            .Clear()
            .AddWithValue("@p1", ddlBasicCustomer.SelectedValue)
        End With

        With cmdUpdateBasicTransactions.Parameters
            .Clear()
            .AddWithValue("@BUser", ddlBasicCustomer.SelectedValue)
            .AddWithValue("@p1", ddlBasicProcessor.SelectedValue)
            .AddWithValue("@p2", ddlBasicMemory.SelectedValue)
            .AddWithValue("@p3", ddlBasicStorage.SelectedValue)
            .AddWithValue("@p4", datarowbasictransaction) 'this is for the basicID transaction #
        End With

        If gdtBTransactions.Rows.Count > 0 Then gdtBTransactions.Rows.Clear()
        Dim dr As DataRow = gdtBTransactions.NewRow

        dr("BasicID") = datarowbasictransaction + 1
        dr("UserID") = ddlBasicCustomer.SelectedValue
        dr("BProcessorID") = ddlBasicProcessor.SelectedValue
        dr("BMemoryID") = ddlBasicMemory.SelectedValue
        dr("BStorageID") = ddlBasicStorage.SelectedValue

        gdtBTransactions.Rows.Add(dr)

        Try
            If con.State = ConnectionState.Closed Then con.Open()

            cmdUpdateBasicTransactions.ExecuteNonQuery()
            gdaBTransactions.Update(gdtBTransactions)
            gdaBTransactions.Fill(gdtBTransactions)

            cmdUpdateProcessorInventory.ExecuteNonQuery()
            cmdUpdateMemoryInventory.ExecuteNonQuery()
            cmdUpdateStorageInventory.ExecuteNonQuery()

            gdaInventory.Fill(gdtInventory)

        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
            con.Close()
        End Try

        gvReceipt.DataSource = gdtCurrentBasicCost
        gvReceipt.DataBind()

        txtTotalCost.Text = gdecBasicTotal

        'For the current Cost
        gvBasicTransactions.DataSource = gdtBTransactions
        gvBasicTransactions.DataBind()

        'update inventory gv
        gvInventory.DataSource = gdtInventory
        gvInventory.DataBind()

        Call getBasicCost()
        Call UpdateCustomerBasicPurchase()
        Call UpdateDDL() 'this should be the last thing you call in the function 

        gdtCurrentBasicCost.Rows.Clear()
        gdtBTransactions.Rows.Clear()
        gdtInventory.Rows.Clear()

        mvPCConfiguration.ActiveViewIndex = 5
    End Sub
#End Region

#Region "Work Station Purchase"
    Protected Sub btnSubmitPurchaseWorkstation_Click(sender As Object, e As EventArgs) Handles btnSubmitPurchaseWorkstation.Click
        Dim cmdUpdateWSTransactions As New SqlCommand("UPDATE POSWorkstationTransactions SET UserID = @WSUser, ProcessorID = @p1, VideoCardID = @p2, MemoryID = @p3, StorageID = @p4 WHERE WorkstationID =@p5", con)
        Dim daUpdateTransaction As New SqlDataAdapter("SELECT * FROM POSServerTransactions", con)
        Dim datarowworkstation As Integer 'needed this so that we can edit the workstationID + 1

        Dim cmdUpdateProcessorInventory As New SqlCommand("UPDATE POSInventory SET StockRemaining -=1 WHERE ItemName = @p2", con)
        Dim cmdUpdateVideoCardInventory As New SqlCommand("UPDATE POSInventory SET StockRemaining -=1 WHERE ItemName = @p2", con)
        Dim cmdUpdateMemoryInventory As New SqlCommand("UPDATE POSInventory SET StockRemaining -=1 WHERE ItemName = @p2", con)
        Dim cmdUpdateStorageInventory As New SqlCommand("UPDATE POSInventory SET StockRemaining -=1 WHERE ItemName = @p2", con)

        'INVENTORY
        With cmdUpdateProcessorInventory.Parameters
            .Clear()
            .AddWithValue("@p2", ddlWorkstationProcessor.SelectedItem.Text)
        End With

        With cmdUpdateVideoCardInventory.Parameters
            .Clear()
            .AddWithValue("@p2", ddlWorkstationVideoCard.SelectedItem.Text)
        End With

        With cmdUpdateMemoryInventory.Parameters
            .Clear()
            .AddWithValue("@p2", ddlWorkstationMemory.SelectedItem.Text)
        End With

        With cmdUpdateStorageInventory.Parameters
            .Clear()
            .AddWithValue("@p2", ddlWorkstationStorage.SelectedItem.Text)
        End With

        'Processors - retrieves the value of the selected workstation processor
        With gdaWorkstationProcessorDDL.SelectCommand.Parameters
            .Clear()
            .AddWithValue("@p1", (ddlWorkstationProcessor.SelectedValue))
        End With

        'Video Cards - retrieves the value of the selected workstation video card
        With gdaWorkstationVideoCardDDL.SelectCommand.Parameters
            .Clear()
            .AddWithValue("@p2", (ddlWorkstationVideoCard.SelectedValue))
        End With

        'Memory - retrieves the value of the selected workstation memory
        With gdaWorkstationMemoryDDL.SelectCommand.Parameters
            .Clear()
            .AddWithValue("@p3", (ddlWorkstationMemory.SelectedValue))
        End With

        'Storage - retrieves the value of the selected workstation storage
        With gdaWorkstationStorageDDL.SelectCommand.Parameters
            .Clear()
            .AddWithValue("@p4", (ddlWorkstationStorage.SelectedValue))
        End With

        'User DDL ID - retrieves the user id
        With gdaWorkstationCustomer.SelectCommand.Parameters
            .Clear()
            .AddWithValue("@p1", ddlWorkstationCustomer.SelectedValue)
        End With

        'We couldnt make a called procedure for this, or else it would add information to all of the transaction tables.
        With cmdUpdateWSTransactions.Parameters
            .Clear()
            .AddWithValue("@WSUser", ddlWorkstationCustomer.SelectedValue)
            .AddWithValue("@p1", ddlWorkstationProcessor.SelectedValue)
            .AddWithValue("@p2", ddlWorkstationMemory.SelectedValue)
            .AddWithValue("@p3", ddlWorkstationVideoCard.SelectedValue)
            .AddWithValue("@p4", ddlWorkstationStorage.SelectedValue)
            .AddWithValue("@p5", datarowworkstation) 'this is for the workstationID transaction
        End With

        Try
            If con.State = ConnectionState.Closed Then con.Open()
            If gdtWSTransactions.Rows.Count > 0 Then gdtWSTransactions.Rows.Clear()

            cmdUpdateWSTransactions.ExecuteNonQuery()

            cmdUpdateProcessorInventory.ExecuteNonQuery()
            cmdUpdateVideoCardInventory.ExecuteNonQuery()
            cmdUpdateMemoryInventory.ExecuteNonQuery()
            cmdUpdateStorageInventory.ExecuteNonQuery()

            gdaInventory.Fill(gdtInventory)

            Dim dr As DataRow = gdtWSTransactions.NewRow

            dr("WorkstationID") = datarowworkstation + 1 'we had to create a variable and then add one to it, then make the column autoincrement for it to work. Before, it would edit ALL of the rows in the DB Table
            dr("UserID") = ddlWorkstationCustomer.SelectedValue
            dr("ProcessorID") = ddlWorkstationProcessor.SelectedValue
            dr("VideoCardID") = ddlWorkstationVideoCard.SelectedValue
            dr("MemoryID") = ddlWorkstationMemory.SelectedValue
            dr("StorageID") = ddlWorkstationStorage.SelectedValue
            gdtWSTransactions.Rows.Add(dr)

            cmdUpdateWSTransactions.ExecuteNonQuery()

            gdaWSTransactions.Update(gdtWSTransactions)
            gdaWSTransactions.Fill(gdtWSTransactions)

        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
            con.Close()
        End Try

        gvReceipt.DataSource = gdtCurrentWorkstationCost
        gvReceipt.DataBind()

        txtTotalCost.Text = gdecWorkstationTotal

        gvWorkstationTransactions.DataSource = gdtWSTransactions
        gvWorkstationTransactions.DataBind()

        'update inventory gv
        gvInventory.DataSource = gdtInventory
        gvInventory.DataBind()

        Call getWorkstationCost()
        Call UpdateCustomerWorkstationPurchase()
        Call UpdateDDL()

        gdtCurrentWorkstationCost.Rows.Clear() 'clear the rows of the current cost
        gdtWSTransactions.Rows.Clear() 'clear the rows of the current transaction

        mvPCConfiguration.ActiveViewIndex = 5
    End Sub
#End Region

#Region "Server Purchase"
    Protected Sub btnSubmitPurchaseServer_Click(sender As Object, e As EventArgs) Handles btnSubmitPurchaseServer.Click
        Dim cmdUpdateSTransactions As New SqlCommand("UPDATE POSServerTransactions SET UserID = @SUser, SProcessorID = @p1, SVideoCardID = @p2, SMemoryID = @p3, SStorageID = @p4 WHERE ServerID = @p5", con)
        Dim datarowserver As Integer 'this is needed for our ServerID

        Dim cmdUpdateServerProcessorInventory As New SqlCommand("UPDATE POSInventory SET StockRemaining -=1 WHERE ItemName = @p1", con)
        Dim cmdUpdateServerVideoCardInventory As New SqlCommand("UPDATE POSInventory SET StockRemaining -=1 WHERE ItemName = @p1", con)
        Dim cmdUpdateServerMemoryInventory As New SqlCommand("UPDATE POSInventory SET StockRemaining -=1 WHERE ItemName = @p1", con)
        Dim cmdUpdateServerStorageInventory As New SqlCommand("UPDATE POSInventory SET StockRemaining -=1 WHERE ItemName = @p1", con)

        'INVENTORY
        With cmdUpdateServerProcessorInventory.Parameters
            .Clear()
            .AddWithValue("@p1", ddlWorkstationProcessor.SelectedItem.Text)
        End With

        With cmdUpdateServerVideoCardInventory.Parameters
            .Clear()
            .AddWithValue("@p1", ddlWorkstationVideoCard.SelectedItem.Text)
        End With

        With cmdUpdateServerMemoryInventory.Parameters
            .Clear()
            .AddWithValue("@p1", ddlWorkstationMemory.SelectedItem.Text)
        End With

        With cmdUpdateServerStorageInventory.Parameters
            .Clear()
            .AddWithValue("@p1", ddlWorkstationStorage.SelectedItem.Text)
        End With

        'Processors
        With gdaServProcessorDDL.SelectCommand.Parameters
            .Clear()
            .AddWithValue("@p5", (ddlServerProcessor.SelectedValue))
        End With

        'Video Cards
        With gdaServVideoCardDDL.SelectCommand.Parameters
            .Clear()
            .AddWithValue("@p6", (ddlServerVideoCard.SelectedValue))
        End With

        'Memory
        With gdaServMemoryDDL.SelectCommand.Parameters
            .Clear()
            .AddWithValue("@p7", (ddlServerMemory.SelectedValue))
        End With

        'Storage
        With gdaServStorageDDL.SelectCommand.Parameters
            .Clear()
            .AddWithValue("@p8", (ddlServerStorage.SelectedValue))
        End With

        'User DDL ID
        With gdaServerCustomer.SelectCommand.Parameters
            .Clear()
            .AddWithValue("@p1", ddlServerCustomer.SelectedValue)
        End With

        With cmdUpdateSTransactions.Parameters
            .Clear()
            .AddWithValue("@SUser", ddlServerCustomer.SelectedValue)
            .AddWithValue("@p1", ddlServerProcessor.SelectedValue)
            .AddWithValue("@p2", ddlServerVideoCard.SelectedValue)
            .AddWithValue("@p3", ddlServerMemory.SelectedValue)
            .AddWithValue("@p4", ddlServerStorage.SelectedValue)
            .AddWithValue("@p5", datarowserver) 'this is needed for the ServerID 
        End With

        If gdtSTransactions.Rows.Count > 0 Then gdtSTransactions.Rows.Clear()
        Dim dr As DataRow = gdtSTransactions.NewRow

        dr("ServerID") = datarowserver + 1 'this adds one onto the serverID
        dr("UserID") = ddlServerCustomer.SelectedValue
        dr("SProcessorID") = ddlServerProcessor.SelectedValue
        dr("SVideoCardID") = ddlServerVideoCard.SelectedValue
        dr("SMemoryID") = ddlServerMemory.SelectedValue
        dr("SStorageID") = ddlServerStorage.SelectedValue

        gdtSTransactions.Rows.Add(dr)

        Try
            If con.State = ConnectionState.Closed Then con.Open()

            cmdUpdateSTransactions.ExecuteNonQuery()
            gdaSTransactions.Update(gdtSTransactions)
            gdaSTransactions.Fill(gdtSTransactions)

            cmdUpdateServerProcessorInventory.ExecuteNonQuery()
            cmdUpdateServerVideoCardInventory.ExecuteNonQuery()
            cmdUpdateServerMemoryInventory.ExecuteNonQuery()
            cmdUpdateServerStorageInventory.ExecuteNonQuery()

            gdaInventory.Fill(gdtInventory)

        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
            con.Close()
        End Try

        gvReceipt.DataSource = gdtCurrentServerCost
        gvReceipt.DataBind()

        txtTotalCost.Text = gdecServerTotal

        'update inventory gv
        gvInventory.DataSource = gdtInventory
        gvInventory.DataBind()

        gvServerTransactions.DataSource = gdtSTransactions
        gvServerTransactions.DataBind()

        Call getServerCost()
        Call UpdateCustomerServerPurchase()
        Call UpdateDDL()

        gdtCurrentServerCost.Rows.Clear() 'clear the rows of the current cost
        gdtSTransactions.Rows.Clear() 'clear the rows of the current transaction

        mvPCConfiguration.ActiveViewIndex = 5
    End Sub
#End Region

#Region "Go Back Buttons"
    Protected Sub btnGoBack3_Click(sender As Object, e As EventArgs) Handles btnGoBack3.Click
        mvPCConfiguration.ActiveViewIndex = 0
        rblHelpMeChoose.SelectedIndex = -1
        labelWhatWeRecommend.Visible = False
        imgbtnRecommendation.Visible = False
        labelHelpMeChoose.Text = Nothing
    End Sub

    Protected Sub btnGoBack_Click(sender As Object, e As EventArgs) Handles btnGoBack.Click
        mvPCConfiguration.ActiveViewIndex = 0
    End Sub

    Protected Sub btnGoBack0_Click(sender As Object, e As EventArgs) Handles btnGoBack0.Click
        mvPCConfiguration.ActiveViewIndex = 0
    End Sub

    Protected Sub btnGoBack1_Click(sender As Object, e As EventArgs) Handles btnGoBack1.Click
        mvPCConfiguration.ActiveViewIndex = 0
    End Sub

    'This is for the finish receipt, which will also clear the gridviews and current cost table
    Protected Sub btnGoBack4_Click(sender As Object, e As EventArgs) Handles btnFinishReceipt.Click
        mvPCConfiguration.ActiveViewIndex = 0
        gdtCurrentBasicCost.Rows.Clear()
        gdtCurrentWorkstationCost.Rows.Clear()
        gdtCurrentServerCost.Rows.Clear()
        gvReceipt.DataSource = Nothing
        gvReceipt.DataBind()
        txtTotalCost.Text = Nothing
    End Sub
#End Region

#Region "Help Me Choose"
    Protected Sub rblHelpMeChoose_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblHelpMeChoose.SelectedIndexChanged
        Select Case rblHelpMeChoose.SelectedIndex
            Case 0 'basic computing 
                helpmechooseActiveViewSelect = 0
                labelWhatWeRecommend.Visible = True
                imgbtnRecommendation.Visible = True
                imgbtnRecommendation.ImageUrl = "dellbasic.jpg"
                labelHelpMeChoose.Text = "Everyday Computing"
                helpmechooseActiveViewSelect += 1

            Case 1 'workstation 
                helpmechooseActiveViewSelect = 0
                labelWhatWeRecommend.Visible = True
                imgbtnRecommendation.Visible = True
                imgbtnRecommendation.ImageUrl = "dellxps.jpg"
                labelHelpMeChoose.Text = "Workstation"
                helpmechooseActiveViewSelect += 2

            Case 2 'server 
                helpmechooseActiveViewSelect = 0
                labelWhatWeRecommend.Visible = True
                imgbtnRecommendation.Visible = True
                imgbtnRecommendation.ImageUrl = "dellserver.jpg"
                labelHelpMeChoose.Text = "Server Solutions"
                helpmechooseActiveViewSelect += 3
        End Select
    End Sub

    'this determines where the image button on the helpmechoose view will bring you. 
    Protected Sub imgbtnRecommendation_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnRecommendation.Click
        If helpmechooseActiveViewSelect = 1 Then mvPCConfiguration.ActiveViewIndex = 2
        If helpmechooseActiveViewSelect = 2 Then mvPCConfiguration.ActiveViewIndex = 3
        If helpmechooseActiveViewSelect = 3 Then mvPCConfiguration.ActiveViewIndex = 4
        rblHelpMeChoose.SelectedIndex = -1
        imgbtnRecommendation.Visible = False
        imgbtnRecommendation.ImageUrl = Nothing
        labelHelpMeChoose.Text = Nothing
    End Sub
#End Region

#Region "Views for PC Customization (Multiview 1)"
    Protected Sub lbHelpMeChoose_Click(sender As Object, e As EventArgs) Handles lbHelpMeChoose.Click
        mvPCConfiguration.ActiveViewIndex = 1 'this brings us to the help me choose page 
    End Sub

    Protected Sub imgbtnBasicPC_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnBasicPC.Click
        mvPCConfiguration.ActiveViewIndex = 2 'this brings us to the basic computer page 
        labelBasicPrice.Text = Nothing
    End Sub

    Protected Sub imgbtnWorkstationPC_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnWorkstationPC.Click
        mvPCConfiguration.ActiveViewIndex = 3 'this brings us to the workstation page 
        labelWorkstationPrice.Text = Nothing
    End Sub

    Protected Sub imgbtnServerPC_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnServerPC.Click
        mvPCConfiguration.ActiveViewIndex = 4 'this brings us to the server solutions page 
        labelServerPrice.Text = Nothing
    End Sub
#End Region

#Region "Views for Admin (Multiview 2)"
    'this is all for our admin purposes, where we can see all customer and product inventory data.
    Protected Sub lbAdminView_Click(sender As Object, e As EventArgs) Handles lbAdminView.Click
        mvAdmin.Visible = True
        mvPCConfiguration.Visible = False
        mvAdmin.ActiveViewIndex = 0
    End Sub

    Protected Sub lbCustomers_Click(sender As Object, e As EventArgs) Handles lbCustomers.Click
        mvAdmin.ActiveViewIndex = 1
        Call getAllCustomers()
    End Sub

    Protected Sub lbProducts_Click(sender As Object, e As EventArgs) Handles lbTransactions.Click
        mvAdmin.ActiveViewIndex = 2
        gdaBTransactions.Fill(gdtBTransactions)
        gvBasicTransactions.DataSource = gdtBTransactions
        gvBasicTransactions.DataBind()

        gdaWSTransactions.Fill(gdtWSTransactions)
        gvWorkstationTransactions.DataSource = gdtWSTransactions
        gvWorkstationTransactions.DataBind()

        gdaSTransactions.Fill(gdtSTransactions)
        gvServerTransactions.DataSource = gdtSTransactions
        gvServerTransactions.DataBind()
    End Sub

    Protected Sub lbInventoryPage_Click(sender As Object, e As EventArgs) Handles lbInventoryPage.Click
        mvAdmin.ActiveViewIndex = 3
        gdaInventory.Fill(gdtInventory)
        gvInventory.DataSource = gdtInventory
        gvInventory.DataBind()
    End Sub

    Protected Sub lbGoBacktoMainPage_Click(sender As Object, e As EventArgs) Handles lbGoBacktoMainPage.Click
        mvPCConfiguration.Visible = True
        mvPCConfiguration.ActiveViewIndex = 0
        mvAdmin.Visible = False
    End Sub
#End Region

#Region "Login and CAPTCHA"
    'We found this code online through the CAPTCHA website. This will prevent bots from logging in. 
    'IF the user enters the correct CAPTCHA information, it will bring them to the login page. If not, it will just keep them there- could implement a page freeze but i dont know javascript.
    Protected Sub Page_PreRender(ByVal sender As Object,
    ByVal e As System.EventArgs) Handles Me.PreRender
        ' initial page setup
        If Not IsPostBack Then
            ' set control text
            ValidateCaptchaButton.Text = "Validate"
        End If

        If IsPostBack Then
            ' validate the Captcha to check we're not dealing with a bot
            Dim isHuman As Boolean
            isHuman = ExampleCaptcha.Validate()
            If isHuman Then
                mvLaunch.ActiveViewIndex = 2 ' this should bring them to the login page.
            Else
                CaptchaCodeTextBox.Text = Nothing
            End If
        End If
    End Sub

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        'Here we use the supplied login information to count the number of rows there are in the database table of registered users that match the login information (user ID and password). If zero rows are returned then the login is not valid. If 1 row was found in the registered users table, then the login information does match that of a registered user. If 1 row of information is found then move from the login page to another page - presumably the first page in your application.

        'This next line Is a SQLCommand that runs a SQL statement that counts the number of rows that have the values passed in by the two parameters (userID And password). so if neither of the paramaters match with what is in the database, it wont increment the intRows variable. 
        Dim cmdCheckRegistrant As New SqlCommand("SELECT COUNT(*) FROM RegisteredUsers WHERE UserID = @p1 AND Password = @p2", con)

        'this next variable will be used to receive the result of running the SELECT COUNT query
        Dim intRows As Integer = 0

        'we dont want them to be able to enter any data if it is all empty
        If txtPassword.Text = Nothing OrElse txtUserID.Text = Nothing Then
            Response.Write("Enter all Data")
            Exit Sub
        Else
            Response.Clear()
        End If

        'if the user tried 3 times then they need to be locked out (0,1,2). You will need to find additional javascript code to freeze the webpage including locking the go back link. Here the screen controls are made invisible.
        If gintloopcounter = 2 Then
            Response.Write("System locked due to excessive attempts")
            'ValidateCaptchaButton.Visible = False
            txtUserID.Visible = False
            txtPassword.Visible = False
            Exit Sub
        End If

        'The SQLCommand is parameterized so these next few lines take the values from the webpage controls and assign them to the parameters so that the values are passed into the SQL SELECT statement when the command is executed below using the .executescalar method.
        With cmdCheckRegistrant.Parameters
            .Clear()
            .AddWithValue("@p1", txtUserID.Text) 'THIS IS BEING PASSED INTO THE USERID PARAMETER
            .AddWithValue("@p2", txtPassword.Text) 'THIS IS BEING PASSED INTO THE PASSWORD PARAMETER
        End With

        'Here we run the parameterized SQLCommand which returns one number that we assign to a local variable (intRows). If you are in the middle of your own programming and you realize you need to retrieve one value from the database (not a row of data) then use .executescalar.
        Try
            If con.State = ConnectionState.Closed Then con.Open()
            intRows = cmdCheckRegistrant.ExecuteScalar
            'THE EXECUTESCALAR WILL RETURN A NUMBER, EITHER 1 OR 0------ IF BOTH THE USERID AND PASSWORD MATCH WHAT WAS PUT INTO THE TEXTBOX IT WILL BE 1, IF NEITHER MATCH, IT WILL BE 0 SINCE THERE ARE NO REGISTERED USERS THAT MATCH WHAT IS IN THE DATABASE.

            'We check the number of rows (calculated from the SELECT COUNT(*)) to see if a row of data was found in the registered user table with the supplied userID and password. Below we increment the global counter variable if no registered user was found.
            If intRows = 0 Then
                gintloopcounter += 1
                Response.Write("No User with that UserID and Password " & gintloopcounter)
                txtUserID.Text = Nothing
                txtPassword.Text = Nothing
            End If

            'if a user was found in the approved user table then navigate to another view
            If intRows = 1 Then

                'HERE WE REDIRECT TO OUR MAIN PROGRAM.
                mvLaunch.ActiveViewIndex = -1
                mvPCConfiguration.ActiveViewIndex = 0
                panelAdmin.Visible = True
                panelLaunch.Visible = False
                txtLoginPopup.Visible = False
                txtLoginPopup.Text = Nothing
                Exit Sub
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
            con.Close()
        End Try
    End Sub

    Protected Sub btnForgotLogin_Click(sender As Object, e As EventArgs) Handles btnForgotLogin.Click
        txtLoginPopup.Visible = True
        txtLoginPopup.Text = "UserID = Butch " & vbNewLine & "Password = Coug "
    End Sub
    Protected Sub lbLogin_Click(sender As Object, e As EventArgs) Handles lbLogin.Click
        mvLaunch.ActiveViewIndex = 1
    End Sub
#End Region

#Region "Label Prices For Each Build"
    'Changes the label value when the ddls are changed.
    Protected Sub ddlBasicProcessor_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlBasicProcessor.SelectedIndexChanged
        Call getBasicCost()
    End Sub

    Protected Sub ddlBasicMemory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlBasicMemory.SelectedIndexChanged
        Call getBasicCost()
    End Sub

    Protected Sub ddlBasicStorage_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlBasicStorage.SelectedIndexChanged
        Call getBasicCost()
    End Sub

    Protected Sub ddlWorkstationProcessor_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlWorkstationProcessor.SelectedIndexChanged
        Call getWorkstationCost()
    End Sub

    Protected Sub ddlWorkstationVideoCard_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlWorkstationVideoCard.SelectedIndexChanged
        Call getWorkstationCost()
    End Sub

    Protected Sub ddlWorkstationMemory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlWorkstationMemory.SelectedIndexChanged
        Call getWorkstationCost()
    End Sub

    Protected Sub ddlWorkstationStorage_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlWorkstationStorage.SelectedIndexChanged
        Call getWorkstationCost()
    End Sub

    Protected Sub ddlServerProcessor_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlServerProcessor.SelectedIndexChanged
        Call getServerCost()
    End Sub

    Protected Sub ddlServerVideoCard_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlServerVideoCard.SelectedIndexChanged
        Call getServerCost()
    End Sub

    Protected Sub ddlServerMemory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlServerMemory.SelectedIndexChanged
        Call getServerCost()
    End Sub

    Protected Sub ddlServerStorage_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlServerStorage.SelectedIndexChanged
        Call getServerCost()
    End Sub
#End Region

End Class












