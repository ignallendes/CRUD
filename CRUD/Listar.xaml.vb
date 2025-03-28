Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics.Eventing.Reader

Class Listar
    Inherits Window

    Private conexion As String = "Server=(localdb)\NachoBD;Database=iac;Integrated Security=True;"

    ' Constructor de la ventana
    Public Sub New()
        InitializeComponent()
        CargarTablas()
        CargarDatosTabla("Empleado")
        cbxTablas.SelectedItem = "Empleado"
    End Sub

    ' Método para cargar las tablas en el ComboBox
    Private Sub CargarTablas()
        Try
            Using conn As New SqlConnection(conexion)
                conn.Open()
                Dim query As String = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_SCHEMA NOT IN ('sys', 'information_schema')"
                Dim cmd As New SqlCommand(query, conn)
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                ' Limpiar el ComboBox antes de agregar nuevos elementos
                cbxTablas.Items.Clear()

                While reader.Read()
                    cbxTablas.Items.Add(reader("TABLE_NAME").ToString())
                End While
                reader.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error al cargar las tablas: " & ex.Message)
        End Try
    End Sub

    Private Sub cbxTablas_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cbxTablas.SelectionChanged
        If cbxTablas.SelectedItem IsNot Nothing Then
            Dim tablaSeleccionada As String = cbxTablas.SelectedItem.ToString()
            CargarDatosTabla(tablaSeleccionada)
        End If
    End Sub

    ' Método para cargar los datos de la tabla seleccionada

    Private Sub CargarDatosTabla(tabla As String)
        Try
            Using conn As New SqlConnection(conexion)
                conn.Open()

                ' Si la tabla es "Empleado", hacemos la consulta específica
                If tabla = "Empleado" Then
                    Dim query As String = "
                    SELECT 
                        e.rut,
                        e.nombre,
                        e.direccion,
                        e.telefono,
                        c.cargo
                    FROM 
                        dbo.Empleado e
                    INNER JOIN 
                        dbo.sinNombre sn ON e.rut = sn.rut
                    INNER JOIN 
                        dbo.Cargos c ON sn.id_cargo = c.id_cargo
                "
                    Dim cmd As New SqlCommand(query, conn)
                    Dim da As New SqlDataAdapter(cmd)
                    Dim dt As New DataTable()

                    ' Llenar el DataTable con los datos de la consulta
                    da.Fill(dt)

                    ' Vincular el DataTable al DataGrid
                    GrillaEmpleados.ItemsSource = dt.DefaultView
                Else
                    ' Si la tabla no es "Empleado", hacer un SELECT * FROM <tabla>
                    Dim query As String = "SELECT * FROM " & tabla
                    Dim cmd As New SqlCommand(query, conn)
                    Dim da As New SqlDataAdapter(cmd)
                    Dim dt As New DataTable()

                    ' Llenar el DataTable con los datos de la consulta
                    da.Fill(dt)

                    ' Vincular el DataTable al DataGrid
                    GrillaEmpleados.ItemsSource = dt.DefaultView
                End If

            End Using
        Catch ex As Exception
            MessageBox.Show("Error al cargar los datos de la tabla: " & ex.Message)
        End Try
    End Sub

    Private Sub btbAtras_Click(sender As Object, e As RoutedEventArgs) Handles btbAtras.Click
        Dim ventanaListar As New MainWindow()
        ventanaListar.Show() ' Abre la ventana Listar.xaml
        Me.Close() ' Cierra MainWindow (opcional)
    End Sub
End Class
