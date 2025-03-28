Class MainWindow
    Private Sub btnListar_Click(sender As Object, e As RoutedEventArgs) Handles btnListar.Click

        Dim ventanaListar As New Listar()
        ventanaListar.Show() ' Abre la ventana Listar.xaml
        Me.Close() ' Cierra MainWindow (opcional)

    End Sub
End Class
