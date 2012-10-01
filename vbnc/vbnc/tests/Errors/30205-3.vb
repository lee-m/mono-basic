Public Module SomeModule

	Class Customer
		Public Name As String
		Public Age As Integer
	End Class

	Function Main(ByVal cmdArgs() As String) As Integer
  
		Dim c As Customer() With { _
			.Name = "Foo", _
			.Address = New Address(), _
			.Address.Street = "Main St" _
			}
		
		Return 0
		
  End Function
		
End Module
