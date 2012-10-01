Public Class SomeClass

	Public Class TestClass
		
		Public Foo As Integer
		Public Bar As Integer 
	End Class
	
	Public Sub Test()
		Dim a As New TestClass With { .Foo = 1, }
	End Sub
	
End Class