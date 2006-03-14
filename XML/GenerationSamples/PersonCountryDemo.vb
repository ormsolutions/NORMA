Imports System
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Xml
Imports GeneratedCodeAttribute = System.CodeDom.Compiler.GeneratedCodeAttribute
Imports SuppressMessageAttribute = System.Diagnostics.CodeAnalysis.SuppressMessageAttribute
#Region "Global Support Classes"
Namespace System
	#Region "Tuple Support"
	Public MustInherit Partial Class Tuple
		Protected Shared Function RotateRight(ByVal value As Integer, ByVal places As Integer) As Integer
			places = places And &H1F
			If places = 0 Then
				Return value
			End If
			Dim mask As Integer = Not &H7FFFFFF >> (places - 1)
			Return ((value >> places) And Not mask) Or ((value << (32 - places)) And mask)
		End Function
	End Class
	#End Region
	#Region "2-ary Tuple"
	Public MustInherit Partial Class Tuple
		Public Overloads Shared Function CreateTuple(Of T1, T2)(ByVal item1 As T1, ByVal item2 As T2) As Tuple(Of T1, T2)
			If (item1 Is Nothing) OrElse (item2 Is Nothing) Then
				Return Nothing
			End If
			Return New Tuple(Of T1, T2)(item1, item2)
		End Function
	End Class
	<System.ComponentModel.ImmutableObjectAttribute(True)> _
	Public NotInheritable Class Tuple(Of T1, T2)
		Inherits Tuple
		Implements System.IEquatable(Of Tuple(Of T1, T2))
		Private ReadOnly item1 As T1
		Public ReadOnly Property Item1() As T1
			Get
				Return Me.item1
			End Get
		End Property
		Private ReadOnly item2 As T2
		Public ReadOnly Property Item2() As T2
			Get
				Return Me.item2
			End Get
		End Property
		Public Sub New(ByVal item1 As T1, ByVal item2 As T2)
			If (item1 Is Nothing) OrElse (item2 Is Nothing) Then
				Throw New System.ArgumentNullException()
			End If
			Me.item1 = item1
			Me.item2 = item2
		End Sub
		Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
			Return Me.Equals(TryCast(obj, Tuple(Of T1, T2)))
		End Function
		Public Overloads Function Equals(ByVal other As Tuple(Of T1, T2)) As Boolean Implements _
			System.IEquatable(Of Tuple(Of T1, T2)).Equals
			If (other Is Nothing) OrElse (Not (Me.item1.Equals(other.item1)) OrElse Not (Me.item2.Equals(other.item2))) Then
				Return False
			End If
			Return True
		End Function
		Public Overrides Function GetHashCode() As Integer
			Return Me.item1.GetHashCode() Xor Tuple.RotateRight(Me.item2.GetHashCode(), 1)
		End Function
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Function ToString(ByVal provider As System.IFormatProvider) As String
			Return String.Format(provider, "({1}, {2})", Me.item1, Me.item2)
		End Function
		Public Shared Operator =(ByVal tuple1 As Tuple(Of T1, T2), ByVal tuple2 As Tuple(Of T1, T2)) As Boolean
			If Not (Object.ReferenceEquals(tuple1, Nothing)) Then
				Return tuple1.Equals(tuple2)
			End If
			Return Object.ReferenceEquals(tuple2, Nothing)
		End Operator
		Public Shared Operator <>(ByVal tuple1 As Tuple(Of T1, T2), ByVal tuple2 As Tuple(Of T1, T2)) As Boolean
			Return Not (tuple1 = tuple2)
		End Operator
	End Class
	#End Region
	#Region "3-ary Tuple"
	Public MustInherit Partial Class Tuple
		Public Overloads Shared Function CreateTuple(Of T1, T2, T3)(ByVal item1 As T1, ByVal item2 As T2, ByVal item3 As T3) As Tuple(Of T1, T2, T3)
			If (item1 Is Nothing) OrElse ((item2 Is Nothing) OrElse (item3 Is Nothing)) Then
				Return Nothing
			End If
			Return New Tuple(Of T1, T2, T3)(item1, item2, item3)
		End Function
	End Class
	<System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA10005")> _
	<System.ComponentModel.ImmutableObjectAttribute(True)> _
	Public NotInheritable Class Tuple(Of T1, T2, T3)
		Inherits Tuple
		Implements System.IEquatable(Of Tuple(Of T1, T2, T3))
		Private ReadOnly item1 As T1
		Public ReadOnly Property Item1() As T1
			Get
				Return Me.item1
			End Get
		End Property
		Private ReadOnly item2 As T2
		Public ReadOnly Property Item2() As T2
			Get
				Return Me.item2
			End Get
		End Property
		Private ReadOnly item3 As T3
		Public ReadOnly Property Item3() As T3
			Get
				Return Me.item3
			End Get
		End Property
		Public Sub New(ByVal item1 As T1, ByVal item2 As T2, ByVal item3 As T3)
			If (item1 Is Nothing) OrElse ((item2 Is Nothing) OrElse (item3 Is Nothing)) Then
				Throw New System.ArgumentNullException()
			End If
			Me.item1 = item1
			Me.item2 = item2
			Me.item3 = item3
		End Sub
		Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
			Return Me.Equals(TryCast(obj, Tuple(Of T1, T2, T3)))
		End Function
		Public Overloads Function Equals(ByVal other As Tuple(Of T1, T2, T3)) As Boolean Implements _
			System.IEquatable(Of Tuple(Of T1, T2, T3)).Equals
			If (other Is Nothing) OrElse (Not (Me.item1.Equals(other.item1)) OrElse (Not (Me.item2.Equals(other.item2)) OrElse Not (Me.item3.Equals(other.item3)))) Then
				Return False
			End If
			Return True
		End Function
		Public Overrides Function GetHashCode() As Integer
			Return Me.item1.GetHashCode() Xor (Tuple.RotateRight(Me.item2.GetHashCode(), 1) Xor Tuple.RotateRight(Me.item3.GetHashCode(), 2))
		End Function
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Function ToString(ByVal provider As System.IFormatProvider) As String
			Return String.Format(provider, "({1}, {2}, {3})", Me.item1, Me.item2, Me.item3)
		End Function
		Public Shared Operator =(ByVal tuple1 As Tuple(Of T1, T2, T3), ByVal tuple2 As Tuple(Of T1, T2, T3)) As Boolean
			If Not (Object.ReferenceEquals(tuple1, Nothing)) Then
				Return tuple1.Equals(tuple2)
			End If
			Return Object.ReferenceEquals(tuple2, Nothing)
		End Operator
		Public Shared Operator <>(ByVal tuple1 As Tuple(Of T1, T2, T3), ByVal tuple2 As Tuple(Of T1, T2, T3)) As Boolean
			Return Not (tuple1 = tuple2)
		End Operator
	End Class
	#End Region
	#Region "4-ary Tuple"
	Public MustInherit Partial Class Tuple
		Public Overloads Shared Function CreateTuple(Of T1, T2, T3, T4)(ByVal item1 As T1, ByVal item2 As T2, ByVal item3 As T3, ByVal item4 As T4) As Tuple(Of T1, T2, T3, T4)
			If (item1 Is Nothing) OrElse ((item2 Is Nothing) OrElse ((item3 Is Nothing) OrElse (item4 Is Nothing))) Then
				Return Nothing
			End If
			Return New Tuple(Of T1, T2, T3, T4)(item1, item2, item3, item4)
		End Function
	End Class
	<System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA10005")> _
	<System.ComponentModel.ImmutableObjectAttribute(True)> _
	Public NotInheritable Class Tuple(Of T1, T2, T3, T4)
		Inherits Tuple
		Implements System.IEquatable(Of Tuple(Of T1, T2, T3, T4))
		Private ReadOnly item1 As T1
		Public ReadOnly Property Item1() As T1
			Get
				Return Me.item1
			End Get
		End Property
		Private ReadOnly item2 As T2
		Public ReadOnly Property Item2() As T2
			Get
				Return Me.item2
			End Get
		End Property
		Private ReadOnly item3 As T3
		Public ReadOnly Property Item3() As T3
			Get
				Return Me.item3
			End Get
		End Property
		Private ReadOnly item4 As T4
		Public ReadOnly Property Item4() As T4
			Get
				Return Me.item4
			End Get
		End Property
		Public Sub New(ByVal item1 As T1, ByVal item2 As T2, ByVal item3 As T3, ByVal item4 As T4)
			If (item1 Is Nothing) OrElse ((item2 Is Nothing) OrElse ((item3 Is Nothing) OrElse (item4 Is Nothing))) Then
				Throw New System.ArgumentNullException()
			End If
			Me.item1 = item1
			Me.item2 = item2
			Me.item3 = item3
			Me.item4 = item4
		End Sub
		Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
			Return Me.Equals(TryCast(obj, Tuple(Of T1, T2, T3, T4)))
		End Function
		Public Overloads Function Equals(ByVal other As Tuple(Of T1, T2, T3, T4)) As Boolean Implements _
			System.IEquatable(Of Tuple(Of T1, T2, T3, T4)).Equals
			If (other Is Nothing) OrElse (Not (Me.item1.Equals(other.item1)) OrElse (Not (Me.item2.Equals(other.item2)) OrElse (Not (Me.item3.Equals(other.item3)) OrElse Not (Me.item4.Equals(other.item4))))) Then
				Return False
			End If
			Return True
		End Function
		Public Overrides Function GetHashCode() As Integer
			Return Me.item1.GetHashCode() Xor (Tuple.RotateRight(Me.item2.GetHashCode(), 1) Xor (Tuple.RotateRight(Me.item3.GetHashCode(), 2) Xor Tuple.RotateRight(Me.item4.GetHashCode(), 3)))
		End Function
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Function ToString(ByVal provider As System.IFormatProvider) As String
			Return String.Format(provider, "({1}, {2}, {3}, {4})", Me.item1, Me.item2, Me.item3, Me.item4)
		End Function
		Public Shared Operator =(ByVal tuple1 As Tuple(Of T1, T2, T3, T4), ByVal tuple2 As Tuple(Of T1, T2, T3, T4)) As Boolean
			If Not (Object.ReferenceEquals(tuple1, Nothing)) Then
				Return tuple1.Equals(tuple2)
			End If
			Return Object.ReferenceEquals(tuple2, Nothing)
		End Operator
		Public Shared Operator <>(ByVal tuple1 As Tuple(Of T1, T2, T3, T4), ByVal tuple2 As Tuple(Of T1, T2, T3, T4)) As Boolean
			Return Not (tuple1 = tuple2)
		End Operator
	End Class
	#End Region
	#Region "5-ary Tuple"
	Public MustInherit Partial Class Tuple
		Public Overloads Shared Function CreateTuple(Of T1, T2, T3, T4, T5)(ByVal item1 As T1, ByVal item2 As T2, ByVal item3 As T3, ByVal item4 As T4, ByVal item5 As T5) As Tuple(Of T1, T2, T3, T4, T5)
			If (item1 Is Nothing) OrElse ((item2 Is Nothing) OrElse ((item3 Is Nothing) OrElse ((item4 Is Nothing) OrElse (item5 Is Nothing)))) Then
				Return Nothing
			End If
			Return New Tuple(Of T1, T2, T3, T4, T5)(item1, item2, item3, item4, item5)
		End Function
	End Class
	<System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA10005")> _
	<System.ComponentModel.ImmutableObjectAttribute(True)> _
	Public NotInheritable Class Tuple(Of T1, T2, T3, T4, T5)
		Inherits Tuple
		Implements System.IEquatable(Of Tuple(Of T1, T2, T3, T4, T5))
		Private ReadOnly item1 As T1
		Public ReadOnly Property Item1() As T1
			Get
				Return Me.item1
			End Get
		End Property
		Private ReadOnly item2 As T2
		Public ReadOnly Property Item2() As T2
			Get
				Return Me.item2
			End Get
		End Property
		Private ReadOnly item3 As T3
		Public ReadOnly Property Item3() As T3
			Get
				Return Me.item3
			End Get
		End Property
		Private ReadOnly item4 As T4
		Public ReadOnly Property Item4() As T4
			Get
				Return Me.item4
			End Get
		End Property
		Private ReadOnly item5 As T5
		Public ReadOnly Property Item5() As T5
			Get
				Return Me.item5
			End Get
		End Property
		Public Sub New(ByVal item1 As T1, ByVal item2 As T2, ByVal item3 As T3, ByVal item4 As T4, ByVal item5 As T5)
			If (item1 Is Nothing) OrElse ((item2 Is Nothing) OrElse ((item3 Is Nothing) OrElse ((item4 Is Nothing) OrElse (item5 Is Nothing)))) Then
				Throw New System.ArgumentNullException()
			End If
			Me.item1 = item1
			Me.item2 = item2
			Me.item3 = item3
			Me.item4 = item4
			Me.item5 = item5
		End Sub
		Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
			Return Me.Equals(TryCast(obj, Tuple(Of T1, T2, T3, T4, T5)))
		End Function
		Public Overloads Function Equals(ByVal other As Tuple(Of T1, T2, T3, T4, T5)) As Boolean Implements _
			System.IEquatable(Of Tuple(Of T1, T2, T3, T4, T5)).Equals
			If (other Is Nothing) OrElse (Not (Me.item1.Equals(other.item1)) OrElse (Not (Me.item2.Equals(other.item2)) OrElse (Not (Me.item3.Equals(other.item3)) OrElse (Not (Me.item4.Equals(other.item4)) OrElse Not (Me.item5.Equals(other.item5)))))) Then
				Return False
			End If
			Return True
		End Function
		Public Overrides Function GetHashCode() As Integer
			Return Me.item1.GetHashCode() Xor (Tuple.RotateRight(Me.item2.GetHashCode(), 1) Xor (Tuple.RotateRight(Me.item3.GetHashCode(), 2) Xor (Tuple.RotateRight(Me.item4.GetHashCode(), 3) Xor Tuple.RotateRight(Me.item5.GetHashCode(), 4))))
		End Function
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Function ToString(ByVal provider As System.IFormatProvider) As String
			Return String.Format(provider, "({1}, {2}, {3}, {4}, {5})", Me.item1, Me.item2, Me.item3, Me.item4, Me.item5)
		End Function
		Public Shared Operator =(ByVal tuple1 As Tuple(Of T1, T2, T3, T4, T5), ByVal tuple2 As Tuple(Of T1, T2, T3, T4, T5)) As Boolean
			If Not (Object.ReferenceEquals(tuple1, Nothing)) Then
				Return tuple1.Equals(tuple2)
			End If
			Return Object.ReferenceEquals(tuple2, Nothing)
		End Operator
		Public Shared Operator <>(ByVal tuple1 As Tuple(Of T1, T2, T3, T4, T5), ByVal tuple2 As Tuple(Of T1, T2, T3, T4, T5)) As Boolean
			Return Not (tuple1 = tuple2)
		End Operator
	End Class
	#End Region
	#Region "6-ary Tuple"
	Public MustInherit Partial Class Tuple
		Public Overloads Shared Function CreateTuple(Of T1, T2, T3, T4, T5, T6)(ByVal item1 As T1, ByVal item2 As T2, ByVal item3 As T3, ByVal item4 As T4, ByVal item5 As T5, ByVal item6 As T6) As Tuple(Of T1, T2, T3, T4, T5, T6)
			If (item1 Is Nothing) OrElse ((item2 Is Nothing) OrElse ((item3 Is Nothing) OrElse ((item4 Is Nothing) OrElse ((item5 Is Nothing) OrElse (item6 Is Nothing))))) Then
				Return Nothing
			End If
			Return New Tuple(Of T1, T2, T3, T4, T5, T6)(item1, item2, item3, item4, item5, item6)
		End Function
	End Class
	<System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA10005")> _
	<System.ComponentModel.ImmutableObjectAttribute(True)> _
	Public NotInheritable Class Tuple(Of T1, T2, T3, T4, T5, T6)
		Inherits Tuple
		Implements System.IEquatable(Of Tuple(Of T1, T2, T3, T4, T5, T6))
		Private ReadOnly item1 As T1
		Public ReadOnly Property Item1() As T1
			Get
				Return Me.item1
			End Get
		End Property
		Private ReadOnly item2 As T2
		Public ReadOnly Property Item2() As T2
			Get
				Return Me.item2
			End Get
		End Property
		Private ReadOnly item3 As T3
		Public ReadOnly Property Item3() As T3
			Get
				Return Me.item3
			End Get
		End Property
		Private ReadOnly item4 As T4
		Public ReadOnly Property Item4() As T4
			Get
				Return Me.item4
			End Get
		End Property
		Private ReadOnly item5 As T5
		Public ReadOnly Property Item5() As T5
			Get
				Return Me.item5
			End Get
		End Property
		Private ReadOnly item6 As T6
		Public ReadOnly Property Item6() As T6
			Get
				Return Me.item6
			End Get
		End Property
		Public Sub New(ByVal item1 As T1, ByVal item2 As T2, ByVal item3 As T3, ByVal item4 As T4, ByVal item5 As T5, ByVal item6 As T6)
			If (item1 Is Nothing) OrElse ((item2 Is Nothing) OrElse ((item3 Is Nothing) OrElse ((item4 Is Nothing) OrElse ((item5 Is Nothing) OrElse (item6 Is Nothing))))) Then
				Throw New System.ArgumentNullException()
			End If
			Me.item1 = item1
			Me.item2 = item2
			Me.item3 = item3
			Me.item4 = item4
			Me.item5 = item5
			Me.item6 = item6
		End Sub
		Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
			Return Me.Equals(TryCast(obj, Tuple(Of T1, T2, T3, T4, T5, T6)))
		End Function
		Public Overloads Function Equals(ByVal other As Tuple(Of T1, T2, T3, T4, T5, T6)) As Boolean Implements _
			System.IEquatable(Of Tuple(Of T1, T2, T3, T4, T5, T6)).Equals
			If (other Is Nothing) OrElse (Not (Me.item1.Equals(other.item1)) OrElse (Not (Me.item2.Equals(other.item2)) OrElse (Not (Me.item3.Equals(other.item3)) OrElse (Not (Me.item4.Equals(other.item4)) OrElse (Not (Me.item5.Equals(other.item5)) OrElse Not (Me.item6.Equals(other.item6))))))) Then
				Return False
			End If
			Return True
		End Function
		Public Overrides Function GetHashCode() As Integer
			Return Me.item1.GetHashCode() Xor (Tuple.RotateRight(Me.item2.GetHashCode(), 1) Xor (Tuple.RotateRight(Me.item3.GetHashCode(), 2) Xor (Tuple.RotateRight(Me.item4.GetHashCode(), 3) Xor (Tuple.RotateRight(Me.item5.GetHashCode(), 4) Xor Tuple.RotateRight(Me.item6.GetHashCode(), 5)))))
		End Function
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Function ToString(ByVal provider As System.IFormatProvider) As String
			Return String.Format(provider, "({1}, {2}, {3}, {4}, {5}, {6})", Me.item1, Me.item2, Me.item3, Me.item4, Me.item5, Me.item6)
		End Function
		Public Shared Operator =(ByVal tuple1 As Tuple(Of T1, T2, T3, T4, T5, T6), ByVal tuple2 As Tuple(Of T1, T2, T3, T4, T5, T6)) As Boolean
			If Not (Object.ReferenceEquals(tuple1, Nothing)) Then
				Return tuple1.Equals(tuple2)
			End If
			Return Object.ReferenceEquals(tuple2, Nothing)
		End Operator
		Public Shared Operator <>(ByVal tuple1 As Tuple(Of T1, T2, T3, T4, T5, T6), ByVal tuple2 As Tuple(Of T1, T2, T3, T4, T5, T6)) As Boolean
			Return Not (tuple1 = tuple2)
		End Operator
	End Class
	#End Region
	#Region "7-ary Tuple"
	Public MustInherit Partial Class Tuple
		Public Overloads Shared Function CreateTuple(Of T1, T2, T3, T4, T5, T6, T7)(ByVal item1 As T1, ByVal item2 As T2, ByVal item3 As T3, ByVal item4 As T4, ByVal item5 As T5, ByVal item6 As T6, ByVal item7 As T7) As Tuple(Of T1, T2, T3, T4, T5, T6, T7)
			If (item1 Is Nothing) OrElse ((item2 Is Nothing) OrElse ((item3 Is Nothing) OrElse ((item4 Is Nothing) OrElse ((item5 Is Nothing) OrElse ((item6 Is Nothing) OrElse (item7 Is Nothing)))))) Then
				Return Nothing
			End If
			Return New Tuple(Of T1, T2, T3, T4, T5, T6, T7)(item1, item2, item3, item4, item5, item6, item7)
		End Function
	End Class
	<System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA10005")> _
	<System.ComponentModel.ImmutableObjectAttribute(True)> _
	Public NotInheritable Class Tuple(Of T1, T2, T3, T4, T5, T6, T7)
		Inherits Tuple
		Implements System.IEquatable(Of Tuple(Of T1, T2, T3, T4, T5, T6, T7))
		Private ReadOnly item1 As T1
		Public ReadOnly Property Item1() As T1
			Get
				Return Me.item1
			End Get
		End Property
		Private ReadOnly item2 As T2
		Public ReadOnly Property Item2() As T2
			Get
				Return Me.item2
			End Get
		End Property
		Private ReadOnly item3 As T3
		Public ReadOnly Property Item3() As T3
			Get
				Return Me.item3
			End Get
		End Property
		Private ReadOnly item4 As T4
		Public ReadOnly Property Item4() As T4
			Get
				Return Me.item4
			End Get
		End Property
		Private ReadOnly item5 As T5
		Public ReadOnly Property Item5() As T5
			Get
				Return Me.item5
			End Get
		End Property
		Private ReadOnly item6 As T6
		Public ReadOnly Property Item6() As T6
			Get
				Return Me.item6
			End Get
		End Property
		Private ReadOnly item7 As T7
		Public ReadOnly Property Item7() As T7
			Get
				Return Me.item7
			End Get
		End Property
		Public Sub New(ByVal item1 As T1, ByVal item2 As T2, ByVal item3 As T3, ByVal item4 As T4, ByVal item5 As T5, ByVal item6 As T6, ByVal item7 As T7)
			If (item1 Is Nothing) OrElse ((item2 Is Nothing) OrElse ((item3 Is Nothing) OrElse ((item4 Is Nothing) OrElse ((item5 Is Nothing) OrElse ((item6 Is Nothing) OrElse (item7 Is Nothing)))))) Then
				Throw New System.ArgumentNullException()
			End If
			Me.item1 = item1
			Me.item2 = item2
			Me.item3 = item3
			Me.item4 = item4
			Me.item5 = item5
			Me.item6 = item6
			Me.item7 = item7
		End Sub
		Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
			Return Me.Equals(TryCast(obj, Tuple(Of T1, T2, T3, T4, T5, T6, T7)))
		End Function
		Public Overloads Function Equals(ByVal other As Tuple(Of T1, T2, T3, T4, T5, T6, T7)) As Boolean Implements _
			System.IEquatable(Of Tuple(Of T1, T2, T3, T4, T5, T6, T7)).Equals
			If (other Is Nothing) OrElse (Not (Me.item1.Equals(other.item1)) OrElse (Not (Me.item2.Equals(other.item2)) OrElse (Not (Me.item3.Equals(other.item3)) OrElse (Not (Me.item4.Equals(other.item4)) OrElse (Not (Me.item5.Equals(other.item5)) OrElse (Not (Me.item6.Equals(other.item6)) OrElse Not (Me.item7.Equals(other.item7)))))))) Then
				Return False
			End If
			Return True
		End Function
		Public Overrides Function GetHashCode() As Integer
			Return Me.item1.GetHashCode() Xor (Tuple.RotateRight(Me.item2.GetHashCode(), 1) Xor (Tuple.RotateRight(Me.item3.GetHashCode(), 2) Xor (Tuple.RotateRight(Me.item4.GetHashCode(), 3) Xor (Tuple.RotateRight(Me.item5.GetHashCode(), 4) Xor (Tuple.RotateRight(Me.item6.GetHashCode(), 5) Xor Tuple.RotateRight(Me.item7.GetHashCode(), 6))))))
		End Function
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Function ToString(ByVal provider As System.IFormatProvider) As String
			Return String.Format(provider, "({1}, {2}, {3}, {4}, {5}, {6}, {7})", Me.item1, Me.item2, Me.item3, Me.item4, Me.item5, Me.item6, Me.item7)
		End Function
		Public Shared Operator =(ByVal tuple1 As Tuple(Of T1, T2, T3, T4, T5, T6, T7), ByVal tuple2 As Tuple(Of T1, T2, T3, T4, T5, T6, T7)) As Boolean
			If Not (Object.ReferenceEquals(tuple1, Nothing)) Then
				Return tuple1.Equals(tuple2)
			End If
			Return Object.ReferenceEquals(tuple2, Nothing)
		End Operator
		Public Shared Operator <>(ByVal tuple1 As Tuple(Of T1, T2, T3, T4, T5, T6, T7), ByVal tuple2 As Tuple(Of T1, T2, T3, T4, T5, T6, T7)) As Boolean
			Return Not (tuple1 = tuple2)
		End Operator
	End Class
	#End Region
	#Region "8-ary Tuple"
	Public MustInherit Partial Class Tuple
		Public Overloads Shared Function CreateTuple(Of T1, T2, T3, T4, T5, T6, T7, T8)(ByVal item1 As T1, ByVal item2 As T2, ByVal item3 As T3, ByVal item4 As T4, ByVal item5 As T5, ByVal item6 As T6, ByVal item7 As T7, ByVal item8 As T8) As Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8)
			If (item1 Is Nothing) OrElse ((item2 Is Nothing) OrElse ((item3 Is Nothing) OrElse ((item4 Is Nothing) OrElse ((item5 Is Nothing) OrElse ((item6 Is Nothing) OrElse ((item7 Is Nothing) OrElse (item8 Is Nothing))))))) Then
				Return Nothing
			End If
			Return New Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8)(item1, item2, item3, item4, item5, item6, item7, item8)
		End Function
	End Class
	<System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA10005")> _
	<System.ComponentModel.ImmutableObjectAttribute(True)> _
	Public NotInheritable Class Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8)
		Inherits Tuple
		Implements System.IEquatable(Of Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8))
		Private ReadOnly item1 As T1
		Public ReadOnly Property Item1() As T1
			Get
				Return Me.item1
			End Get
		End Property
		Private ReadOnly item2 As T2
		Public ReadOnly Property Item2() As T2
			Get
				Return Me.item2
			End Get
		End Property
		Private ReadOnly item3 As T3
		Public ReadOnly Property Item3() As T3
			Get
				Return Me.item3
			End Get
		End Property
		Private ReadOnly item4 As T4
		Public ReadOnly Property Item4() As T4
			Get
				Return Me.item4
			End Get
		End Property
		Private ReadOnly item5 As T5
		Public ReadOnly Property Item5() As T5
			Get
				Return Me.item5
			End Get
		End Property
		Private ReadOnly item6 As T6
		Public ReadOnly Property Item6() As T6
			Get
				Return Me.item6
			End Get
		End Property
		Private ReadOnly item7 As T7
		Public ReadOnly Property Item7() As T7
			Get
				Return Me.item7
			End Get
		End Property
		Private ReadOnly item8 As T8
		Public ReadOnly Property Item8() As T8
			Get
				Return Me.item8
			End Get
		End Property
		Public Sub New(ByVal item1 As T1, ByVal item2 As T2, ByVal item3 As T3, ByVal item4 As T4, ByVal item5 As T5, ByVal item6 As T6, ByVal item7 As T7, ByVal item8 As T8)
			If (item1 Is Nothing) OrElse ((item2 Is Nothing) OrElse ((item3 Is Nothing) OrElse ((item4 Is Nothing) OrElse ((item5 Is Nothing) OrElse ((item6 Is Nothing) OrElse ((item7 Is Nothing) OrElse (item8 Is Nothing))))))) Then
				Throw New System.ArgumentNullException()
			End If
			Me.item1 = item1
			Me.item2 = item2
			Me.item3 = item3
			Me.item4 = item4
			Me.item5 = item5
			Me.item6 = item6
			Me.item7 = item7
			Me.item8 = item8
		End Sub
		Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
			Return Me.Equals(TryCast(obj, Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8)))
		End Function
		Public Overloads Function Equals(ByVal other As Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8)) As Boolean Implements _
			System.IEquatable(Of Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8)).Equals
			If (other Is Nothing) OrElse (Not (Me.item1.Equals(other.item1)) OrElse (Not (Me.item2.Equals(other.item2)) OrElse (Not (Me.item3.Equals(other.item3)) OrElse (Not (Me.item4.Equals(other.item4)) OrElse (Not (Me.item5.Equals(other.item5)) OrElse (Not (Me.item6.Equals(other.item6)) OrElse (Not (Me.item7.Equals(other.item7)) OrElse Not (Me.item8.Equals(other.item8))))))))) Then
				Return False
			End If
			Return True
		End Function
		Public Overrides Function GetHashCode() As Integer
			Return Me.item1.GetHashCode() Xor (Tuple.RotateRight(Me.item2.GetHashCode(), 1) Xor (Tuple.RotateRight(Me.item3.GetHashCode(), 2) Xor (Tuple.RotateRight(Me.item4.GetHashCode(), 3) Xor (Tuple.RotateRight(Me.item5.GetHashCode(), 4) Xor (Tuple.RotateRight(Me.item6.GetHashCode(), 5) Xor (Tuple.RotateRight(Me.item7.GetHashCode(), 6) Xor Tuple.RotateRight(Me.item8.GetHashCode(), 7)))))))
		End Function
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Function ToString(ByVal provider As System.IFormatProvider) As String
			Return String.Format(provider, "({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8})", Me.item1, Me.item2, Me.item3, Me.item4, Me.item5, Me.item6, Me.item7, Me.item8)
		End Function
		Public Shared Operator =(ByVal tuple1 As Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8), ByVal tuple2 As Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8)) As Boolean
			If Not (Object.ReferenceEquals(tuple1, Nothing)) Then
				Return tuple1.Equals(tuple2)
			End If
			Return Object.ReferenceEquals(tuple2, Nothing)
		End Operator
		Public Shared Operator <>(ByVal tuple1 As Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8), ByVal tuple2 As Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8)) As Boolean
			Return Not (tuple1 = tuple2)
		End Operator
	End Class
	#End Region
	#Region "9-ary Tuple"
	Public MustInherit Partial Class Tuple
		Public Overloads Shared Function CreateTuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)(ByVal item1 As T1, ByVal item2 As T2, ByVal item3 As T3, ByVal item4 As T4, ByVal item5 As T5, ByVal item6 As T6, ByVal item7 As T7, ByVal item8 As T8, ByVal item9 As T9) As Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)
			If (item1 Is Nothing) OrElse ((item2 Is Nothing) OrElse ((item3 Is Nothing) OrElse ((item4 Is Nothing) OrElse ((item5 Is Nothing) OrElse ((item6 Is Nothing) OrElse ((item7 Is Nothing) OrElse ((item8 Is Nothing) OrElse (item9 Is Nothing)))))))) Then
				Return Nothing
			End If
			Return New Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)(item1, item2, item3, item4, item5, item6, item7, item8, item9)
		End Function
	End Class
	<System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA10005")> _
	<System.ComponentModel.ImmutableObjectAttribute(True)> _
	Public NotInheritable Class Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)
		Inherits Tuple
		Implements System.IEquatable(Of Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9))
		Private ReadOnly item1 As T1
		Public ReadOnly Property Item1() As T1
			Get
				Return Me.item1
			End Get
		End Property
		Private ReadOnly item2 As T2
		Public ReadOnly Property Item2() As T2
			Get
				Return Me.item2
			End Get
		End Property
		Private ReadOnly item3 As T3
		Public ReadOnly Property Item3() As T3
			Get
				Return Me.item3
			End Get
		End Property
		Private ReadOnly item4 As T4
		Public ReadOnly Property Item4() As T4
			Get
				Return Me.item4
			End Get
		End Property
		Private ReadOnly item5 As T5
		Public ReadOnly Property Item5() As T5
			Get
				Return Me.item5
			End Get
		End Property
		Private ReadOnly item6 As T6
		Public ReadOnly Property Item6() As T6
			Get
				Return Me.item6
			End Get
		End Property
		Private ReadOnly item7 As T7
		Public ReadOnly Property Item7() As T7
			Get
				Return Me.item7
			End Get
		End Property
		Private ReadOnly item8 As T8
		Public ReadOnly Property Item8() As T8
			Get
				Return Me.item8
			End Get
		End Property
		Private ReadOnly item9 As T9
		Public ReadOnly Property Item9() As T9
			Get
				Return Me.item9
			End Get
		End Property
		Public Sub New(ByVal item1 As T1, ByVal item2 As T2, ByVal item3 As T3, ByVal item4 As T4, ByVal item5 As T5, ByVal item6 As T6, ByVal item7 As T7, ByVal item8 As T8, ByVal item9 As T9)
			If (item1 Is Nothing) OrElse ((item2 Is Nothing) OrElse ((item3 Is Nothing) OrElse ((item4 Is Nothing) OrElse ((item5 Is Nothing) OrElse ((item6 Is Nothing) OrElse ((item7 Is Nothing) OrElse ((item8 Is Nothing) OrElse (item9 Is Nothing)))))))) Then
				Throw New System.ArgumentNullException()
			End If
			Me.item1 = item1
			Me.item2 = item2
			Me.item3 = item3
			Me.item4 = item4
			Me.item5 = item5
			Me.item6 = item6
			Me.item7 = item7
			Me.item8 = item8
			Me.item9 = item9
		End Sub
		Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
			Return Me.Equals(TryCast(obj, Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)))
		End Function
		Public Overloads Function Equals(ByVal other As Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)) As Boolean Implements _
			System.IEquatable(Of Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)).Equals
			If (other Is Nothing) OrElse (Not (Me.item1.Equals(other.item1)) OrElse (Not (Me.item2.Equals(other.item2)) OrElse (Not (Me.item3.Equals(other.item3)) OrElse (Not (Me.item4.Equals(other.item4)) OrElse (Not (Me.item5.Equals(other.item5)) OrElse (Not (Me.item6.Equals(other.item6)) OrElse (Not (Me.item7.Equals(other.item7)) OrElse (Not (Me.item8.Equals(other.item8)) OrElse Not (Me.item9.Equals(other.item9)))))))))) Then
				Return False
			End If
			Return True
		End Function
		Public Overrides Function GetHashCode() As Integer
			Return Me.item1.GetHashCode() Xor (Tuple.RotateRight(Me.item2.GetHashCode(), 1) Xor (Tuple.RotateRight(Me.item3.GetHashCode(), 2) Xor (Tuple.RotateRight(Me.item4.GetHashCode(), 3) Xor (Tuple.RotateRight(Me.item5.GetHashCode(), 4) Xor (Tuple.RotateRight(Me.item6.GetHashCode(), 5) Xor (Tuple.RotateRight(Me.item7.GetHashCode(), 6) Xor (Tuple.RotateRight(Me.item8.GetHashCode(), 7) Xor Tuple.RotateRight(Me.item9.GetHashCode(), 8))))))))
		End Function
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Function ToString(ByVal provider As System.IFormatProvider) As String
			Return String.Format(provider, "({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9})", Me.item1, Me.item2, Me.item3, Me.item4, Me.item5, Me.item6, Me.item7, Me.item8, Me.item9)
		End Function
		Public Shared Operator =(ByVal tuple1 As Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9), ByVal tuple2 As Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)) As Boolean
			If Not (Object.ReferenceEquals(tuple1, Nothing)) Then
				Return tuple1.Equals(tuple2)
			End If
			Return Object.ReferenceEquals(tuple2, Nothing)
		End Operator
		Public Shared Operator <>(ByVal tuple1 As Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9), ByVal tuple2 As Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)) As Boolean
			Return Not (tuple1 = tuple2)
		End Operator
	End Class
	#End Region
	#Region "10-ary Tuple"
	Public MustInherit Partial Class Tuple
		Public Overloads Shared Function CreateTuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)(ByVal item1 As T1, ByVal item2 As T2, ByVal item3 As T3, ByVal item4 As T4, ByVal item5 As T5, ByVal item6 As T6, ByVal item7 As T7, ByVal item8 As T8, ByVal item9 As T9, ByVal item10 As T10) As Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)
			If (item1 Is Nothing) OrElse ((item2 Is Nothing) OrElse ((item3 Is Nothing) OrElse ((item4 Is Nothing) OrElse ((item5 Is Nothing) OrElse ((item6 Is Nothing) OrElse ((item7 Is Nothing) OrElse ((item8 Is Nothing) OrElse ((item9 Is Nothing) OrElse (item10 Is Nothing))))))))) Then
				Return Nothing
			End If
			Return New Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)(item1, item2, item3, item4, item5, item6, item7, item8, item9, item10)
		End Function
	End Class
	<System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA10005")> _
	<System.ComponentModel.ImmutableObjectAttribute(True)> _
	Public NotInheritable Class Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)
		Inherits Tuple
		Implements System.IEquatable(Of Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10))
		Private ReadOnly item1 As T1
		Public ReadOnly Property Item1() As T1
			Get
				Return Me.item1
			End Get
		End Property
		Private ReadOnly item2 As T2
		Public ReadOnly Property Item2() As T2
			Get
				Return Me.item2
			End Get
		End Property
		Private ReadOnly item3 As T3
		Public ReadOnly Property Item3() As T3
			Get
				Return Me.item3
			End Get
		End Property
		Private ReadOnly item4 As T4
		Public ReadOnly Property Item4() As T4
			Get
				Return Me.item4
			End Get
		End Property
		Private ReadOnly item5 As T5
		Public ReadOnly Property Item5() As T5
			Get
				Return Me.item5
			End Get
		End Property
		Private ReadOnly item6 As T6
		Public ReadOnly Property Item6() As T6
			Get
				Return Me.item6
			End Get
		End Property
		Private ReadOnly item7 As T7
		Public ReadOnly Property Item7() As T7
			Get
				Return Me.item7
			End Get
		End Property
		Private ReadOnly item8 As T8
		Public ReadOnly Property Item8() As T8
			Get
				Return Me.item8
			End Get
		End Property
		Private ReadOnly item9 As T9
		Public ReadOnly Property Item9() As T9
			Get
				Return Me.item9
			End Get
		End Property
		Private ReadOnly item10 As T10
		Public ReadOnly Property Item10() As T10
			Get
				Return Me.item10
			End Get
		End Property
		Public Sub New(ByVal item1 As T1, ByVal item2 As T2, ByVal item3 As T3, ByVal item4 As T4, ByVal item5 As T5, ByVal item6 As T6, ByVal item7 As T7, ByVal item8 As T8, ByVal item9 As T9, ByVal item10 As T10)
			If (item1 Is Nothing) OrElse ((item2 Is Nothing) OrElse ((item3 Is Nothing) OrElse ((item4 Is Nothing) OrElse ((item5 Is Nothing) OrElse ((item6 Is Nothing) OrElse ((item7 Is Nothing) OrElse ((item8 Is Nothing) OrElse ((item9 Is Nothing) OrElse (item10 Is Nothing))))))))) Then
				Throw New System.ArgumentNullException()
			End If
			Me.item1 = item1
			Me.item2 = item2
			Me.item3 = item3
			Me.item4 = item4
			Me.item5 = item5
			Me.item6 = item6
			Me.item7 = item7
			Me.item8 = item8
			Me.item9 = item9
			Me.item10 = item10
		End Sub
		Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
			Return Me.Equals(TryCast(obj, Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)))
		End Function
		Public Overloads Function Equals(ByVal other As Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)) As Boolean Implements _
			System.IEquatable(Of Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)).Equals
			If (other Is Nothing) OrElse (Not (Me.item1.Equals(other.item1)) OrElse (Not (Me.item2.Equals(other.item2)) OrElse (Not (Me.item3.Equals(other.item3)) OrElse (Not (Me.item4.Equals(other.item4)) OrElse (Not (Me.item5.Equals(other.item5)) OrElse (Not (Me.item6.Equals(other.item6)) OrElse (Not (Me.item7.Equals(other.item7)) OrElse (Not (Me.item8.Equals(other.item8)) OrElse (Not (Me.item9.Equals(other.item9)) OrElse Not (Me.item10.Equals(other.item10))))))))))) Then
				Return False
			End If
			Return True
		End Function
		Public Overrides Function GetHashCode() As Integer
			Return Me.item1.GetHashCode() Xor (Tuple.RotateRight(Me.item2.GetHashCode(), 1) Xor (Tuple.RotateRight(Me.item3.GetHashCode(), 2) Xor (Tuple.RotateRight(Me.item4.GetHashCode(), 3) Xor (Tuple.RotateRight(Me.item5.GetHashCode(), 4) Xor (Tuple.RotateRight(Me.item6.GetHashCode(), 5) Xor (Tuple.RotateRight(Me.item7.GetHashCode(), 6) Xor (Tuple.RotateRight(Me.item8.GetHashCode(), 7) Xor (Tuple.RotateRight(Me.item9.GetHashCode(), 8) Xor Tuple.RotateRight(Me.item10.GetHashCode(), 9)))))))))
		End Function
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Function ToString(ByVal provider As System.IFormatProvider) As String
			Return String.Format(provider, "({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10})", Me.item1, Me.item2, Me.item3, Me.item4, Me.item5, Me.item6, Me.item7, Me.item8, Me.item9, Me.item10)
		End Function
		Public Shared Operator =(ByVal tuple1 As Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10), ByVal tuple2 As Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)) As Boolean
			If Not (Object.ReferenceEquals(tuple1, Nothing)) Then
				Return tuple1.Equals(tuple2)
			End If
			Return Object.ReferenceEquals(tuple2, Nothing)
		End Operator
		Public Shared Operator <>(ByVal tuple1 As Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10), ByVal tuple2 As Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)) As Boolean
			Return Not (tuple1 = tuple2)
		End Operator
	End Class
	#End Region
	#Region "Property Change Event Support"
	Public Interface IPropertyChangeEventArgs(Of TProperty)
		ReadOnly Property OldValue() As TProperty
		ReadOnly Property NewValue() As TProperty
	End Interface
	Public NotInheritable Class PropertyChangingEventArgs(Of TProperty)
		Inherits CancelEventArgs
		Implements IPropertyChangeEventArgs(Of TProperty)
		Private ReadOnly oldValue As TProperty
		Private ReadOnly newValue As TProperty
		Public Sub New(ByVal oldValue As TProperty, ByVal newValue As TProperty)
			Me.oldValue = oldValue
			Me.newValue = newValue
		End Sub
		Public ReadOnly Property OldValue() As TProperty Implements _
			IPropertyChangeEventArgs.OldValue
			Get
				Return Me.oldValue
			End Get
		End Property
		Public ReadOnly Property NewValue() As TProperty Implements _
			IPropertyChangeEventArgs.NewValue
			Get
				Return Me.newValue
			End Get
		End Property
	End Class
	Public NotInheritable Class PropertyChangedEventArgs(Of TProperty)
		Inherits EventArgs
		Implements IPropertyChangeEventArgs(Of TProperty)
		Private ReadOnly oldValue As TProperty
		Private ReadOnly newValue As TProperty
		Public Sub New(ByVal oldValue As TProperty, ByVal newValue As TProperty)
			Me.oldValue = oldValue
			Me.newValue = newValue
		End Sub
		Public ReadOnly Property OldValue() As TProperty Implements _
			IPropertyChangeEventArgs.OldValue
			Get
				Return Me.oldValue
			End Get
		End Property
		Public ReadOnly Property NewValue() As TProperty Implements _
			IPropertyChangeEventArgs.NewValue
			Get
				Return Me.newValue
			End Get
		End Property
	End Class
	#End Region
