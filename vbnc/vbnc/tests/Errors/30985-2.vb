Public Module SomeModule

	Public Class Customer
		Public Name As String
		Public Address As String 
	End Class
	
	Function Main(ByVal cmdArgs() As String) As Integer
  
		Dim x As New Customer() With { Key .Name = "Bob Smith", .Address = .Name & " St." }
		Return 0
		
  End Function
		
End Module