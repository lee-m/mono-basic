Public Interface SomeInterface

    Property Foo As Integer
End Interface

Public Class SomeClass
    Implements SomeInterface

    Public Property ImplementsAutoProp As Integer Implements SomeInterface.Foo Implements SomeInterface.Foo
		
		Public Property SomeProp As Integer Implements SomeInterface.Foo
		
End Class