End Namespace
#End Region
Namespace PersonCountryDemo
	#Region "Person"
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	Public MustInherit Partial Class Person
		Implements INotifyPropertyChanged
		Protected Sub New()
		End Sub
		Private ReadOnly Events As System.Delegate() = New System.Delegate(5) {}
		<SuppressMessageAttribute("Microsoft.Design", "CA1033")> _
		Private Custom Event PropertyChanged As PropertyChangedEventHandler Implements _
			INotifyPropertyChanged.PropertyChanged
			AddHandler(ByVal Value As PropertyChangedEventHandler)
				Me.Events(0) = System.Delegate.Combine(Me.Events(0), Value)
			End AddHandler
			RemoveHandler(ByVal Value As PropertyChangedEventHandler)
				Me.Events(0) = System.Delegate.Remove(Me.Events(0), Value)
			End RemoveHandler
		End Event
		Private Sub RaisePropertyChangedEvent(ByVal propertyName As String)
			Dim eventHandler As PropertyChangedEventHandler = TryCast(Me.Events(0), PropertyChangedEventHandler)
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(propertyName), New AsyncCallback(eventHandler.EndInvoke), Nothing)
			End If
		End Sub
		Public MustOverride ReadOnly Property Context() As PersonCountryDemoContext
		Public Custom Event LastNameChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseLastNameChangingEvent(ByVal newValue As String) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of String)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangingEventArgs(Of String)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of String) = New PropertyChangingEventArgs(Of String)(Me.LastName, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event LastNameChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseLastNameChangedEvent(ByVal oldValue As String)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of String)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangedEventArgs(Of String)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of String)(oldValue, Me.LastName), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("LastName")
			End If
		End Sub
		Public Custom Event FirstNameChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseFirstNameChangingEvent(ByVal newValue As String) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of String)) = TryCast(Me.Events(2), EventHandler(Of PropertyChangingEventArgs(Of String)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of String) = New PropertyChangingEventArgs(Of String)(Me.FirstName, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event FirstNameChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseFirstNameChangedEvent(ByVal oldValue As String)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of String)) = TryCast(Me.Events(2), EventHandler(Of PropertyChangedEventArgs(Of String)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of String)(oldValue, Me.FirstName), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("FirstName")
			End If
		End Sub
		Public Custom Event TitleChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Combine(Me.Events(3), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Remove(Me.Events(3), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseTitleChangingEvent(ByVal newValue As String) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of String)) = TryCast(Me.Events(3), EventHandler(Of PropertyChangingEventArgs(Of String)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of String) = New PropertyChangingEventArgs(Of String)(Me.Title, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event TitleChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Combine(Me.Events(3), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Remove(Me.Events(3), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseTitleChangedEvent(ByVal oldValue As String)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of String)) = TryCast(Me.Events(3), EventHandler(Of PropertyChangedEventArgs(Of String)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of String)(oldValue, Me.Title), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Title")
			End If
		End Sub
		Public Custom Event CountryChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(4) = System.Delegate.Combine(Me.Events(4), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(4) = System.Delegate.Remove(Me.Events(4), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseCountryChangingEvent(ByVal newValue As Country) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Country)) = TryCast(Me.Events(4), EventHandler(Of PropertyChangingEventArgs(Of Country)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Country) = New PropertyChangingEventArgs(Of Country)(Me.Country, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event CountryChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(4) = System.Delegate.Combine(Me.Events(4), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(4) = System.Delegate.Remove(Me.Events(4), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseCountryChangedEvent(ByVal oldValue As Country)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Country)) = TryCast(Me.Events(4), EventHandler(Of PropertyChangedEventArgs(Of Country)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Country)(oldValue, Me.Country), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Country")
			End If
		End Sub
		Public MustOverride Property LastName() As String
		Public MustOverride Property FirstName() As String
		Public MustOverride Property Title() As String
		Public MustOverride Property Country() As Country
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Overridable Function ToString(ByVal provider As IFormatProvider) As String
			Return String.Format(provider, "Person{0}{{{0}{1}LastName = ""{2}"",{0}{1}FirstName = ""{3}"",{0}{1}Title = ""{4}"",{0}{1}Country = {5}{0}}}", Environment.NewLine, "", Me.LastName, Me.FirstName, Me.Title, "TODO: Recursively call ToString for customTypes...")
		End Function
	End Class
	#End Region
	#Region "Country"
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	Public MustInherit Partial Class Country
		Implements INotifyPropertyChanged
		Protected Sub New()
		End Sub
		Private ReadOnly Events As System.Delegate() = New System.Delegate(3) {}
		<SuppressMessageAttribute("Microsoft.Design", "CA1033")> _
		Private Custom Event PropertyChanged As PropertyChangedEventHandler Implements _
			INotifyPropertyChanged.PropertyChanged
			AddHandler(ByVal Value As PropertyChangedEventHandler)
				Me.Events(0) = System.Delegate.Combine(Me.Events(0), Value)
			End AddHandler
			RemoveHandler(ByVal Value As PropertyChangedEventHandler)
				Me.Events(0) = System.Delegate.Remove(Me.Events(0), Value)
			End RemoveHandler
		End Event
		Private Sub RaisePropertyChangedEvent(ByVal propertyName As String)
			Dim eventHandler As PropertyChangedEventHandler = TryCast(Me.Events(0), PropertyChangedEventHandler)
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(propertyName), New AsyncCallback(eventHandler.EndInvoke), Nothing)
			End If
		End Sub
		Public MustOverride ReadOnly Property Context() As PersonCountryDemoContext
		Public Custom Event Country_nameChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseCountry_nameChangingEvent(ByVal newValue As String) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of String)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangingEventArgs(Of String)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of String) = New PropertyChangingEventArgs(Of String)(Me.Country_name, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event Country_nameChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseCountry_nameChangedEvent(ByVal oldValue As String)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of String)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangedEventArgs(Of String)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of String)(oldValue, Me.Country_name), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Country_name")
			End If
		End Sub
		Public Custom Event Region_Region_codeChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseRegion_Region_codeChangingEvent(ByVal newValue As String) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of String)) = TryCast(Me.Events(2), EventHandler(Of PropertyChangingEventArgs(Of String)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of String) = New PropertyChangingEventArgs(Of String)(Me.Region_Region_code, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event Region_Region_codeChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseRegion_Region_codeChangedEvent(ByVal oldValue As String)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of String)) = TryCast(Me.Events(2), EventHandler(Of PropertyChangedEventArgs(Of String)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of String)(oldValue, Me.Region_Region_code), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Region_Region_code")
			End If
		End Sub
		Public MustOverride Property Country_name() As String
		Public MustOverride Property Region_Region_code() As String
		Public MustOverride ReadOnly Property Person() As ICollection(Of Person)
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Overridable Function ToString(ByVal provider As IFormatProvider) As String
			Return String.Format(provider, "Country{0}{{{0}{1}Country_name = ""{2}"",{0}{1}Region_Region_code = ""{3}""{0}}}", Environment.NewLine, "", Me.Country_name, Me.Region_Region_code)
		End Function
	End Class
	#End Region
	#Region "IPersonCountryDemoContext"
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	Public Interface IPersonCountryDemoContext
		ReadOnly Property IsDeserializing() As Boolean
		Function GetCountryByCountry_name(ByVal Country_name As String) As Country
		Function CreatePerson(ByVal LastName As String, ByVal FirstName As String) As Person
		ReadOnly Property PersonCollection() As ReadOnlyCollection(Of Person)
		Function CreateCountry(ByVal Country_name As String) As Country
		ReadOnly Property CountryCollection() As ReadOnlyCollection(Of Country)
	End Interface
	#End Region
	#Region "PersonCountryDemoContext"
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	Public NotInheritable Class PersonCountryDemoContext
		Implements IPersonCountryDemoContext
		Public Sub New()
			Dim PersonList As List(Of Person) = New List(Of Person)()
			Me.myPersonList = PersonList
			Me.myPersonReadOnlyCollection = New ReadOnlyCollection(Of Person)(PersonList)
			Dim CountryList As List(Of Country) = New List(Of Country)()
			Me.myCountryList = CountryList
			Me.myCountryReadOnlyCollection = New ReadOnlyCollection(Of Country)(CountryList)
		End Sub
		#Region "ConstraintEnforcementCollection"
		Private Delegate Function PotentialCollectionModificationCallback(Of TClass As Class, TProperty)(ByVal instance As TClass, ByVal value As TProperty) As Boolean
		Private Delegate Sub CommittedCollectionModificationCallback(Of TClass As Class, TProperty)(ByVal instance As TClass, ByVal value As TProperty)
		Private NotInheritable Class ConstraintEnforcementCollection(Of TClass As Class, TProperty)
			Implements ICollection(Of TProperty)
			Private ReadOnly myInstance As TClass
			Private ReadOnly myList As List(Of TProperty) = New List(Of TProperty)()
			Public Sub New(ByVal instance As TClass, ByVal adding As PotentialCollectionModificationCallback(Of TClass, TProperty), ByVal added As CommittedCollectionModificationCallback(Of TClass, TProperty), ByVal removing As PotentialCollectionModificationCallback(Of TClass, TProperty), ByVal removed As CommittedCollectionModificationCallback(Of TClass, TProperty))
				If instance Is Nothing Then
					Throw New ArgumentNullException("instance")
				End If
				Me.myInstance = instance
				Me.myAdding = adding
				Me.myAdded = added
				Me.myRemoving = removing
				Me.myRemoved = removed
			End Sub
			Private ReadOnly myAdding As PotentialCollectionModificationCallback(Of TClass, TProperty)
			Private ReadOnly myAdded As CommittedCollectionModificationCallback(Of TClass, TProperty)
			Private ReadOnly myRemoving As PotentialCollectionModificationCallback(Of TClass, TProperty)
			Private ReadOnly myRemoved As CommittedCollectionModificationCallback(Of TClass, TProperty)
			Private Function OnAdding(ByVal value As TProperty) As Boolean
				If Me.myAdding IsNot Nothing Then
					Return Me.myAdding(Me.myInstance, value)
				End If
				Return True
			End Function
			Private Sub OnAdded(ByVal value As TProperty)
				If Me.myAdded IsNot Nothing Then
					Me.myAdded(Me.myInstance, value)
				End If
			End Sub
			Private Function OnRemoving(ByVal value As TProperty) As Boolean
				If Me.myRemoving IsNot Nothing Then
					Return Me.myRemoving(Me.myInstance, value)
				End If
				Return True
			End Function
			Private Sub OnRemoved(ByVal value As TProperty)
				If Me.myRemoved IsNot Nothing Then
					Me.myRemoved(Me.myInstance, value)
				End If
			End Sub
			Private Function GetEnumerator() As System.Collections.IEnumerator Implements _
				System.Collections.IEnumerable.GetEnumerator
				Return Me.GetEnumerator()
			End Function
			Public Function GetEnumerator() As IEnumerator(Of TProperty) Implements _
				IEnumerable(Of TProperty).GetEnumerator
				Return Me.myList.GetEnumerator()
			End Function
			Public Sub Add(ByVal item As TProperty) Implements _
				ICollection(Of TProperty).Add
				If Me.OnAdding(item) Then
					Me.myList.Add(item)
					Me.OnAdded(item)
				End If
			End Sub
			Public Function Remove(ByVal item As TProperty) As Boolean Implements _
				ICollection(Of TProperty).Remove
				If Me.OnRemoving(item) Then
					If Me.myList.Remove(item) Then
						Me.OnRemoved(item)
						Return True
					End If
				End If
				Return False
			End Function
			Public Sub Clear() Implements _
				ICollection(Of TProperty).Clear
				Dim i As Integer = 0
				While i < Me.myList.Count
					Me.Remove(Me.myList(i))
					i = i + 1
				End While
			End Sub
			Public Function Contains(ByVal item As TProperty) As Boolean Implements _
				ICollection(Of TProperty).Contains
				Return Me.myList.Contains(item)
			End Function
			Public Sub CopyTo(ByVal array As TProperty(), ByVal arrayIndex As Integer) Implements _
				ICollection(Of TProperty).CopyTo
				Me.myList.CopyTo(array, arrayIndex)
			End Sub
			Public ReadOnly Property Count() As Integer Implements _
				ICollection(Of TProperty).Count
				Get
					Return Me.myList.Count
				End Get
			End Property
			Public ReadOnly Property IsReadOnly() As Boolean Implements _
				ICollection(Of TProperty).IsReadOnly
				Get
					Return False
				End Get
			End Property
		End Class
		#End Region
		Private myIsDeserializing As Boolean
		Public ReadOnly Property IsDeserializing() As Boolean Implements _
			IPersonCountryDemoContext.IsDeserializing
			Get
				Return Me.myIsDeserializing
			End Get
		End Property
		Private ReadOnly myCountryCountry_nameDictionary As Dictionary(Of String, Country) = New Dictionary(Of String, Country)()
		Public Function GetCountryByCountry_name(ByVal Country_name As String) As Country Implements _
			IPersonCountryDemoContext.GetCountryByCountry_name
			Return Me.myCountryCountry_nameDictionary(Country_name)
		End Function
		Private Function OnPersonLastNameChanging(ByVal instance As Person, ByVal newValue As String) As Boolean
			Return True
		End Function
		Private Function OnPersonFirstNameChanging(ByVal instance As Person, ByVal newValue As String) As Boolean
			Return True
		End Function
		Private Function OnPersonTitleChanging(ByVal instance As Person, ByVal newValue As String) As Boolean
			Return True
		End Function
		<SuppressMessageAttribute("Microsoft.Usage", "CA2208")> _
		<SuppressMessageAttribute("Microsoft.Globalization", "CA1303")> _
		Private Function OnPersonCountryChanging(ByVal instance As Person, ByVal newValue As Country) As Boolean
			If newValue IsNot Nothing Then
				If Me IsNot newValue.Context Then
					Throw New ArgumentException("All objects in a relationship must be part of the same Context.", "value")
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnPersonCountryChanged(ByVal instance As Person, ByVal oldValue As Country)
			If instance.Country IsNot Nothing Then
				instance.Country.Person.Add(instance)
			End If
			If oldValue IsNot Nothing Then
				oldValue.Person.Remove(instance)
			Else
			End If
		End Sub
		Public Function CreatePerson(ByVal LastName As String, ByVal FirstName As String) As Person
			If Not (Me.IsDeserializing) Then
				If Not (Me.OnPersonLastNameChanging(Nothing, LastName)) Then
					Throw New ArgumentException("Argument failed constraint enforcement.", "LastName")
				End If
				If Not (Me.OnPersonFirstNameChanging(Nothing, FirstName)) Then
					Throw New ArgumentException("Argument failed constraint enforcement.", "FirstName")
				End If
			End If
			Return New PersonCore(Me, LastName, FirstName)
		End Function
		Private ReadOnly myPersonList As List(Of Person)
		Private ReadOnly myPersonReadOnlyCollection As ReadOnlyCollection(Of Person)
		Public ReadOnly Property PersonCollection() As ReadOnlyCollection(Of Person) Implements _
			IPersonCountryDemoContext.PersonCollection
			Get
				Return Me.myPersonReadOnlyCollection
			End Get
		End Property
		#Region "PersonCore"
		Private NotInheritable Class PersonCore
			Inherits Person
			Public Sub New(ByVal context As PersonCountryDemoContext, ByVal LastName As String, ByVal FirstName As String)
				Me.myContext = context
				Me.myLastName = LastName
				Me.myFirstName = FirstName
				context.myPersonList.Add(Me)
			End Sub
			Private ReadOnly myContext As PersonCountryDemoContext
			Public Overrides ReadOnly Property Context() As PersonCountryDemoContext
				Get
					Return Me.myContext
				End Get
			End Property
			Private myLastName As String
			Public Overrides Property LastName() As String
				Get
					Return Me.myLastName
				End Get
				Set(ByVal Value As String)
					If Value Is Nothing Then
						Return
					End If
					If Not (Object.Equals(Me.LastName, Value)) Then
						If Me.Context.OnPersonLastNameChanging(Me, Value) Then
							If MyBase.RaiseLastNameChangingEvent(Value) Then
								Dim oldValue As String = Me.LastName
								Me.myLastName = Value
								MyBase.RaiseLastNameChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private myFirstName As String
			Public Overrides Property FirstName() As String
				Get
					Return Me.myFirstName
				End Get
				Set(ByVal Value As String)
					If Value Is Nothing Then
						Return
					End If
					If Not (Object.Equals(Me.FirstName, Value)) Then
						If Me.Context.OnPersonFirstNameChanging(Me, Value) Then
							If MyBase.RaiseFirstNameChangingEvent(Value) Then
								Dim oldValue As String = Me.FirstName
								Me.myFirstName = Value
								MyBase.RaiseFirstNameChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private myTitle As String
			Public Overrides Property Title() As String
				Get
					Return Me.myTitle
				End Get
				Set(ByVal Value As String)
					If Not (Object.Equals(Me.Title, Value)) Then
						If Me.Context.OnPersonTitleChanging(Me, Value) Then
							If MyBase.RaiseTitleChangingEvent(Value) Then
								Dim oldValue As String = Me.Title
								Me.myTitle = Value
								MyBase.RaiseTitleChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private myCountry As Country
			Public Overrides Property Country() As Country
				Get
					Return Me.myCountry
				End Get
				Set(ByVal Value As Country)
					If Not (Object.Equals(Me.Country, Value)) Then
						If Me.Context.OnPersonCountryChanging(Me, Value) Then
							If MyBase.RaiseCountryChangingEvent(Value) Then
								Dim oldValue As Country = Me.Country
								Me.myCountry = Value
								Me.Context.OnPersonCountryChanged(Me, oldValue)
								MyBase.RaiseCountryChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
		End Class
		#End Region
		Private Function OnCountryCountry_nameChanging(ByVal instance As Country, ByVal newValue As String) As Boolean
			Dim currentInstance As Country = instance
			If Me.myCountryCountry_nameDictionary.TryGetValue(newValue, currentInstance) Then
				If Not (Object.Equals(currentInstance, instance)) Then
					Return False
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnCountryCountry_nameChanged(ByVal instance As Country, ByVal oldValue As String)
			Me.myCountryCountry_nameDictionary.Add(instance.Country_name, instance)
			If oldValue IsNot Nothing Then
				Me.myCountryCountry_nameDictionary.Remove(oldValue)
			Else
			End If
		End Sub
		Private Function OnCountryRegion_Region_codeChanging(ByVal instance As Country, ByVal newValue As String) As Boolean
			Return True
		End Function
		Private Function OnCountryPersonAdding(ByVal instance As Country, ByVal value As Person) As Boolean
			Return True
		End Function
		Private Sub OnCountryPersonAdded(ByVal instance As Country, ByVal value As Person)
			If value IsNot Nothing Then
				value.Country = instance
			End If
		End Sub
		Private Function OnCountryPersonRemoving(ByVal instance As Country, ByVal value As Person) As Boolean
			Return True
		End Function
		Private Sub OnCountryPersonRemoved(ByVal instance As Country, ByVal value As Person)
			If value IsNot Nothing Then
				value.Country = Nothing
			End If
		End Sub
		Public Function CreateCountry(ByVal Country_name As String) As Country
			If Not (Me.IsDeserializing) Then
				If Not (Me.OnCountryCountry_nameChanging(Nothing, Country_name)) Then
					Throw New ArgumentException("Argument failed constraint enforcement.", "Country_name")
				End If
			End If
			Return New CountryCore(Me, Country_name)
		End Function
		Private ReadOnly myCountryList As List(Of Country)
		Private ReadOnly myCountryReadOnlyCollection As ReadOnlyCollection(Of Country)
		Public ReadOnly Property CountryCollection() As ReadOnlyCollection(Of Country) Implements _
			IPersonCountryDemoContext.CountryCollection
			Get
				Return Me.myCountryReadOnlyCollection
			End Get
		End Property
		#Region "CountryCore"
		Private NotInheritable Class CountryCore
			Inherits Country
			Public Sub New(ByVal context As PersonCountryDemoContext, ByVal Country_name As String)
				Me.myContext = context
				Me.myPerson = New ConstraintEnforcementCollection(Of Country, Person)(Me, New PotentialCollectionModificationCallback(Of Country, Person)(context.OnCountryPersonAdding), New CommittedCollectionModificationCallback(Of Country, Person)(context.OnCountryPersonAdded), New PotentialCollectionModificationCallback(Of Country, Person)(context.OnCountryPersonRemoving), New CommittedCollectionModificationCallback(Of Country, Person)(context.OnCountryPersonRemoved))
				Me.myCountry_name = Country_name
				context.OnCountryCountry_nameChanged(Me, Nothing)
				context.myCountryList.Add(Me)
			End Sub
			Private ReadOnly myContext As PersonCountryDemoContext
			Public Overrides ReadOnly Property Context() As PersonCountryDemoContext
				Get
					Return Me.myContext
				End Get
			End Property
			Private myCountry_name As String
			Public Overrides Property Country_name() As String
				Get
					Return Me.myCountry_name
				End Get
				Set(ByVal Value As String)
					If Value Is Nothing Then
						Return
					End If
					If Not (Object.Equals(Me.Country_name, Value)) Then
						If Me.Context.OnCountryCountry_nameChanging(Me, Value) Then
							If MyBase.RaiseCountry_nameChangingEvent(Value) Then
								Dim oldValue As String = Me.Country_name
								Me.myCountry_name = Value
								Me.Context.OnCountryCountry_nameChanged(Me, oldValue)
								MyBase.RaiseCountry_nameChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private myRegion_Region_code As String
			Public Overrides Property Region_Region_code() As String
				Get
					Return Me.myRegion_Region_code
				End Get
				Set(ByVal Value As String)
					If Not (Object.Equals(Me.Region_Region_code, Value)) Then
						If Me.Context.OnCountryRegion_Region_codeChanging(Me, Value) Then
							If MyBase.RaiseRegion_Region_codeChangingEvent(Value) Then
								Dim oldValue As String = Me.Region_Region_code
								Me.myRegion_Region_code = Value
								MyBase.RaiseRegion_Region_codeChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private ReadOnly myPerson As ICollection(Of Person)
			Public Overrides ReadOnly Property Person() As ICollection(Of Person)
				Get
					Return Me.myPerson
				End Get
			End Property
		End Class
		#End Region
	End Class
	#End Region
End Namespace
