

''' <summary>
''' FieldInitializerList  ::=
'''	    FieldInitializer  |
'''	    FieldInitializerList  Comma  FieldInitializer
''' 
''' FieldInitializer ::=  
'''     .  IdentifierOrKeyword  Equals  ]  Expression
''' </summary>
''' <remarks></remarks>
Public Class FieldInitialiser
    Inherits ParsedObject

    ''' <summary>
    ''' Accessor expression for the field being initialised.
    ''' </summary>
    ''' <remarks></remarks>
    Private m_FieldName As IdentifierOrKeyword

    ''' <summary>
    ''' Expression to initialise the field with.
    ''' </summary>
    ''' <remarks></remarks>
    Private m_Expression As Expression

    Public Sub New(Parent As ParsedObject)
        MyBase.New(Parent)
    End Sub

    ''' <summary>
    ''' Initialises the field initialiser.
    ''' </summary>
    ''' <param name="FieldAccessor">Identifier of the field being initialised.</param>
    ''' <param name="InitialisationExpression">The expression to initialise the field with.</param>
    ''' <remarks></remarks>
    Public Sub Init(FieldAccessor As IdentifierOrKeyword, InitialisationExpression As Expression)
        m_FieldName = FieldAccessor
        m_Expression = InitialisationExpression
    End Sub

    ''' <summary>
    ''' Accessor for the field identifier.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property FieldName As IdentifierOrKeyword
        Get
            Return m_FieldName
        End Get
    End Property

    ''' <summary>
    ''' Accessor for the expression to initialise the field with.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property InitialisationExpression As Expression
        Get
            Return m_Expression
        End Get
    End Property

End Class
