Imports System.Data.SqlClient
Imports System.Web.UI.DataVisualization.Charting

Public Class _Default
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindGridView()
        End If
    End Sub

    Private Sub BindGridView()
        Dim connectionString As String = "Data Source=localhost\MSSQLSERVER01;Initial Catalog=parthiban;Integrated Security=True"
        Dim query As String = "SELECT ID, FirstName, LastName FROM Student"

        Using connection As New SqlConnection(connectionString)
            connection.Open()
            Using command As New SqlCommand(query, connection)
                Dim adapter As New SqlDataAdapter(command)
                Dim dataSet As New DataSet()
                adapter.Fill(dataSet)
                GridView1.DataSource = dataSet.Tables(0)
                GridView1.DataBind()
            End Using
        End Using
    End Sub

    Protected Sub btnGenerateChart_Click(sender As Object, e As EventArgs) Handles btnGenerateChart.Click
        Dim studentID As Integer
        If Integer.TryParse(txtStudentID.Text, studentID) Then
            Dim connectionString As String = "Data Source=localhost\MSSQLSERVER01;Initial Catalog=parthiban;Integrated Security=True"
            Dim query As String = "SELECT MathMarks, ScienceMarks, HistoryMarks, EnglishMarks, GeographyMarks FROM Student WHERE ID = @StudentID"

            Using connection As New SqlConnection(connectionString)
                connection.Open()
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@StudentID", studentID)

                    Dim reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        Dim marksData As Integer() = {
                            Convert.ToInt32(reader("MathMarks")),
                            Convert.ToInt32(reader("ScienceMarks")),
                            Convert.ToInt32(reader("HistoryMarks")),
                            Convert.ToInt32(reader("EnglishMarks")),
                            Convert.ToInt32(reader("GeographyMarks"))
                        }

                        GeneratePieChart(marksData)
                    Else
                        ClientScript.RegisterStartupScript(Me.GetType(), "alert", "alert('Student not found.');", True)
                    End If

                    reader.Close()
                End Using
            End Using
        Else
            ClientScript.RegisterStartupScript(Me.GetType(), "alert", "alert('Invalid Student ID.');", True)
        End If
    End Sub
    'Private Sub GeneratePieChart(ByVal data As Integer())
    '    Dim chart = New Chart()
    '    Dim chartArea = New ChartArea()
    '    chart.ChartAreas.Add(chartArea)
    '    Dim series = New Series()
    '    chart.Series.Add(series)

    '    For Each mark In data
    '        Dim point = New DataPoint()
    '        point.YValues = {mark}
    '        series.Points.Add(point)
    '    Next

    '    Dim canvas = "document.getElementById('myChart').getContext('2d');"
    '    canvas += chart.GetHtmlImageMap("ImageMap1")

    '    ClientScript.RegisterStartupScript(Me.GetType(), "canvas", canvas, True)
    'End Sub
    Private Sub GeneratePieChart(ByVal data As Integer())
        Dim marksSeries As Series = Chart1.Series("Marks")
        marksSeries.Points.Clear()

        Dim subjects As String() = {"Math", "Science", "History", "English", "Geography"}

        For i As Integer = 0 To data.Length - 1
            marksSeries.Points.AddXY(subjects(i), data(i))
        Next
    End Sub
End Class