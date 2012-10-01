Public Module SomeModule

	Class Customer
		Public Name As String
		Public Address As Address
		Public Age As Integer
	End Class

	Class Address
		Public Street As String
		Public City As String
		Public State As String
		Public ZIP As String
	End Class

	Function Main(ByVal cmdArgs() As String) As Integer
  
		Dim c As New Customer() With { _
			.Name = "Foo", _
			.Address = New Address(), _
			.Address.Street = "Main St" _
			}
		
		Return 0
		
  End Function
		
End Module
