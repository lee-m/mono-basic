Public Class SomeClass

	Public Class TestClass
        Public Foo As Integer
    End Class
	
	Public Sub Test()
		Dim a As New TestClass With { .XXX = 42 }
	End Sub
	
End Class