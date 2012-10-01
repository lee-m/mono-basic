Public Class SomeClass

	Public Class TestClass
		
		Public ReadOnly Property Foo As Integer
			Get
				Return 0
			End Get
		End Property
		
	End Class
	
	Public Sub Test()
		Dim a As New TestClass With  .Foo = 42 }
	End Sub
	
End Class