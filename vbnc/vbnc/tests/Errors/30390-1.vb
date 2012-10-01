Public Class SomeClass

	Public Class TestClass
		Private Field As Integer
	End Class
	
	Public Sub Test()
		Dim a As New TestClass With { .Field = 42 }
	End Sub
	
End Class