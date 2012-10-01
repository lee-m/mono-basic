Public Class SomeClass

    Public MustInherit Class TestClass

        Public Property Foo As Integer

    End Class

    Public Sub Test()
        Dim a As New TestClass With {.Foo = 42}
    End Sub

End Class