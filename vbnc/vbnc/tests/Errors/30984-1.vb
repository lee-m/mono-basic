Public Module SomeModule

	Class Customer
		Public Name As String
		Public Age As Integer
	End Class

	Function Main(ByVal cmdArgs() As String) As Integer
  
		Dim c As New Customer() With { _
			.Name "Foo", _
			Age = 20 _
			}
		
		Return 0
		
  End Function
		
End Module
