Imports System.Data
Imports System.Data.SqlClient

Class Listar
    Inherits Window

    Private conexion As String = "Server=(localdb)\NachoBD;Database=iac;Integrated Security=True;"

    ' Constructor de la ventana
    Public Sub New()
        InitializeComponent()
        CargarTablas()
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

    ' Evento que se dispara al cambiar la selección en el ComboBox
    Private Sub cbxTablas_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cbxTablas.SelectionChanged
        ' Verificar si se seleccionó una tabla en el ComboBox
        If cbxTablas.SelectedItem IsNot Nothing Then
            ' Obtener el nombre de la tabla seleccionada
            Dim tablaSeleccionada As String = cbxTablas.SelectedItem.ToString()

            ' Llamar al método para cargar los datos de la tabla seleccionada
            CargarDatosTabla(tablaSeleccionada)
        End If
    End Sub

    ' Método para cargar los datos de la tabla seleccionada
    Private Sub CargarDatosTabla(tabla As String)
        Try
            Using conn As New SqlConnection(conexion)
                conn.Open()
                ' Crear la consulta SQL para obtener todos los registros de la tabla seleccionada
                Dim query As String = "SELECT * FROM " & tabla
                Dim cmd As New SqlCommand(query, conn)
                Dim da As New SqlDataAdapter(cmd)
                Dim dt As New DataTable()

                ' Llenar el DataTable con los datos de la tabla seleccionada
                da.Fill(dt)

                ' Vincular el DataTable al DataGrid
                GrillaEmpleados.ItemsSource = dt.DefaultView
            End Using
        Catch ex As Exception
            MessageBox.Show("Error al cargar los datos de la tabla: " & ex.Message)
        End Try
    End Sub
End Class
