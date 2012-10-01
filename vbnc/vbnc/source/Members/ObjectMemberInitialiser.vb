' 
' Visual Basic.Net Compiler
' Copyright (C) 2004 - 2010 Rolf Bjarne Kvinge, RKvinge@novell.com
' 
' This library is free software; you can redistribute it and/or
' modify it under the terms of the GNU Lesser General Public
' License as published by the Free Software Foundation; either
' version 2.1 of the License, or (at your option) any later version.
' 
' This library is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
' Lesser General Public License for more details.
' 
' You should have received a copy of the GNU Lesser General Public
' License along with this library; if not, write to the Free Software
' Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
' 

''' <summary>
''' ObjectMemberInitializer  ::=
'''	    With  OpenCurlyBrace  FieldInitializerList  CloseCurlyBrace
''' </summary>
''' <remarks></remarks>
Public Class ObjectMemberInitializer
    Inherits ParsedObject

    ''' <summary>
    ''' Set of initialisation statements which will initialise each field of the object with the required value.
    ''' </summary>
    ''' <remarks></remarks>
    Private m_InitialisationStmts As Collections.Generic.List(Of AssignmentStatement)

    ''' <summary>
    ''' Creates a new instance of this class attached to the specified parent.
    ''' </summary>
    ''' <param name="Parent">Parent of this object.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal Parent As ParsedObject)
        MyBase.New(Parent)
    End Sub

    ''' <summary>
    ''' Initialises this object instance.
    ''' </summary>
    ''' <param name="fieldInitialisers">Field initialisers.</param>
    ''' <param name="InitTarget">The target object whose fields are being initialised.</param>
    ''' <remarks></remarks>
    Public Sub Init(ByVal FieldInitialisers As Collections.Generic.List(Of FieldInitialiser), ByVal InitTarget As Expression)

        m_InitialisationStmts = New Collections.Generic.List(Of AssignmentStatement)

        'Expand the field initialisers into a series of assignment statements initialising each field with
        'the required value
        For Each fieldInit As FieldInitialiser In FieldInitialisers

            'Build "target.Field" access expression
            Dim fieldAccessExpr As New MemberAccessExpression(Me)
            fieldAccessExpr.Init(InitTarget, fieldInit.FieldName)

            'Build "target.Field = Expr" to assign the value
            Dim assignStmt As New AssignmentStatement(Me)
            assignStmt.Init(fieldAccessExpr, fieldInit.InitialisationExpression)
            m_InitialisationStmts.Add(assignStmt)

        Next

    End Sub

    ''' <summary>
    ''' Resolves each generated assignment statement.
    ''' </summary>
    ''' <param name="Info">Resolve info.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function ResolveCode(ByVal Info As ResolveInfo) As Boolean

        Dim result As Boolean = True

        For Each assignStmt As AssignmentStatement In m_InitialisationStmts
            result = assignStmt.ResolveStatement(Info) AndAlso result
        Next

        Return result

    End Function

    ''' <summary>
    ''' Generates the required MSIL for the field initialisations.
    ''' </summary>
    ''' <param name="Info">Code generation info.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Overrides Function GenerateCode(Info As EmitInfo) As Boolean

        Dim result As Boolean = True

        For Each assignStmt As AssignmentStatement In m_InitialisationStmts
            result = assignStmt.GenerateCode(Info) AndAlso result
        Next

        Return result

    End Function

End Class
