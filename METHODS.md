# B1Core Methods Documentation

## Main Class
The central class that manages SAP B1 company connection and provides core functionality.

### Methods
- `SetupControl(string tableName, int version)`: Controls the setup version of a table

### Properties
- `oCompany`: SAP B1 company connection object

## Data Class
Handles data operations and database interactions.

### Methods
- `ReadSingleData(string table, string column, string columnWhere, string valueWhere)`: Reads a single value from a table
- `ReadSingleData(string table, string column, Dictionary<string, string> whereColumnValue)`: Reads a single value with multiple conditions
- `ReadSingleData(string table, Dictionary<string, string> whereColumnValue)`: Reads a single record with conditions
- `ReadListData(string table, Dictionary<string, string> whereColumnValue, int selectTop = 0)`: Reads multiple records with conditions
- `DeleteData(string table, string columnWhere, string valueWhere)`: Deletes records with a single condition
- `DeleteData(string table, Dictionary<string, string> whereColumnValue)`: Deletes records with multiple conditions
- `ExecuteSql(string sql, SqlObjectType sqlType = SqlObjectType.Sql)`: Executes SQL queries

## Form Class
Handles SAP B1 form operations and UI components.

### Methods
- `GetActiveForm()`: Gets the currently active form
- `SetCenter()`: Centers the active form on screen
- `AddButton(Form oForm, string uniqId, int pane, string caption, int top, int left, int width, int height)`: Adds a button to a form
- `AddCheckBox(Form oForm, string table, string field, string uniqId, int pane, int top, int left, int width, int height)`: Adds a checkbox to a form
- `AddFolder(Form oForm, string folderId, string caption, string folderBaseId)`: Adds a folder to a form
- `AddEditText(Form oForm, string table, string field, string uniqId, int pane, int top, int left, int width, int height, bool visible = true)`: Adds an edit text field
- `AddComboBox(Form oForm, string table, string field, string uniqId, int pane, int top, int left, int width, int height)`: Adds a combobox to a form
- `Refresh()`: Refreshes the active form
- `AddMode()`: Switches form to add mode

## TableCreate Class
Handles table creation and management in SAP B1.

### Methods
- `CreateTable(string TableName, string TableDesc, BoUTBTableType TableType)`: Creates a new table
- `CreateUserFields(string TableName, string FieldName, string FieldDescription, BoFieldTypes type, long size = 0, BoFldSubTypes subType = BoFldSubTypes.st_None, string LinkedTable = "", string DefaultValue = "", Dictionary<string, string> ValidValues = null)`: Creates user-defined fields
- `AddUDO(string TableName, BoUDOObjType UDOType, List<string> childTable = null)`: Adds a User-Defined Object

## FmsCreate Class
Handles formatted searches and queries in SAP B1.

### Methods
- `QueryAdd(int categoryId, string sql, string queryName, bool fmsAdd = false, string formId = "", string itemId = "", string colId = "", string colIdRelation = "")`: Adds a new query
- `FmsAdd(int categoryId, string queryName, string formId, string ItemId, string colId, string colIdRelation)`: Adds a formatted search
- `FmsRemove(string categoryName)`: Removes formatted searches
- `GetCategoryId(string categoryName)`: Gets category ID for queries
- `GetTableFormId(string tableName)`: Gets form ID for a table

## ProcCreate Class
Handles database procedure creation.

### Methods
- `CreateProcedure(string sql, string procName)`: Creates a stored procedure
- `CreateView(string sql, string viewName)`: Creates a database view
- `CreateTrigger(string sql, string triggerName)`: Creates a database trigger
- `CreateFunction(string sql, string function)`: Creates a database function

## DataHelper Class
Provides helper methods for data operations.

### Methods
- `GetValueString(Recordset data, string column)`: Gets string value from recordset
- `GetValueDecimal(Recordset data, string column)`: Gets decimal value from recordset
- `GetValueDate(Recordset data, string column)`: Gets date value from recordset
- `GetValueTime(Recordset data, string column, string columDate)`: Gets time value from recordset

## Menu Class
Handles menu creation and management.

### Methods
- `CreateMenuItem(string menuId, string menuTitle, string formName, int order)`: Creates a menu item
- `CreateMenuItem(SAPbouiCOM.Menus oMenus, SAPbouiCOM.MenuCreationParams oCreationPackage, string text, string uniqueID, int order)`: Creates a menu item with parameters
- `CreateMenuItemMainMenu(string id, string title, string logo = "", string menuId = "43520")`: Creates a main menu item

## StatusBar Class
Handles status bar messages.

### Methods
- `SetMessage(BoStatusBarMessageType messageType, string message, BoMessageTime messageTime = BoMessageTime.bmt_Medium)`: Sets a status bar message

## Response Class
Generic response class for method returns.

### Properties
- `Success`: Indicates if the operation was successful
- `Message`: Response message
- `Data`: Response data
- `RecordCount`: Number of records affected 