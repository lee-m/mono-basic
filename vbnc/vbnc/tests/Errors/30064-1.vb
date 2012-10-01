Public Class SomeClass

	Public Class TestClass
		Public ReadOnly Field As Integer = 50
	End Class
	
	Public Sub Test()
		Dim a As New TestClass With { .Field = 42 }
	End Sub
	
End Class