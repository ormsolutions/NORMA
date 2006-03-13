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
Namespace SampleModel
	#Region "PersonDrivesCar"
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	Public MustInherit Partial Class PersonDrivesCar
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
		Public MustOverride ReadOnly Property Context() As SampleModelContext
		Public Custom Event DrivesCar_vinChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseDrivesCar_vinChangingEvent(ByVal newValue As Integer) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Integer)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangingEventArgs(Of Integer)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Integer) = New PropertyChangingEventArgs(Of Integer)(Me.DrivesCar_vin, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event DrivesCar_vinChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseDrivesCar_vinChangedEvent(ByVal oldValue As Integer)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Integer)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangedEventArgs(Of Integer)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Integer)(oldValue, Me.DrivesCar_vin), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("DrivesCar_vin")
			End If
		End Sub
		Public Custom Event DrivenByPersonChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseDrivenByPersonChangingEvent(ByVal newValue As Person) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Person)) = TryCast(Me.Events(2), EventHandler(Of PropertyChangingEventArgs(Of Person)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Person) = New PropertyChangingEventArgs(Of Person)(Me.DrivenByPerson, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event DrivenByPersonChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseDrivenByPersonChangedEvent(ByVal oldValue As Person)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Person)) = TryCast(Me.Events(2), EventHandler(Of PropertyChangedEventArgs(Of Person)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Person)(oldValue, Me.DrivenByPerson), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("DrivenByPerson")
			End If
		End Sub
		Public MustOverride Property DrivesCar_vin() As Integer
		Public MustOverride Property DrivenByPerson() As Person
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Overridable Function ToString(ByVal provider As IFormatProvider) As String
			Return String.Format(provider, "PersonDrivesCar{0}{{{0}{1}DrivesCar_vin = ""{2}"",{0}{1}DrivenByPerson = {3}{0}}}", Environment.NewLine, "", Me.DrivesCar_vin, "TODO: Recursively call ToString for customTypes...")
		End Function
	End Class
	#End Region
	#Region "PersonBoughtCarFromPersonOnDate"
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	Public MustInherit Partial Class PersonBoughtCarFromPersonOnDate
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
		Public MustOverride ReadOnly Property Context() As SampleModelContext
		Public Custom Event CarSold_vinChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseCarSold_vinChangingEvent(ByVal newValue As Integer) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Integer)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangingEventArgs(Of Integer)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Integer) = New PropertyChangingEventArgs(Of Integer)(Me.CarSold_vin, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event CarSold_vinChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseCarSold_vinChangedEvent(ByVal oldValue As Integer)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Integer)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangedEventArgs(Of Integer)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Integer)(oldValue, Me.CarSold_vin), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("CarSold_vin")
			End If
		End Sub
		Public Custom Event SaleDate_YMDChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseSaleDate_YMDChangingEvent(ByVal newValue As Integer) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Integer)) = TryCast(Me.Events(2), EventHandler(Of PropertyChangingEventArgs(Of Integer)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Integer) = New PropertyChangingEventArgs(Of Integer)(Me.SaleDate_YMD, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event SaleDate_YMDChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseSaleDate_YMDChangedEvent(ByVal oldValue As Integer)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Integer)) = TryCast(Me.Events(2), EventHandler(Of PropertyChangedEventArgs(Of Integer)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Integer)(oldValue, Me.SaleDate_YMD), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("SaleDate_YMD")
			End If
		End Sub
		Public Custom Event BuyerChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Combine(Me.Events(3), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Remove(Me.Events(3), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseBuyerChangingEvent(ByVal newValue As Person) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Person)) = TryCast(Me.Events(3), EventHandler(Of PropertyChangingEventArgs(Of Person)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Person) = New PropertyChangingEventArgs(Of Person)(Me.Buyer, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event BuyerChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Combine(Me.Events(3), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Remove(Me.Events(3), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseBuyerChangedEvent(ByVal oldValue As Person)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Person)) = TryCast(Me.Events(3), EventHandler(Of PropertyChangedEventArgs(Of Person)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Person)(oldValue, Me.Buyer), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Buyer")
			End If
		End Sub
		Public Custom Event SellerChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(4) = System.Delegate.Combine(Me.Events(4), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(4) = System.Delegate.Remove(Me.Events(4), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseSellerChangingEvent(ByVal newValue As Person) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Person)) = TryCast(Me.Events(4), EventHandler(Of PropertyChangingEventArgs(Of Person)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Person) = New PropertyChangingEventArgs(Of Person)(Me.Seller, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event SellerChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(4) = System.Delegate.Combine(Me.Events(4), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(4) = System.Delegate.Remove(Me.Events(4), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseSellerChangedEvent(ByVal oldValue As Person)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Person)) = TryCast(Me.Events(4), EventHandler(Of PropertyChangedEventArgs(Of Person)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Person)(oldValue, Me.Seller), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Seller")
			End If
		End Sub
		Public MustOverride Property CarSold_vin() As Integer
		Public MustOverride Property SaleDate_YMD() As Integer
		Public MustOverride Property Buyer() As Person
		Public MustOverride Property Seller() As Person
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Overridable Function ToString(ByVal provider As IFormatProvider) As String
			Return String.Format(provider, "PersonBoughtCarFromPersonOnDate{0}{{{0}{1}CarSold_vin = ""{2}"",{0}{1}SaleDate_YMD = ""{3}"",{0}{1}Buyer = {4},{0}{1}Seller = {5}{0}}}", Environment.NewLine, "", Me.CarSold_vin, Me.SaleDate_YMD, "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...")
		End Function
	End Class
	#End Region
	#Region "Review"
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	Public MustInherit Partial Class Review
		Implements INotifyPropertyChanged
		Protected Sub New()
		End Sub
		Private ReadOnly Events As System.Delegate() = New System.Delegate(4) {}
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
		Public MustOverride ReadOnly Property Context() As SampleModelContext
		Public Custom Event Car_vinChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseCar_vinChangingEvent(ByVal newValue As Integer) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Integer)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangingEventArgs(Of Integer)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Integer) = New PropertyChangingEventArgs(Of Integer)(Me.Car_vin, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event Car_vinChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseCar_vinChangedEvent(ByVal oldValue As Integer)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Integer)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangedEventArgs(Of Integer)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Integer)(oldValue, Me.Car_vin), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Car_vin")
			End If
		End Sub
		Public Custom Event Rating_Nr_IntegerChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseRating_Nr_IntegerChangingEvent(ByVal newValue As Integer) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Integer)) = TryCast(Me.Events(2), EventHandler(Of PropertyChangingEventArgs(Of Integer)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Integer) = New PropertyChangingEventArgs(Of Integer)(Me.Rating_Nr_Integer, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event Rating_Nr_IntegerChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseRating_Nr_IntegerChangedEvent(ByVal oldValue As Integer)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Integer)) = TryCast(Me.Events(2), EventHandler(Of PropertyChangedEventArgs(Of Integer)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Integer)(oldValue, Me.Rating_Nr_Integer), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Rating_Nr_Integer")
			End If
		End Sub
		Public Custom Event Criteria_NameChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Combine(Me.Events(3), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Remove(Me.Events(3), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseCriteria_NameChangingEvent(ByVal newValue As String) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of String)) = TryCast(Me.Events(3), EventHandler(Of PropertyChangingEventArgs(Of String)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of String) = New PropertyChangingEventArgs(Of String)(Me.Criteria_Name, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event Criteria_NameChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Combine(Me.Events(3), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Remove(Me.Events(3), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseCriteria_NameChangedEvent(ByVal oldValue As String)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of String)) = TryCast(Me.Events(3), EventHandler(Of PropertyChangedEventArgs(Of String)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of String)(oldValue, Me.Criteria_Name), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Criteria_Name")
			End If
		End Sub
		Public MustOverride Property Car_vin() As Integer
		Public MustOverride Property Rating_Nr_Integer() As Integer
		Public MustOverride Property Criteria_Name() As String
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Overridable Function ToString(ByVal provider As IFormatProvider) As String
			Return String.Format(provider, "Review{0}{{{0}{1}Car_vin = ""{2}"",{0}{1}Rating_Nr_Integer = ""{3}"",{0}{1}Criteria_Name = ""{4}""{0}}}", Environment.NewLine, "", Me.Car_vin, Me.Rating_Nr_Integer, Me.Criteria_Name)
		End Function
	End Class
	#End Region
	#Region "PersonHasNickName"
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	Public MustInherit Partial Class PersonHasNickName
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
		Public MustOverride ReadOnly Property Context() As SampleModelContext
		Public Custom Event NickNameChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseNickNameChangingEvent(ByVal newValue As String) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of String)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangingEventArgs(Of String)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of String) = New PropertyChangingEventArgs(Of String)(Me.NickName, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event NickNameChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseNickNameChangedEvent(ByVal oldValue As String)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of String)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangedEventArgs(Of String)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of String)(oldValue, Me.NickName), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("NickName")
			End If
		End Sub
		Public Custom Event PersonChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaisePersonChangingEvent(ByVal newValue As Person) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Person)) = TryCast(Me.Events(2), EventHandler(Of PropertyChangingEventArgs(Of Person)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Person) = New PropertyChangingEventArgs(Of Person)(Me.Person, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event PersonChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaisePersonChangedEvent(ByVal oldValue As Person)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Person)) = TryCast(Me.Events(2), EventHandler(Of PropertyChangedEventArgs(Of Person)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Person)(oldValue, Me.Person), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Person")
			End If
		End Sub
		Public MustOverride Property NickName() As String
		Public MustOverride Property Person() As Person
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Overridable Function ToString(ByVal provider As IFormatProvider) As String
			Return String.Format(provider, "PersonHasNickName{0}{{{0}{1}NickName = ""{2}"",{0}{1}Person = {3}{0}}}", Environment.NewLine, "", Me.NickName, "TODO: Recursively call ToString for customTypes...")
		End Function
	End Class
	#End Region
	#Region "Person"
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	Public MustInherit Partial Class Person
		Implements INotifyPropertyChanged
		Protected Sub New()
		End Sub
		Private ReadOnly Events As System.Delegate() = New System.Delegate(15) {}
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
		Public MustOverride ReadOnly Property Context() As SampleModelContext
		Public Custom Event FirstNameChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseFirstNameChangingEvent(ByVal newValue As String) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of String)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangingEventArgs(Of String)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of String) = New PropertyChangingEventArgs(Of String)(Me.FirstName, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event FirstNameChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseFirstNameChangedEvent(ByVal oldValue As String)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of String)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangedEventArgs(Of String)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of String)(oldValue, Me.FirstName), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("FirstName")
			End If
		End Sub
		Public Custom Event Date_YMDChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseDate_YMDChangingEvent(ByVal newValue As Integer) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Integer)) = TryCast(Me.Events(2), EventHandler(Of PropertyChangingEventArgs(Of Integer)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Integer) = New PropertyChangingEventArgs(Of Integer)(Me.Date_YMD, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event Date_YMDChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseDate_YMDChangedEvent(ByVal oldValue As Integer)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Integer)) = TryCast(Me.Events(2), EventHandler(Of PropertyChangedEventArgs(Of Integer)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Integer)(oldValue, Me.Date_YMD), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Date_YMD")
			End If
		End Sub
		Public Custom Event LastNameChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Combine(Me.Events(3), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Remove(Me.Events(3), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseLastNameChangingEvent(ByVal newValue As String) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of String)) = TryCast(Me.Events(3), EventHandler(Of PropertyChangingEventArgs(Of String)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of String) = New PropertyChangingEventArgs(Of String)(Me.LastName, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event LastNameChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Combine(Me.Events(3), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Remove(Me.Events(3), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseLastNameChangedEvent(ByVal oldValue As String)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of String)) = TryCast(Me.Events(3), EventHandler(Of PropertyChangedEventArgs(Of String)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of String)(oldValue, Me.LastName), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("LastName")
			End If
		End Sub
		Public Custom Event SocialSecurityNumberChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(4) = System.Delegate.Combine(Me.Events(4), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(4) = System.Delegate.Remove(Me.Events(4), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseSocialSecurityNumberChangingEvent(ByVal newValue As String) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of String)) = TryCast(Me.Events(4), EventHandler(Of PropertyChangingEventArgs(Of String)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of String) = New PropertyChangingEventArgs(Of String)(Me.SocialSecurityNumber, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event SocialSecurityNumberChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(4) = System.Delegate.Combine(Me.Events(4), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(4) = System.Delegate.Remove(Me.Events(4), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseSocialSecurityNumberChangedEvent(ByVal oldValue As String)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of String)) = TryCast(Me.Events(4), EventHandler(Of PropertyChangedEventArgs(Of String)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of String)(oldValue, Me.SocialSecurityNumber), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("SocialSecurityNumber")
			End If
		End Sub
		Public Custom Event HatType_ColorARGBChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(5) = System.Delegate.Combine(Me.Events(5), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(5) = System.Delegate.Remove(Me.Events(5), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseHatType_ColorARGBChangingEvent(ByVal newValue As Nullable(Of Integer)) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Nullable(Of Integer))) = TryCast(Me.Events(5), EventHandler(Of PropertyChangingEventArgs(Of Nullable(Of Integer))))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Nullable(Of Integer)) = New PropertyChangingEventArgs(Of Nullable(Of Integer))(Me.HatType_ColorARGB, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event HatType_ColorARGBChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(5) = System.Delegate.Combine(Me.Events(5), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(5) = System.Delegate.Remove(Me.Events(5), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseHatType_ColorARGBChangedEvent(ByVal oldValue As Nullable(Of Integer))
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Nullable(Of Integer))) = TryCast(Me.Events(5), EventHandler(Of PropertyChangedEventArgs(Of Nullable(Of Integer))))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Nullable(Of Integer))(oldValue, Me.HatType_ColorARGB), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("HatType_ColorARGB")
			End If
		End Sub
		Public Custom Event HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(6) = System.Delegate.Combine(Me.Events(6), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(6) = System.Delegate.Remove(Me.Events(6), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseHatType_HatTypeStyle_HatTypeStyle_DescriptionChangingEvent(ByVal newValue As String) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of String)) = TryCast(Me.Events(6), EventHandler(Of PropertyChangingEventArgs(Of String)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of String) = New PropertyChangingEventArgs(Of String)(Me.HatType_HatTypeStyle_HatTypeStyle_Description, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(6) = System.Delegate.Combine(Me.Events(6), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(6) = System.Delegate.Remove(Me.Events(6), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseHatType_HatTypeStyle_HatTypeStyle_DescriptionChangedEvent(ByVal oldValue As String)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of String)) = TryCast(Me.Events(6), EventHandler(Of PropertyChangedEventArgs(Of String)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of String)(oldValue, Me.HatType_HatTypeStyle_HatTypeStyle_Description), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("HatType_HatTypeStyle_HatTypeStyle_Description")
			End If
		End Sub
		Public Custom Event OwnsCar_vinChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(7) = System.Delegate.Combine(Me.Events(7), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(7) = System.Delegate.Remove(Me.Events(7), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseOwnsCar_vinChangingEvent(ByVal newValue As Nullable(Of Integer)) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Nullable(Of Integer))) = TryCast(Me.Events(7), EventHandler(Of PropertyChangingEventArgs(Of Nullable(Of Integer))))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Nullable(Of Integer)) = New PropertyChangingEventArgs(Of Nullable(Of Integer))(Me.OwnsCar_vin, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event OwnsCar_vinChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(7) = System.Delegate.Combine(Me.Events(7), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(7) = System.Delegate.Remove(Me.Events(7), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseOwnsCar_vinChangedEvent(ByVal oldValue As Nullable(Of Integer))
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Nullable(Of Integer))) = TryCast(Me.Events(7), EventHandler(Of PropertyChangedEventArgs(Of Nullable(Of Integer))))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Nullable(Of Integer))(oldValue, Me.OwnsCar_vin), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("OwnsCar_vin")
			End If
		End Sub
		Public Custom Event Gender_Gender_CodeChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(8) = System.Delegate.Combine(Me.Events(8), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(8) = System.Delegate.Remove(Me.Events(8), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseGender_Gender_CodeChangingEvent(ByVal newValue As String) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of String)) = TryCast(Me.Events(8), EventHandler(Of PropertyChangingEventArgs(Of String)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of String) = New PropertyChangingEventArgs(Of String)(Me.Gender_Gender_Code, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event Gender_Gender_CodeChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(8) = System.Delegate.Combine(Me.Events(8), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(8) = System.Delegate.Remove(Me.Events(8), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseGender_Gender_CodeChangedEvent(ByVal oldValue As String)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of String)) = TryCast(Me.Events(8), EventHandler(Of PropertyChangedEventArgs(Of String)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of String)(oldValue, Me.Gender_Gender_Code), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Gender_Gender_Code")
			End If
		End Sub
		Public Custom Event PersonHasParentsChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(9) = System.Delegate.Combine(Me.Events(9), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(9) = System.Delegate.Remove(Me.Events(9), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaisePersonHasParentsChangingEvent(ByVal newValue As Nullable(Of Boolean)) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Nullable(Of Boolean))) = TryCast(Me.Events(9), EventHandler(Of PropertyChangingEventArgs(Of Nullable(Of Boolean))))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Nullable(Of Boolean)) = New PropertyChangingEventArgs(Of Nullable(Of Boolean))(Me.PersonHasParents, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event PersonHasParentsChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(9) = System.Delegate.Combine(Me.Events(9), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(9) = System.Delegate.Remove(Me.Events(9), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaisePersonHasParentsChangedEvent(ByVal oldValue As Nullable(Of Boolean))
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Nullable(Of Boolean))) = TryCast(Me.Events(9), EventHandler(Of PropertyChangedEventArgs(Of Nullable(Of Boolean))))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Nullable(Of Boolean))(oldValue, Me.PersonHasParents), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("PersonHasParents")
			End If
		End Sub
		Public Custom Event ValueType1DoesSomethingElseWithChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(10) = System.Delegate.Combine(Me.Events(10), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(10) = System.Delegate.Remove(Me.Events(10), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseValueType1DoesSomethingElseWithChangingEvent(ByVal newValue As ValueType1) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of ValueType1)) = TryCast(Me.Events(10), EventHandler(Of PropertyChangingEventArgs(Of ValueType1)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of ValueType1) = New PropertyChangingEventArgs(Of ValueType1)(Me.ValueType1DoesSomethingElseWith, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event ValueType1DoesSomethingElseWithChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(10) = System.Delegate.Combine(Me.Events(10), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(10) = System.Delegate.Remove(Me.Events(10), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseValueType1DoesSomethingElseWithChangedEvent(ByVal oldValue As ValueType1)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of ValueType1)) = TryCast(Me.Events(10), EventHandler(Of PropertyChangedEventArgs(Of ValueType1)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of ValueType1)(oldValue, Me.ValueType1DoesSomethingElseWith), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("ValueType1DoesSomethingElseWith")
			End If
		End Sub
		Public Custom Event MalePersonChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(11) = System.Delegate.Combine(Me.Events(11), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(11) = System.Delegate.Remove(Me.Events(11), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseMalePersonChangingEvent(ByVal newValue As MalePerson) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of MalePerson)) = TryCast(Me.Events(11), EventHandler(Of PropertyChangingEventArgs(Of MalePerson)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of MalePerson) = New PropertyChangingEventArgs(Of MalePerson)(Me.MalePerson, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event MalePersonChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(11) = System.Delegate.Combine(Me.Events(11), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(11) = System.Delegate.Remove(Me.Events(11), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseMalePersonChangedEvent(ByVal oldValue As MalePerson)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of MalePerson)) = TryCast(Me.Events(11), EventHandler(Of PropertyChangedEventArgs(Of MalePerson)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of MalePerson)(oldValue, Me.MalePerson), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("MalePerson")
			End If
		End Sub
		Public Custom Event FemalePersonChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(12) = System.Delegate.Combine(Me.Events(12), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(12) = System.Delegate.Remove(Me.Events(12), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseFemalePersonChangingEvent(ByVal newValue As FemalePerson) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of FemalePerson)) = TryCast(Me.Events(12), EventHandler(Of PropertyChangingEventArgs(Of FemalePerson)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of FemalePerson) = New PropertyChangingEventArgs(Of FemalePerson)(Me.FemalePerson, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event FemalePersonChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(12) = System.Delegate.Combine(Me.Events(12), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(12) = System.Delegate.Remove(Me.Events(12), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseFemalePersonChangedEvent(ByVal oldValue As FemalePerson)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of FemalePerson)) = TryCast(Me.Events(12), EventHandler(Of PropertyChangedEventArgs(Of FemalePerson)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of FemalePerson)(oldValue, Me.FemalePerson), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("FemalePerson")
			End If
		End Sub
		Public Custom Event ChildPersonChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(13) = System.Delegate.Combine(Me.Events(13), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(13) = System.Delegate.Remove(Me.Events(13), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseChildPersonChangingEvent(ByVal newValue As ChildPerson) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of ChildPerson)) = TryCast(Me.Events(13), EventHandler(Of PropertyChangingEventArgs(Of ChildPerson)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of ChildPerson) = New PropertyChangingEventArgs(Of ChildPerson)(Me.ChildPerson, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event ChildPersonChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(13) = System.Delegate.Combine(Me.Events(13), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(13) = System.Delegate.Remove(Me.Events(13), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseChildPersonChangedEvent(ByVal oldValue As ChildPerson)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of ChildPerson)) = TryCast(Me.Events(13), EventHandler(Of PropertyChangedEventArgs(Of ChildPerson)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of ChildPerson)(oldValue, Me.ChildPerson), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("ChildPerson")
			End If
		End Sub
		Public Custom Event DeathChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(14) = System.Delegate.Combine(Me.Events(14), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(14) = System.Delegate.Remove(Me.Events(14), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseDeathChangingEvent(ByVal newValue As Death) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Death)) = TryCast(Me.Events(14), EventHandler(Of PropertyChangingEventArgs(Of Death)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Death) = New PropertyChangingEventArgs(Of Death)(Me.Death, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event DeathChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(14) = System.Delegate.Combine(Me.Events(14), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(14) = System.Delegate.Remove(Me.Events(14), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseDeathChangedEvent(ByVal oldValue As Death)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Death)) = TryCast(Me.Events(14), EventHandler(Of PropertyChangedEventArgs(Of Death)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Death)(oldValue, Me.Death), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Death")
			End If
		End Sub
		Public MustOverride Property FirstName() As String
		Public MustOverride Property Date_YMD() As Integer
		Public MustOverride Property LastName() As String
		Public MustOverride Property SocialSecurityNumber() As String
		Public MustOverride Property HatType_ColorARGB() As Nullable(Of Integer)
		Public MustOverride Property HatType_HatTypeStyle_HatTypeStyle_Description() As String
		Public MustOverride Property OwnsCar_vin() As Nullable(Of Integer)
		Public MustOverride Property Gender_Gender_Code() As String
		Public MustOverride Property PersonHasParents() As Nullable(Of Boolean)
		Public MustOverride Property ValueType1DoesSomethingElseWith() As ValueType1
		Public MustOverride Property MalePerson() As MalePerson
		Public MustOverride Property FemalePerson() As FemalePerson
		Public MustOverride Property ChildPerson() As ChildPerson
		Public MustOverride Property Death() As Death
		Public MustOverride ReadOnly Property PersonDrivesCarAsDrivenByPerson() As ICollection(Of PersonDrivesCar)
		Public MustOverride ReadOnly Property PersonBoughtCarFromPersonOnDateAsBuyer() As ICollection(Of PersonBoughtCarFromPersonOnDate)
		Public MustOverride ReadOnly Property PersonBoughtCarFromPersonOnDateAsSeller() As ICollection(Of PersonBoughtCarFromPersonOnDate)
		Public MustOverride ReadOnly Property PersonHasNickNameAsPerson() As ICollection(Of PersonHasNickName)
		Public MustOverride ReadOnly Property Task() As ICollection(Of Task)
		Public MustOverride ReadOnly Property ValueType1DoesSomethingWith() As ICollection(Of ValueType1)
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Overridable Function ToString(ByVal provider As IFormatProvider) As String
			Return String.Format(provider, "Person{0}{{{0}{1}FirstName = ""{2}"",{0}{1}Date_YMD = ""{3}"",{0}{1}LastName = ""{4}"",{0}{1}SocialSecurityNumber = ""{5}"",{0}{1}HatType_ColorARGB = ""{6}"",{0}{1}HatType_HatTypeStyle_HatTypeStyle_Description = ""{7}"",{0}{1}OwnsCar_vin = ""{8}"",{0}{1}Gender_Gender_Code = ""{9}"",{0}{1}PersonHasParents = ""{10}"",{0}{1}ValueType1DoesSomethingElseWith = {11},{0}{1}MalePerson = {12},{0}{1}FemalePerson = {13},{0}{1}ChildPerson = {14},{0}{1}Death = {15}{0}}}", Environment.NewLine, "", Me.FirstName, Me.Date_YMD, Me.LastName, Me.SocialSecurityNumber, Me.HatType_ColorARGB, Me.HatType_HatTypeStyle_HatTypeStyle_Description, Me.OwnsCar_vin, Me.Gender_Gender_Code, Me.PersonHasParents, "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...")
		End Function
		Public Shared Narrowing Operator CType(ByVal Person As Person) As MalePerson
			If Person Is Nothing Then
				Return Nothing
			ElseIf Person.MalePerson Is Nothing Then
				Throw New InvalidCastException()
			Else
				Return Person.MalePerson
			End If
		End Operator
		Public Shared Narrowing Operator CType(ByVal Person As Person) As FemalePerson
			If Person Is Nothing Then
				Return Nothing
			ElseIf Person.FemalePerson Is Nothing Then
				Throw New InvalidCastException()
			Else
				Return Person.FemalePerson
			End If
		End Operator
		Public Shared Narrowing Operator CType(ByVal Person As Person) As ChildPerson
			If Person Is Nothing Then
				Return Nothing
			ElseIf Person.ChildPerson Is Nothing Then
				Throw New InvalidCastException()
			Else
				Return Person.ChildPerson
			End If
		End Operator
		Public Shared Narrowing Operator CType(ByVal Person As Person) As Death
			If Person Is Nothing Then
				Return Nothing
			ElseIf Person.Death Is Nothing Then
				Throw New InvalidCastException()
			Else
				Return Person.Death
			End If
		End Operator
		Public Shared Narrowing Operator CType(ByVal Person As Person) As NaturalDeath
			If Person Is Nothing Then
				Return Nothing
			Else
				Return CType(CType(Person, Death), NaturalDeath)
			End If
		End Operator
		Public Shared Narrowing Operator CType(ByVal Person As Person) As UnnaturalDeath
			If Person Is Nothing Then
				Return Nothing
			Else
				Return CType(CType(Person, Death), UnnaturalDeath)
			End If
		End Operator
	End Class
	#End Region
	#Region "MalePerson"
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	Public MustInherit Partial Class MalePerson
		Implements INotifyPropertyChanged
		Protected Sub New()
		End Sub
		Private ReadOnly Events As System.Delegate() = New System.Delegate(2) {}
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
		Public MustOverride ReadOnly Property Context() As SampleModelContext
		Public Custom Event PersonChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaisePersonChangingEvent(ByVal newValue As Person) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Person)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangingEventArgs(Of Person)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Person) = New PropertyChangingEventArgs(Of Person)(Me.Person, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event PersonChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaisePersonChangedEvent(ByVal oldValue As Person)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Person)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangedEventArgs(Of Person)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Person)(oldValue, Me.Person), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Person")
			End If
		End Sub
		Public MustOverride Property Person() As Person
		Public MustOverride ReadOnly Property ChildPerson() As ICollection(Of ChildPerson)
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Overridable Function ToString(ByVal provider As IFormatProvider) As String
			Return String.Format(provider, "MalePerson{0}{{{0}{1}Person = {2}{0}}}", Environment.NewLine, "", "TODO: Recursively call ToString for customTypes...")
		End Function
		Public Shared Widening Operator CType(ByVal MalePerson As MalePerson) As Person
			If MalePerson Is Nothing Then
				Return Nothing
			Else
				Return MalePerson.Person
			End If
		End Operator
		Public Overridable Property FirstName() As String
			Get
				Return Me.Person.FirstName
			End Get
			Set(ByVal Value As String)
				Me.Person.FirstName = Value
			End Set
		End Property
		Public Custom Event FirstNameChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.FirstNameChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.FirstNameChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event FirstNameChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.FirstNameChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.FirstNameChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property Date_YMD() As Integer
			Get
				Return Me.Person.Date_YMD
			End Get
			Set(ByVal Value As Integer)
				Me.Person.Date_YMD = Value
			End Set
		End Property
		Public Custom Event Date_YMDChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.Date_YMDChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.Date_YMDChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event Date_YMDChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.Date_YMDChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.Date_YMDChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property LastName() As String
			Get
				Return Me.Person.LastName
			End Get
			Set(ByVal Value As String)
				Me.Person.LastName = Value
			End Set
		End Property
		Public Custom Event LastNameChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.LastNameChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.LastNameChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event LastNameChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.LastNameChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.LastNameChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property SocialSecurityNumber() As String
			Get
				Return Me.Person.SocialSecurityNumber
			End Get
			Set(ByVal Value As String)
				Me.Person.SocialSecurityNumber = Value
			End Set
		End Property
		Public Custom Event SocialSecurityNumberChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.SocialSecurityNumberChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.SocialSecurityNumberChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event SocialSecurityNumberChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.SocialSecurityNumberChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.SocialSecurityNumberChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property HatType_ColorARGB() As Nullable(Of Integer)
			Get
				Return Me.Person.HatType_ColorARGB
			End Get
			Set(ByVal Value As Nullable(Of Integer))
				Me.Person.HatType_ColorARGB = Value
			End Set
		End Property
		Public Custom Event HatType_ColorARGBChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.HatType_ColorARGBChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.HatType_ColorARGBChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event HatType_ColorARGBChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.HatType_ColorARGBChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.HatType_ColorARGBChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property HatType_HatTypeStyle_HatTypeStyle_Description() As String
			Get
				Return Me.Person.HatType_HatTypeStyle_HatTypeStyle_Description
			End Get
			Set(ByVal Value As String)
				Me.Person.HatType_HatTypeStyle_HatTypeStyle_Description = Value
			End Set
		End Property
		Public Custom Event HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property OwnsCar_vin() As Nullable(Of Integer)
			Get
				Return Me.Person.OwnsCar_vin
			End Get
			Set(ByVal Value As Nullable(Of Integer))
				Me.Person.OwnsCar_vin = Value
			End Set
		End Property
		Public Custom Event OwnsCar_vinChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.OwnsCar_vinChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.OwnsCar_vinChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event OwnsCar_vinChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.OwnsCar_vinChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.OwnsCar_vinChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property Gender_Gender_Code() As String
			Get
				Return Me.Person.Gender_Gender_Code
			End Get
			Set(ByVal Value As String)
				Me.Person.Gender_Gender_Code = Value
			End Set
		End Property
		Public Custom Event Gender_Gender_CodeChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.Gender_Gender_CodeChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.Gender_Gender_CodeChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event Gender_Gender_CodeChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.Gender_Gender_CodeChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.Gender_Gender_CodeChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property PersonHasParents() As Nullable(Of Boolean)
			Get
				Return Me.Person.PersonHasParents
			End Get
			Set(ByVal Value As Nullable(Of Boolean))
				Me.Person.PersonHasParents = Value
			End Set
		End Property
		Public Custom Event PersonHasParentsChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.PersonHasParentsChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.PersonHasParentsChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event PersonHasParentsChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.PersonHasParentsChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.PersonHasParentsChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property ValueType1DoesSomethingElseWith() As ValueType1
			Get
				Return Me.Person.ValueType1DoesSomethingElseWith
			End Get
			Set(ByVal Value As ValueType1)
				Me.Person.ValueType1DoesSomethingElseWith = Value
			End Set
		End Property
		Public Custom Event ValueType1DoesSomethingElseWithChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.ValueType1DoesSomethingElseWithChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.ValueType1DoesSomethingElseWithChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event ValueType1DoesSomethingElseWithChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.ValueType1DoesSomethingElseWithChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.ValueType1DoesSomethingElseWithChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property FemalePerson() As FemalePerson
			Get
				Return Me.Person.FemalePerson
			End Get
			Set(ByVal Value As FemalePerson)
				Me.Person.FemalePerson = Value
			End Set
		End Property
		Public Custom Event FemalePersonChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.FemalePersonChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.FemalePersonChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event FemalePersonChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.FemalePersonChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.FemalePersonChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property Death() As Death
			Get
				Return Me.Person.Death
			End Get
			Set(ByVal Value As Death)
				Me.Person.Death = Value
			End Set
		End Property
		Public Custom Event DeathChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.DeathChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.DeathChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event DeathChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.DeathChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.DeathChanged, Value
			End RemoveHandler
		End Event
		Public Overridable ReadOnly Property PersonDrivesCarAsDrivenByPerson() As ICollection(Of PersonDrivesCar)
			Get
				Return Me.Person.PersonDrivesCarAsDrivenByPerson
			End Get
		End Property
		Public Overridable ReadOnly Property PersonBoughtCarFromPersonOnDateAsBuyer() As ICollection(Of PersonBoughtCarFromPersonOnDate)
			Get
				Return Me.Person.PersonBoughtCarFromPersonOnDateAsBuyer
			End Get
		End Property
		Public Overridable ReadOnly Property PersonBoughtCarFromPersonOnDateAsSeller() As ICollection(Of PersonBoughtCarFromPersonOnDate)
			Get
				Return Me.Person.PersonBoughtCarFromPersonOnDateAsSeller
			End Get
		End Property
		Public Overridable ReadOnly Property PersonHasNickNameAsPerson() As ICollection(Of PersonHasNickName)
			Get
				Return Me.Person.PersonHasNickNameAsPerson
			End Get
		End Property
		Public Overridable ReadOnly Property Task() As ICollection(Of Task)
			Get
				Return Me.Person.Task
			End Get
		End Property
		Public Overridable ReadOnly Property ValueType1DoesSomethingWith() As ICollection(Of ValueType1)
			Get
				Return Me.Person.ValueType1DoesSomethingWith
			End Get
		End Property
	End Class
	#End Region
	#Region "FemalePerson"
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	Public MustInherit Partial Class FemalePerson
		Implements INotifyPropertyChanged
		Protected Sub New()
		End Sub
		Private ReadOnly Events As System.Delegate() = New System.Delegate(2) {}
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
		Public MustOverride ReadOnly Property Context() As SampleModelContext
		Public Custom Event PersonChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaisePersonChangingEvent(ByVal newValue As Person) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Person)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangingEventArgs(Of Person)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Person) = New PropertyChangingEventArgs(Of Person)(Me.Person, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event PersonChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaisePersonChangedEvent(ByVal oldValue As Person)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Person)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangedEventArgs(Of Person)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Person)(oldValue, Me.Person), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Person")
			End If
		End Sub
		Public MustOverride Property Person() As Person
		Public MustOverride ReadOnly Property ChildPerson() As ICollection(Of ChildPerson)
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Overridable Function ToString(ByVal provider As IFormatProvider) As String
			Return String.Format(provider, "FemalePerson{0}{{{0}{1}Person = {2}{0}}}", Environment.NewLine, "", "TODO: Recursively call ToString for customTypes...")
		End Function
		Public Shared Widening Operator CType(ByVal FemalePerson As FemalePerson) As Person
			If FemalePerson Is Nothing Then
				Return Nothing
			Else
				Return FemalePerson.Person
			End If
		End Operator
		Public Overridable Property FirstName() As String
			Get
				Return Me.Person.FirstName
			End Get
			Set(ByVal Value As String)
				Me.Person.FirstName = Value
			End Set
		End Property
		Public Custom Event FirstNameChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.FirstNameChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.FirstNameChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event FirstNameChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.FirstNameChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.FirstNameChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property Date_YMD() As Integer
			Get
				Return Me.Person.Date_YMD
			End Get
			Set(ByVal Value As Integer)
				Me.Person.Date_YMD = Value
			End Set
		End Property
		Public Custom Event Date_YMDChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.Date_YMDChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.Date_YMDChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event Date_YMDChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.Date_YMDChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.Date_YMDChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property LastName() As String
			Get
				Return Me.Person.LastName
			End Get
			Set(ByVal Value As String)
				Me.Person.LastName = Value
			End Set
		End Property
		Public Custom Event LastNameChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.LastNameChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.LastNameChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event LastNameChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.LastNameChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.LastNameChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property SocialSecurityNumber() As String
			Get
				Return Me.Person.SocialSecurityNumber
			End Get
			Set(ByVal Value As String)
				Me.Person.SocialSecurityNumber = Value
			End Set
		End Property
		Public Custom Event SocialSecurityNumberChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.SocialSecurityNumberChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.SocialSecurityNumberChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event SocialSecurityNumberChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.SocialSecurityNumberChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.SocialSecurityNumberChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property HatType_ColorARGB() As Nullable(Of Integer)
			Get
				Return Me.Person.HatType_ColorARGB
			End Get
			Set(ByVal Value As Nullable(Of Integer))
				Me.Person.HatType_ColorARGB = Value
			End Set
		End Property
		Public Custom Event HatType_ColorARGBChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.HatType_ColorARGBChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.HatType_ColorARGBChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event HatType_ColorARGBChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.HatType_ColorARGBChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.HatType_ColorARGBChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property HatType_HatTypeStyle_HatTypeStyle_Description() As String
			Get
				Return Me.Person.HatType_HatTypeStyle_HatTypeStyle_Description
			End Get
			Set(ByVal Value As String)
				Me.Person.HatType_HatTypeStyle_HatTypeStyle_Description = Value
			End Set
		End Property
		Public Custom Event HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property OwnsCar_vin() As Nullable(Of Integer)
			Get
				Return Me.Person.OwnsCar_vin
			End Get
			Set(ByVal Value As Nullable(Of Integer))
				Me.Person.OwnsCar_vin = Value
			End Set
		End Property
		Public Custom Event OwnsCar_vinChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.OwnsCar_vinChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.OwnsCar_vinChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event OwnsCar_vinChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.OwnsCar_vinChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.OwnsCar_vinChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property Gender_Gender_Code() As String
			Get
				Return Me.Person.Gender_Gender_Code
			End Get
			Set(ByVal Value As String)
				Me.Person.Gender_Gender_Code = Value
			End Set
		End Property
		Public Custom Event Gender_Gender_CodeChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.Gender_Gender_CodeChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.Gender_Gender_CodeChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event Gender_Gender_CodeChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.Gender_Gender_CodeChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.Gender_Gender_CodeChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property PersonHasParents() As Nullable(Of Boolean)
			Get
				Return Me.Person.PersonHasParents
			End Get
			Set(ByVal Value As Nullable(Of Boolean))
				Me.Person.PersonHasParents = Value
			End Set
		End Property
		Public Custom Event PersonHasParentsChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.PersonHasParentsChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.PersonHasParentsChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event PersonHasParentsChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.PersonHasParentsChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.PersonHasParentsChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property ValueType1DoesSomethingElseWith() As ValueType1
			Get
				Return Me.Person.ValueType1DoesSomethingElseWith
			End Get
			Set(ByVal Value As ValueType1)
				Me.Person.ValueType1DoesSomethingElseWith = Value
			End Set
		End Property
		Public Custom Event ValueType1DoesSomethingElseWithChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.ValueType1DoesSomethingElseWithChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.ValueType1DoesSomethingElseWithChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event ValueType1DoesSomethingElseWithChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.ValueType1DoesSomethingElseWithChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.ValueType1DoesSomethingElseWithChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property MalePerson() As MalePerson
			Get
				Return Me.Person.MalePerson
			End Get
			Set(ByVal Value As MalePerson)
				Me.Person.MalePerson = Value
			End Set
		End Property
		Public Custom Event MalePersonChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.MalePersonChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.MalePersonChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event MalePersonChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.MalePersonChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.MalePersonChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property Death() As Death
			Get
				Return Me.Person.Death
			End Get
			Set(ByVal Value As Death)
				Me.Person.Death = Value
			End Set
		End Property
		Public Custom Event DeathChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.DeathChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.DeathChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event DeathChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.DeathChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.DeathChanged, Value
			End RemoveHandler
		End Event
		Public Overridable ReadOnly Property PersonDrivesCarAsDrivenByPerson() As ICollection(Of PersonDrivesCar)
			Get
				Return Me.Person.PersonDrivesCarAsDrivenByPerson
			End Get
		End Property
		Public Overridable ReadOnly Property PersonBoughtCarFromPersonOnDateAsBuyer() As ICollection(Of PersonBoughtCarFromPersonOnDate)
			Get
				Return Me.Person.PersonBoughtCarFromPersonOnDateAsBuyer
			End Get
		End Property
		Public Overridable ReadOnly Property PersonBoughtCarFromPersonOnDateAsSeller() As ICollection(Of PersonBoughtCarFromPersonOnDate)
			Get
				Return Me.Person.PersonBoughtCarFromPersonOnDateAsSeller
			End Get
		End Property
		Public Overridable ReadOnly Property PersonHasNickNameAsPerson() As ICollection(Of PersonHasNickName)
			Get
				Return Me.Person.PersonHasNickNameAsPerson
			End Get
		End Property
		Public Overridable ReadOnly Property Task() As ICollection(Of Task)
			Get
				Return Me.Person.Task
			End Get
		End Property
		Public Overridable ReadOnly Property ValueType1DoesSomethingWith() As ICollection(Of ValueType1)
			Get
				Return Me.Person.ValueType1DoesSomethingWith
			End Get
		End Property
	End Class
	#End Region
	#Region "ChildPerson"
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	Public MustInherit Partial Class ChildPerson
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
		Public MustOverride ReadOnly Property Context() As SampleModelContext
		Public Custom Event BirthOrder_BirthOrder_NrChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseBirthOrder_BirthOrder_NrChangingEvent(ByVal newValue As Integer) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Integer)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangingEventArgs(Of Integer)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Integer) = New PropertyChangingEventArgs(Of Integer)(Me.BirthOrder_BirthOrder_Nr, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event BirthOrder_BirthOrder_NrChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseBirthOrder_BirthOrder_NrChangedEvent(ByVal oldValue As Integer)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Integer)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangedEventArgs(Of Integer)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Integer)(oldValue, Me.BirthOrder_BirthOrder_Nr), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("BirthOrder_BirthOrder_Nr")
			End If
		End Sub
		Public Custom Event FatherChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseFatherChangingEvent(ByVal newValue As MalePerson) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of MalePerson)) = TryCast(Me.Events(2), EventHandler(Of PropertyChangingEventArgs(Of MalePerson)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of MalePerson) = New PropertyChangingEventArgs(Of MalePerson)(Me.Father, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event FatherChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseFatherChangedEvent(ByVal oldValue As MalePerson)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of MalePerson)) = TryCast(Me.Events(2), EventHandler(Of PropertyChangedEventArgs(Of MalePerson)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of MalePerson)(oldValue, Me.Father), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Father")
			End If
		End Sub
		Public Custom Event MotherChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Combine(Me.Events(3), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Remove(Me.Events(3), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseMotherChangingEvent(ByVal newValue As FemalePerson) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of FemalePerson)) = TryCast(Me.Events(3), EventHandler(Of PropertyChangingEventArgs(Of FemalePerson)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of FemalePerson) = New PropertyChangingEventArgs(Of FemalePerson)(Me.Mother, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event MotherChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Combine(Me.Events(3), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Remove(Me.Events(3), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseMotherChangedEvent(ByVal oldValue As FemalePerson)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of FemalePerson)) = TryCast(Me.Events(3), EventHandler(Of PropertyChangedEventArgs(Of FemalePerson)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of FemalePerson)(oldValue, Me.Mother), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Mother")
			End If
		End Sub
		Public Custom Event PersonChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(4) = System.Delegate.Combine(Me.Events(4), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(4) = System.Delegate.Remove(Me.Events(4), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaisePersonChangingEvent(ByVal newValue As Person) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Person)) = TryCast(Me.Events(4), EventHandler(Of PropertyChangingEventArgs(Of Person)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Person) = New PropertyChangingEventArgs(Of Person)(Me.Person, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event PersonChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(4) = System.Delegate.Combine(Me.Events(4), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(4) = System.Delegate.Remove(Me.Events(4), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaisePersonChangedEvent(ByVal oldValue As Person)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Person)) = TryCast(Me.Events(4), EventHandler(Of PropertyChangedEventArgs(Of Person)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Person)(oldValue, Me.Person), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Person")
			End If
		End Sub
		Public MustOverride Property BirthOrder_BirthOrder_Nr() As Integer
		Public MustOverride Property Father() As MalePerson
		Public MustOverride Property Mother() As FemalePerson
		Public MustOverride Property Person() As Person
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Overridable Function ToString(ByVal provider As IFormatProvider) As String
			Return String.Format(provider, "ChildPerson{0}{{{0}{1}BirthOrder_BirthOrder_Nr = ""{2}"",{0}{1}Father = {3},{0}{1}Mother = {4},{0}{1}Person = {5}{0}}}", Environment.NewLine, "", Me.BirthOrder_BirthOrder_Nr, "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...")
		End Function
		Public Shared Widening Operator CType(ByVal ChildPerson As ChildPerson) As Person
			If ChildPerson Is Nothing Then
				Return Nothing
			Else
				Return ChildPerson.Person
			End If
		End Operator
		Public Overridable Property FirstName() As String
			Get
				Return Me.Person.FirstName
			End Get
			Set(ByVal Value As String)
				Me.Person.FirstName = Value
			End Set
		End Property
		Public Custom Event FirstNameChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.FirstNameChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.FirstNameChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event FirstNameChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.FirstNameChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.FirstNameChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property Date_YMD() As Integer
			Get
				Return Me.Person.Date_YMD
			End Get
			Set(ByVal Value As Integer)
				Me.Person.Date_YMD = Value
			End Set
		End Property
		Public Custom Event Date_YMDChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.Date_YMDChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.Date_YMDChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event Date_YMDChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.Date_YMDChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.Date_YMDChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property LastName() As String
			Get
				Return Me.Person.LastName
			End Get
			Set(ByVal Value As String)
				Me.Person.LastName = Value
			End Set
		End Property
		Public Custom Event LastNameChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.LastNameChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.LastNameChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event LastNameChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.LastNameChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.LastNameChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property SocialSecurityNumber() As String
			Get
				Return Me.Person.SocialSecurityNumber
			End Get
			Set(ByVal Value As String)
				Me.Person.SocialSecurityNumber = Value
			End Set
		End Property
		Public Custom Event SocialSecurityNumberChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.SocialSecurityNumberChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.SocialSecurityNumberChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event SocialSecurityNumberChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.SocialSecurityNumberChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.SocialSecurityNumberChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property HatType_ColorARGB() As Nullable(Of Integer)
			Get
				Return Me.Person.HatType_ColorARGB
			End Get
			Set(ByVal Value As Nullable(Of Integer))
				Me.Person.HatType_ColorARGB = Value
			End Set
		End Property
		Public Custom Event HatType_ColorARGBChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.HatType_ColorARGBChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.HatType_ColorARGBChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event HatType_ColorARGBChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.HatType_ColorARGBChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.HatType_ColorARGBChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property HatType_HatTypeStyle_HatTypeStyle_Description() As String
			Get
				Return Me.Person.HatType_HatTypeStyle_HatTypeStyle_Description
			End Get
			Set(ByVal Value As String)
				Me.Person.HatType_HatTypeStyle_HatTypeStyle_Description = Value
			End Set
		End Property
		Public Custom Event HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property OwnsCar_vin() As Nullable(Of Integer)
			Get
				Return Me.Person.OwnsCar_vin
			End Get
			Set(ByVal Value As Nullable(Of Integer))
				Me.Person.OwnsCar_vin = Value
			End Set
		End Property
		Public Custom Event OwnsCar_vinChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.OwnsCar_vinChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.OwnsCar_vinChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event OwnsCar_vinChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.OwnsCar_vinChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.OwnsCar_vinChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property Gender_Gender_Code() As String
			Get
				Return Me.Person.Gender_Gender_Code
			End Get
			Set(ByVal Value As String)
				Me.Person.Gender_Gender_Code = Value
			End Set
		End Property
		Public Custom Event Gender_Gender_CodeChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.Gender_Gender_CodeChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.Gender_Gender_CodeChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event Gender_Gender_CodeChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.Gender_Gender_CodeChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.Gender_Gender_CodeChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property PersonHasParents() As Nullable(Of Boolean)
			Get
				Return Me.Person.PersonHasParents
			End Get
			Set(ByVal Value As Nullable(Of Boolean))
				Me.Person.PersonHasParents = Value
			End Set
		End Property
		Public Custom Event PersonHasParentsChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.PersonHasParentsChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.PersonHasParentsChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event PersonHasParentsChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.PersonHasParentsChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.PersonHasParentsChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property ValueType1DoesSomethingElseWith() As ValueType1
			Get
				Return Me.Person.ValueType1DoesSomethingElseWith
			End Get
			Set(ByVal Value As ValueType1)
				Me.Person.ValueType1DoesSomethingElseWith = Value
			End Set
		End Property
		Public Custom Event ValueType1DoesSomethingElseWithChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.ValueType1DoesSomethingElseWithChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.ValueType1DoesSomethingElseWithChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event ValueType1DoesSomethingElseWithChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.ValueType1DoesSomethingElseWithChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.ValueType1DoesSomethingElseWithChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property MalePerson() As MalePerson
			Get
				Return Me.Person.MalePerson
			End Get
			Set(ByVal Value As MalePerson)
				Me.Person.MalePerson = Value
			End Set
		End Property
		Public Custom Event MalePersonChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.MalePersonChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.MalePersonChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event MalePersonChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.MalePersonChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.MalePersonChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property FemalePerson() As FemalePerson
			Get
				Return Me.Person.FemalePerson
			End Get
			Set(ByVal Value As FemalePerson)
				Me.Person.FemalePerson = Value
			End Set
		End Property
		Public Custom Event FemalePersonChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.FemalePersonChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.FemalePersonChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event FemalePersonChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.FemalePersonChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.FemalePersonChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property Death() As Death
			Get
				Return Me.Person.Death
			End Get
			Set(ByVal Value As Death)
				Me.Person.Death = Value
			End Set
		End Property
		Public Custom Event DeathChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.DeathChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.DeathChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event DeathChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.DeathChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.DeathChanged, Value
			End RemoveHandler
		End Event
		Public Overridable ReadOnly Property PersonDrivesCarAsDrivenByPerson() As ICollection(Of PersonDrivesCar)
			Get
				Return Me.Person.PersonDrivesCarAsDrivenByPerson
			End Get
		End Property
		Public Overridable ReadOnly Property PersonBoughtCarFromPersonOnDateAsBuyer() As ICollection(Of PersonBoughtCarFromPersonOnDate)
			Get
				Return Me.Person.PersonBoughtCarFromPersonOnDateAsBuyer
			End Get
		End Property
		Public Overridable ReadOnly Property PersonBoughtCarFromPersonOnDateAsSeller() As ICollection(Of PersonBoughtCarFromPersonOnDate)
			Get
				Return Me.Person.PersonBoughtCarFromPersonOnDateAsSeller
			End Get
		End Property
		Public Overridable ReadOnly Property PersonHasNickNameAsPerson() As ICollection(Of PersonHasNickName)
			Get
				Return Me.Person.PersonHasNickNameAsPerson
			End Get
		End Property
		Public Overridable ReadOnly Property Task() As ICollection(Of Task)
			Get
				Return Me.Person.Task
			End Get
		End Property
		Public Overridable ReadOnly Property ValueType1DoesSomethingWith() As ICollection(Of ValueType1)
			Get
				Return Me.Person.ValueType1DoesSomethingWith
			End Get
		End Property
	End Class
	#End Region
	#Region "Death"
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	Public MustInherit Partial Class Death
		Implements INotifyPropertyChanged
		Protected Sub New()
		End Sub
		Private ReadOnly Events As System.Delegate() = New System.Delegate(6) {}
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
		Public MustOverride ReadOnly Property Context() As SampleModelContext
		Public Custom Event Date_YMDChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseDate_YMDChangingEvent(ByVal newValue As Nullable(Of Integer)) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Nullable(Of Integer))) = TryCast(Me.Events(1), EventHandler(Of PropertyChangingEventArgs(Of Nullable(Of Integer))))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Nullable(Of Integer)) = New PropertyChangingEventArgs(Of Nullable(Of Integer))(Me.Date_YMD, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event Date_YMDChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseDate_YMDChangedEvent(ByVal oldValue As Nullable(Of Integer))
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Nullable(Of Integer))) = TryCast(Me.Events(1), EventHandler(Of PropertyChangedEventArgs(Of Nullable(Of Integer))))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Nullable(Of Integer))(oldValue, Me.Date_YMD), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Date_YMD")
			End If
		End Sub
		Public Custom Event DeathCause_DeathCause_TypeChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseDeathCause_DeathCause_TypeChangingEvent(ByVal newValue As String) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of String)) = TryCast(Me.Events(2), EventHandler(Of PropertyChangingEventArgs(Of String)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of String) = New PropertyChangingEventArgs(Of String)(Me.DeathCause_DeathCause_Type, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event DeathCause_DeathCause_TypeChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseDeathCause_DeathCause_TypeChangedEvent(ByVal oldValue As String)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of String)) = TryCast(Me.Events(2), EventHandler(Of PropertyChangedEventArgs(Of String)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of String)(oldValue, Me.DeathCause_DeathCause_Type), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("DeathCause_DeathCause_Type")
			End If
		End Sub
		Public Custom Event NaturalDeathChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Combine(Me.Events(3), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Remove(Me.Events(3), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseNaturalDeathChangingEvent(ByVal newValue As NaturalDeath) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of NaturalDeath)) = TryCast(Me.Events(3), EventHandler(Of PropertyChangingEventArgs(Of NaturalDeath)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of NaturalDeath) = New PropertyChangingEventArgs(Of NaturalDeath)(Me.NaturalDeath, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event NaturalDeathChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Combine(Me.Events(3), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Remove(Me.Events(3), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseNaturalDeathChangedEvent(ByVal oldValue As NaturalDeath)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of NaturalDeath)) = TryCast(Me.Events(3), EventHandler(Of PropertyChangedEventArgs(Of NaturalDeath)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of NaturalDeath)(oldValue, Me.NaturalDeath), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("NaturalDeath")
			End If
		End Sub
		Public Custom Event UnnaturalDeathChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(4) = System.Delegate.Combine(Me.Events(4), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(4) = System.Delegate.Remove(Me.Events(4), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseUnnaturalDeathChangingEvent(ByVal newValue As UnnaturalDeath) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of UnnaturalDeath)) = TryCast(Me.Events(4), EventHandler(Of PropertyChangingEventArgs(Of UnnaturalDeath)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of UnnaturalDeath) = New PropertyChangingEventArgs(Of UnnaturalDeath)(Me.UnnaturalDeath, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event UnnaturalDeathChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(4) = System.Delegate.Combine(Me.Events(4), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(4) = System.Delegate.Remove(Me.Events(4), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseUnnaturalDeathChangedEvent(ByVal oldValue As UnnaturalDeath)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of UnnaturalDeath)) = TryCast(Me.Events(4), EventHandler(Of PropertyChangedEventArgs(Of UnnaturalDeath)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of UnnaturalDeath)(oldValue, Me.UnnaturalDeath), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("UnnaturalDeath")
			End If
		End Sub
		Public Custom Event PersonChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(5) = System.Delegate.Combine(Me.Events(5), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(5) = System.Delegate.Remove(Me.Events(5), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaisePersonChangingEvent(ByVal newValue As Person) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Person)) = TryCast(Me.Events(5), EventHandler(Of PropertyChangingEventArgs(Of Person)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Person) = New PropertyChangingEventArgs(Of Person)(Me.Person, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event PersonChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(5) = System.Delegate.Combine(Me.Events(5), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(5) = System.Delegate.Remove(Me.Events(5), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaisePersonChangedEvent(ByVal oldValue As Person)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Person)) = TryCast(Me.Events(5), EventHandler(Of PropertyChangedEventArgs(Of Person)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Person)(oldValue, Me.Person), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Person")
			End If
		End Sub
		Public MustOverride Property Date_YMD() As Nullable(Of Integer)
		Public MustOverride Property DeathCause_DeathCause_Type() As String
		Public MustOverride Property NaturalDeath() As NaturalDeath
		Public MustOverride Property UnnaturalDeath() As UnnaturalDeath
		Public MustOverride Property Person() As Person
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Overridable Function ToString(ByVal provider As IFormatProvider) As String
			Return String.Format(provider, "Death{0}{{{0}{1}Date_YMD = ""{2}"",{0}{1}DeathCause_DeathCause_Type = ""{3}"",{0}{1}NaturalDeath = {4},{0}{1}UnnaturalDeath = {5},{0}{1}Person = {6}{0}}}", Environment.NewLine, "", Me.Date_YMD, Me.DeathCause_DeathCause_Type, "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...")
		End Function
		Public Shared Widening Operator CType(ByVal Death As Death) As Person
			If Death Is Nothing Then
				Return Nothing
			Else
				Return Death.Person
			End If
		End Operator
		Public Overridable Property FirstName() As String
			Get
				Return Me.Person.FirstName
			End Get
			Set(ByVal Value As String)
				Me.Person.FirstName = Value
			End Set
		End Property
		Public Custom Event FirstNameChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.FirstNameChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.FirstNameChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event FirstNameChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.FirstNameChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.FirstNameChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property LastName() As String
			Get
				Return Me.Person.LastName
			End Get
			Set(ByVal Value As String)
				Me.Person.LastName = Value
			End Set
		End Property
		Public Custom Event LastNameChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.LastNameChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.LastNameChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event LastNameChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.LastNameChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.LastNameChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property SocialSecurityNumber() As String
			Get
				Return Me.Person.SocialSecurityNumber
			End Get
			Set(ByVal Value As String)
				Me.Person.SocialSecurityNumber = Value
			End Set
		End Property
		Public Custom Event SocialSecurityNumberChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.SocialSecurityNumberChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.SocialSecurityNumberChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event SocialSecurityNumberChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.SocialSecurityNumberChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.SocialSecurityNumberChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property HatType_ColorARGB() As Nullable(Of Integer)
			Get
				Return Me.Person.HatType_ColorARGB
			End Get
			Set(ByVal Value As Nullable(Of Integer))
				Me.Person.HatType_ColorARGB = Value
			End Set
		End Property
		Public Custom Event HatType_ColorARGBChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.HatType_ColorARGBChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.HatType_ColorARGBChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event HatType_ColorARGBChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.HatType_ColorARGBChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.HatType_ColorARGBChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property HatType_HatTypeStyle_HatTypeStyle_Description() As String
			Get
				Return Me.Person.HatType_HatTypeStyle_HatTypeStyle_Description
			End Get
			Set(ByVal Value As String)
				Me.Person.HatType_HatTypeStyle_HatTypeStyle_Description = Value
			End Set
		End Property
		Public Custom Event HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property OwnsCar_vin() As Nullable(Of Integer)
			Get
				Return Me.Person.OwnsCar_vin
			End Get
			Set(ByVal Value As Nullable(Of Integer))
				Me.Person.OwnsCar_vin = Value
			End Set
		End Property
		Public Custom Event OwnsCar_vinChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.OwnsCar_vinChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.OwnsCar_vinChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event OwnsCar_vinChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.OwnsCar_vinChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.OwnsCar_vinChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property Gender_Gender_Code() As String
			Get
				Return Me.Person.Gender_Gender_Code
			End Get
			Set(ByVal Value As String)
				Me.Person.Gender_Gender_Code = Value
			End Set
		End Property
		Public Custom Event Gender_Gender_CodeChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.Gender_Gender_CodeChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.Gender_Gender_CodeChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event Gender_Gender_CodeChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.Gender_Gender_CodeChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.Gender_Gender_CodeChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property PersonHasParents() As Nullable(Of Boolean)
			Get
				Return Me.Person.PersonHasParents
			End Get
			Set(ByVal Value As Nullable(Of Boolean))
				Me.Person.PersonHasParents = Value
			End Set
		End Property
		Public Custom Event PersonHasParentsChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.PersonHasParentsChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.PersonHasParentsChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event PersonHasParentsChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.PersonHasParentsChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.PersonHasParentsChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property ValueType1DoesSomethingElseWith() As ValueType1
			Get
				Return Me.Person.ValueType1DoesSomethingElseWith
			End Get
			Set(ByVal Value As ValueType1)
				Me.Person.ValueType1DoesSomethingElseWith = Value
			End Set
		End Property
		Public Custom Event ValueType1DoesSomethingElseWithChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.ValueType1DoesSomethingElseWithChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.ValueType1DoesSomethingElseWithChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event ValueType1DoesSomethingElseWithChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.ValueType1DoesSomethingElseWithChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.ValueType1DoesSomethingElseWithChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property MalePerson() As MalePerson
			Get
				Return Me.Person.MalePerson
			End Get
			Set(ByVal Value As MalePerson)
				Me.Person.MalePerson = Value
			End Set
		End Property
		Public Custom Event MalePersonChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.MalePersonChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.MalePersonChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event MalePersonChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.MalePersonChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.MalePersonChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property FemalePerson() As FemalePerson
			Get
				Return Me.Person.FemalePerson
			End Get
			Set(ByVal Value As FemalePerson)
				Me.Person.FemalePerson = Value
			End Set
		End Property
		Public Custom Event FemalePersonChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.FemalePersonChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.FemalePersonChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event FemalePersonChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.FemalePersonChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.FemalePersonChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property ChildPerson() As ChildPerson
			Get
				Return Me.Person.ChildPerson
			End Get
			Set(ByVal Value As ChildPerson)
				Me.Person.ChildPerson = Value
			End Set
		End Property
		Public Custom Event ChildPersonChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.ChildPersonChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.ChildPersonChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event ChildPersonChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.ChildPersonChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.ChildPersonChanged, Value
			End RemoveHandler
		End Event
		Public Overridable ReadOnly Property PersonDrivesCarAsDrivenByPerson() As ICollection(Of PersonDrivesCar)
			Get
				Return Me.Person.PersonDrivesCarAsDrivenByPerson
			End Get
		End Property
		Public Overridable ReadOnly Property PersonBoughtCarFromPersonOnDateAsBuyer() As ICollection(Of PersonBoughtCarFromPersonOnDate)
			Get
				Return Me.Person.PersonBoughtCarFromPersonOnDateAsBuyer
			End Get
		End Property
		Public Overridable ReadOnly Property PersonBoughtCarFromPersonOnDateAsSeller() As ICollection(Of PersonBoughtCarFromPersonOnDate)
			Get
				Return Me.Person.PersonBoughtCarFromPersonOnDateAsSeller
			End Get
		End Property
		Public Overridable ReadOnly Property PersonHasNickNameAsPerson() As ICollection(Of PersonHasNickName)
			Get
				Return Me.Person.PersonHasNickNameAsPerson
			End Get
		End Property
		Public Overridable ReadOnly Property Task() As ICollection(Of Task)
			Get
				Return Me.Person.Task
			End Get
		End Property
		Public Overridable ReadOnly Property ValueType1DoesSomethingWith() As ICollection(Of ValueType1)
			Get
				Return Me.Person.ValueType1DoesSomethingWith
			End Get
		End Property
		Public Shared Narrowing Operator CType(ByVal Death As Death) As NaturalDeath
			If Death Is Nothing Then
				Return Nothing
			ElseIf Death.NaturalDeath Is Nothing Then
				Throw New InvalidCastException()
			Else
				Return Death.NaturalDeath
			End If
		End Operator
		Public Shared Narrowing Operator CType(ByVal Death As Death) As UnnaturalDeath
			If Death Is Nothing Then
				Return Nothing
			ElseIf Death.UnnaturalDeath Is Nothing Then
				Throw New InvalidCastException()
			Else
				Return Death.UnnaturalDeath
			End If
		End Operator
	End Class
	#End Region
	#Region "NaturalDeath"
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	Public MustInherit Partial Class NaturalDeath
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
		Public MustOverride ReadOnly Property Context() As SampleModelContext
		Public Custom Event NaturalDeathIsFromProstateCancerChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseNaturalDeathIsFromProstateCancerChangingEvent(ByVal newValue As Nullable(Of Boolean)) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Nullable(Of Boolean))) = TryCast(Me.Events(1), EventHandler(Of PropertyChangingEventArgs(Of Nullable(Of Boolean))))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Nullable(Of Boolean)) = New PropertyChangingEventArgs(Of Nullable(Of Boolean))(Me.NaturalDeathIsFromProstateCancer, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event NaturalDeathIsFromProstateCancerChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseNaturalDeathIsFromProstateCancerChangedEvent(ByVal oldValue As Nullable(Of Boolean))
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Nullable(Of Boolean))) = TryCast(Me.Events(1), EventHandler(Of PropertyChangedEventArgs(Of Nullable(Of Boolean))))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Nullable(Of Boolean))(oldValue, Me.NaturalDeathIsFromProstateCancer), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("NaturalDeathIsFromProstateCancer")
			End If
		End Sub
		Public Custom Event DeathChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseDeathChangingEvent(ByVal newValue As Death) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Death)) = TryCast(Me.Events(2), EventHandler(Of PropertyChangingEventArgs(Of Death)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Death) = New PropertyChangingEventArgs(Of Death)(Me.Death, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event DeathChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseDeathChangedEvent(ByVal oldValue As Death)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Death)) = TryCast(Me.Events(2), EventHandler(Of PropertyChangedEventArgs(Of Death)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Death)(oldValue, Me.Death), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Death")
			End If
		End Sub
		Public MustOverride Property NaturalDeathIsFromProstateCancer() As Nullable(Of Boolean)
		Public MustOverride Property Death() As Death
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Overridable Function ToString(ByVal provider As IFormatProvider) As String
			Return String.Format(provider, "NaturalDeath{0}{{{0}{1}NaturalDeathIsFromProstateCancer = ""{2}"",{0}{1}Death = {3}{0}}}", Environment.NewLine, "", Me.NaturalDeathIsFromProstateCancer, "TODO: Recursively call ToString for customTypes...")
		End Function
		Public Shared Widening Operator CType(ByVal NaturalDeath As NaturalDeath) As Death
			If NaturalDeath Is Nothing Then
				Return Nothing
			Else
				Return NaturalDeath.Death
			End If
		End Operator
		Public Shared Widening Operator CType(ByVal NaturalDeath As NaturalDeath) As Person
			If NaturalDeath Is Nothing Then
				Return Nothing
			Else
				Return NaturalDeath.Death.Person
			End If
		End Operator
		Public Overridable Property Date_YMD() As Nullable(Of Integer)
			Get
				Return Me.Death.Date_YMD
			End Get
			Set(ByVal Value As Nullable(Of Integer))
				Me.Death.Date_YMD = Value
			End Set
		End Property
		Public Custom Event Date_YMDChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Date_YMDChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Date_YMDChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event Date_YMDChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Date_YMDChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Date_YMDChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property DeathCause_DeathCause_Type() As String
			Get
				Return Me.Death.DeathCause_DeathCause_Type
			End Get
			Set(ByVal Value As String)
				Me.Death.DeathCause_DeathCause_Type = Value
			End Set
		End Property
		Public Custom Event DeathCause_DeathCause_TypeChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.DeathCause_DeathCause_TypeChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.DeathCause_DeathCause_TypeChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event DeathCause_DeathCause_TypeChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.DeathCause_DeathCause_TypeChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.DeathCause_DeathCause_TypeChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property UnnaturalDeath() As UnnaturalDeath
			Get
				Return Me.Death.UnnaturalDeath
			End Get
			Set(ByVal Value As UnnaturalDeath)
				Me.Death.UnnaturalDeath = Value
			End Set
		End Property
		Public Custom Event UnnaturalDeathChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.UnnaturalDeathChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.UnnaturalDeathChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event UnnaturalDeathChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.UnnaturalDeathChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.UnnaturalDeathChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property Person() As Person
			Get
				Return Me.Death.Person
			End Get
			Set(ByVal Value As Person)
				Me.Death.Person = Value
			End Set
		End Property
		Public Custom Event PersonChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.PersonChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.PersonChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event PersonChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.PersonChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.PersonChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property FirstName() As String
			Get
				Return Me.Death.Person.FirstName
			End Get
			Set(ByVal Value As String)
				Me.Death.Person.FirstName = Value
			End Set
		End Property
		Public Custom Event FirstNameChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.FirstNameChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.FirstNameChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event FirstNameChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.FirstNameChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.FirstNameChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property LastName() As String
			Get
				Return Me.Death.Person.LastName
			End Get
			Set(ByVal Value As String)
				Me.Death.Person.LastName = Value
			End Set
		End Property
		Public Custom Event LastNameChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.LastNameChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.LastNameChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event LastNameChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.LastNameChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.LastNameChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property SocialSecurityNumber() As String
			Get
				Return Me.Death.Person.SocialSecurityNumber
			End Get
			Set(ByVal Value As String)
				Me.Death.Person.SocialSecurityNumber = Value
			End Set
		End Property
		Public Custom Event SocialSecurityNumberChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.SocialSecurityNumberChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.SocialSecurityNumberChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event SocialSecurityNumberChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.SocialSecurityNumberChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.SocialSecurityNumberChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property HatType_ColorARGB() As Nullable(Of Integer)
			Get
				Return Me.Death.Person.HatType_ColorARGB
			End Get
			Set(ByVal Value As Nullable(Of Integer))
				Me.Death.Person.HatType_ColorARGB = Value
			End Set
		End Property
		Public Custom Event HatType_ColorARGBChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.HatType_ColorARGBChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.HatType_ColorARGBChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event HatType_ColorARGBChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.HatType_ColorARGBChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.HatType_ColorARGBChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property HatType_HatTypeStyle_HatTypeStyle_Description() As String
			Get
				Return Me.Death.Person.HatType_HatTypeStyle_HatTypeStyle_Description
			End Get
			Set(ByVal Value As String)
				Me.Death.Person.HatType_HatTypeStyle_HatTypeStyle_Description = Value
			End Set
		End Property
		Public Custom Event HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property OwnsCar_vin() As Nullable(Of Integer)
			Get
				Return Me.Death.Person.OwnsCar_vin
			End Get
			Set(ByVal Value As Nullable(Of Integer))
				Me.Death.Person.OwnsCar_vin = Value
			End Set
		End Property
		Public Custom Event OwnsCar_vinChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.OwnsCar_vinChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.OwnsCar_vinChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event OwnsCar_vinChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.OwnsCar_vinChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.OwnsCar_vinChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property Gender_Gender_Code() As String
			Get
				Return Me.Death.Person.Gender_Gender_Code
			End Get
			Set(ByVal Value As String)
				Me.Death.Person.Gender_Gender_Code = Value
			End Set
		End Property
		Public Custom Event Gender_Gender_CodeChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.Gender_Gender_CodeChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.Gender_Gender_CodeChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event Gender_Gender_CodeChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.Gender_Gender_CodeChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.Gender_Gender_CodeChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property PersonHasParents() As Nullable(Of Boolean)
			Get
				Return Me.Death.Person.PersonHasParents
			End Get
			Set(ByVal Value As Nullable(Of Boolean))
				Me.Death.Person.PersonHasParents = Value
			End Set
		End Property
		Public Custom Event PersonHasParentsChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.PersonHasParentsChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.PersonHasParentsChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event PersonHasParentsChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.PersonHasParentsChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.PersonHasParentsChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property ValueType1DoesSomethingElseWith() As ValueType1
			Get
				Return Me.Death.Person.ValueType1DoesSomethingElseWith
			End Get
			Set(ByVal Value As ValueType1)
				Me.Death.Person.ValueType1DoesSomethingElseWith = Value
			End Set
		End Property
		Public Custom Event ValueType1DoesSomethingElseWithChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.ValueType1DoesSomethingElseWithChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.ValueType1DoesSomethingElseWithChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event ValueType1DoesSomethingElseWithChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.ValueType1DoesSomethingElseWithChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.ValueType1DoesSomethingElseWithChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property MalePerson() As MalePerson
			Get
				Return Me.Death.Person.MalePerson
			End Get
			Set(ByVal Value As MalePerson)
				Me.Death.Person.MalePerson = Value
			End Set
		End Property
		Public Custom Event MalePersonChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.MalePersonChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.MalePersonChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event MalePersonChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.MalePersonChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.MalePersonChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property FemalePerson() As FemalePerson
			Get
				Return Me.Death.Person.FemalePerson
			End Get
			Set(ByVal Value As FemalePerson)
				Me.Death.Person.FemalePerson = Value
			End Set
		End Property
		Public Custom Event FemalePersonChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.FemalePersonChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.FemalePersonChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event FemalePersonChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.FemalePersonChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.FemalePersonChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property ChildPerson() As ChildPerson
			Get
				Return Me.Death.Person.ChildPerson
			End Get
			Set(ByVal Value As ChildPerson)
				Me.Death.Person.ChildPerson = Value
			End Set
		End Property
		Public Custom Event ChildPersonChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.ChildPersonChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.ChildPersonChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event ChildPersonChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.ChildPersonChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.ChildPersonChanged, Value
			End RemoveHandler
		End Event
		Public Overridable ReadOnly Property PersonDrivesCarAsDrivenByPerson() As ICollection(Of PersonDrivesCar)
			Get
				Return Me.Death.Person.PersonDrivesCarAsDrivenByPerson
			End Get
		End Property
		Public Overridable ReadOnly Property PersonBoughtCarFromPersonOnDateAsBuyer() As ICollection(Of PersonBoughtCarFromPersonOnDate)
			Get
				Return Me.Death.Person.PersonBoughtCarFromPersonOnDateAsBuyer
			End Get
		End Property
		Public Overridable ReadOnly Property PersonBoughtCarFromPersonOnDateAsSeller() As ICollection(Of PersonBoughtCarFromPersonOnDate)
			Get
				Return Me.Death.Person.PersonBoughtCarFromPersonOnDateAsSeller
			End Get
		End Property
		Public Overridable ReadOnly Property PersonHasNickNameAsPerson() As ICollection(Of PersonHasNickName)
			Get
				Return Me.Death.Person.PersonHasNickNameAsPerson
			End Get
		End Property
		Public Overridable ReadOnly Property Task() As ICollection(Of Task)
			Get
				Return Me.Death.Person.Task
			End Get
		End Property
		Public Overridable ReadOnly Property ValueType1DoesSomethingWith() As ICollection(Of ValueType1)
			Get
				Return Me.Death.Person.ValueType1DoesSomethingWith
			End Get
		End Property
	End Class
	#End Region
	#Region "UnnaturalDeath"
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	Public MustInherit Partial Class UnnaturalDeath
		Implements INotifyPropertyChanged
		Protected Sub New()
		End Sub
		Private ReadOnly Events As System.Delegate() = New System.Delegate(4) {}
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
		Public MustOverride ReadOnly Property Context() As SampleModelContext
		Public Custom Event UnnaturalDeathIsViolentChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseUnnaturalDeathIsViolentChangingEvent(ByVal newValue As Nullable(Of Boolean)) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Nullable(Of Boolean))) = TryCast(Me.Events(1), EventHandler(Of PropertyChangingEventArgs(Of Nullable(Of Boolean))))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Nullable(Of Boolean)) = New PropertyChangingEventArgs(Of Nullable(Of Boolean))(Me.UnnaturalDeathIsViolent, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event UnnaturalDeathIsViolentChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseUnnaturalDeathIsViolentChangedEvent(ByVal oldValue As Nullable(Of Boolean))
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Nullable(Of Boolean))) = TryCast(Me.Events(1), EventHandler(Of PropertyChangedEventArgs(Of Nullable(Of Boolean))))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Nullable(Of Boolean))(oldValue, Me.UnnaturalDeathIsViolent), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("UnnaturalDeathIsViolent")
			End If
		End Sub
		Public Custom Event UnnaturalDeathIsBloodyChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseUnnaturalDeathIsBloodyChangingEvent(ByVal newValue As Nullable(Of Boolean)) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Nullable(Of Boolean))) = TryCast(Me.Events(2), EventHandler(Of PropertyChangingEventArgs(Of Nullable(Of Boolean))))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Nullable(Of Boolean)) = New PropertyChangingEventArgs(Of Nullable(Of Boolean))(Me.UnnaturalDeathIsBloody, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event UnnaturalDeathIsBloodyChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseUnnaturalDeathIsBloodyChangedEvent(ByVal oldValue As Nullable(Of Boolean))
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Nullable(Of Boolean))) = TryCast(Me.Events(2), EventHandler(Of PropertyChangedEventArgs(Of Nullable(Of Boolean))))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Nullable(Of Boolean))(oldValue, Me.UnnaturalDeathIsBloody), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("UnnaturalDeathIsBloody")
			End If
		End Sub
		Public Custom Event DeathChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Combine(Me.Events(3), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Remove(Me.Events(3), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseDeathChangingEvent(ByVal newValue As Death) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Death)) = TryCast(Me.Events(3), EventHandler(Of PropertyChangingEventArgs(Of Death)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Death) = New PropertyChangingEventArgs(Of Death)(Me.Death, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event DeathChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Combine(Me.Events(3), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Remove(Me.Events(3), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseDeathChangedEvent(ByVal oldValue As Death)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Death)) = TryCast(Me.Events(3), EventHandler(Of PropertyChangedEventArgs(Of Death)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Death)(oldValue, Me.Death), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Death")
			End If
		End Sub
		Public MustOverride Property UnnaturalDeathIsViolent() As Nullable(Of Boolean)
		Public MustOverride Property UnnaturalDeathIsBloody() As Nullable(Of Boolean)
		Public MustOverride Property Death() As Death
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Overridable Function ToString(ByVal provider As IFormatProvider) As String
			Return String.Format(provider, "UnnaturalDeath{0}{{{0}{1}UnnaturalDeathIsViolent = ""{2}"",{0}{1}UnnaturalDeathIsBloody = ""{3}"",{0}{1}Death = {4}{0}}}", Environment.NewLine, "", Me.UnnaturalDeathIsViolent, Me.UnnaturalDeathIsBloody, "TODO: Recursively call ToString for customTypes...")
		End Function
		Public Shared Widening Operator CType(ByVal UnnaturalDeath As UnnaturalDeath) As Death
			If UnnaturalDeath Is Nothing Then
				Return Nothing
			Else
				Return UnnaturalDeath.Death
			End If
		End Operator
		Public Shared Widening Operator CType(ByVal UnnaturalDeath As UnnaturalDeath) As Person
			If UnnaturalDeath Is Nothing Then
				Return Nothing
			Else
				Return UnnaturalDeath.Death.Person
			End If
		End Operator
		Public Overridable Property Date_YMD() As Nullable(Of Integer)
			Get
				Return Me.Death.Date_YMD
			End Get
			Set(ByVal Value As Nullable(Of Integer))
				Me.Death.Date_YMD = Value
			End Set
		End Property
		Public Custom Event Date_YMDChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Date_YMDChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Date_YMDChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event Date_YMDChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Date_YMDChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Date_YMDChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property DeathCause_DeathCause_Type() As String
			Get
				Return Me.Death.DeathCause_DeathCause_Type
			End Get
			Set(ByVal Value As String)
				Me.Death.DeathCause_DeathCause_Type = Value
			End Set
		End Property
		Public Custom Event DeathCause_DeathCause_TypeChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.DeathCause_DeathCause_TypeChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.DeathCause_DeathCause_TypeChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event DeathCause_DeathCause_TypeChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.DeathCause_DeathCause_TypeChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.DeathCause_DeathCause_TypeChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property NaturalDeath() As NaturalDeath
			Get
				Return Me.Death.NaturalDeath
			End Get
			Set(ByVal Value As NaturalDeath)
				Me.Death.NaturalDeath = Value
			End Set
		End Property
		Public Custom Event NaturalDeathChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.NaturalDeathChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.NaturalDeathChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event NaturalDeathChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.NaturalDeathChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.NaturalDeathChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property Person() As Person
			Get
				Return Me.Death.Person
			End Get
			Set(ByVal Value As Person)
				Me.Death.Person = Value
			End Set
		End Property
		Public Custom Event PersonChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.PersonChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.PersonChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event PersonChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.PersonChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.PersonChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property FirstName() As String
			Get
				Return Me.Death.Person.FirstName
			End Get
			Set(ByVal Value As String)
				Me.Death.Person.FirstName = Value
			End Set
		End Property
		Public Custom Event FirstNameChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.FirstNameChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.FirstNameChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event FirstNameChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.FirstNameChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.FirstNameChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property LastName() As String
			Get
				Return Me.Death.Person.LastName
			End Get
			Set(ByVal Value As String)
				Me.Death.Person.LastName = Value
			End Set
		End Property
		Public Custom Event LastNameChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.LastNameChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.LastNameChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event LastNameChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.LastNameChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.LastNameChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property SocialSecurityNumber() As String
			Get
				Return Me.Death.Person.SocialSecurityNumber
			End Get
			Set(ByVal Value As String)
				Me.Death.Person.SocialSecurityNumber = Value
			End Set
		End Property
		Public Custom Event SocialSecurityNumberChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.SocialSecurityNumberChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.SocialSecurityNumberChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event SocialSecurityNumberChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.SocialSecurityNumberChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.SocialSecurityNumberChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property HatType_ColorARGB() As Nullable(Of Integer)
			Get
				Return Me.Death.Person.HatType_ColorARGB
			End Get
			Set(ByVal Value As Nullable(Of Integer))
				Me.Death.Person.HatType_ColorARGB = Value
			End Set
		End Property
		Public Custom Event HatType_ColorARGBChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.HatType_ColorARGBChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.HatType_ColorARGBChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event HatType_ColorARGBChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.HatType_ColorARGBChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.HatType_ColorARGBChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property HatType_HatTypeStyle_HatTypeStyle_Description() As String
			Get
				Return Me.Death.Person.HatType_HatTypeStyle_HatTypeStyle_Description
			End Get
			Set(ByVal Value As String)
				Me.Death.Person.HatType_HatTypeStyle_HatTypeStyle_Description = Value
			End Set
		End Property
		Public Custom Event HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property OwnsCar_vin() As Nullable(Of Integer)
			Get
				Return Me.Death.Person.OwnsCar_vin
			End Get
			Set(ByVal Value As Nullable(Of Integer))
				Me.Death.Person.OwnsCar_vin = Value
			End Set
		End Property
		Public Custom Event OwnsCar_vinChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.OwnsCar_vinChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.OwnsCar_vinChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event OwnsCar_vinChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.OwnsCar_vinChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.OwnsCar_vinChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property Gender_Gender_Code() As String
			Get
				Return Me.Death.Person.Gender_Gender_Code
			End Get
			Set(ByVal Value As String)
				Me.Death.Person.Gender_Gender_Code = Value
			End Set
		End Property
		Public Custom Event Gender_Gender_CodeChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.Gender_Gender_CodeChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.Gender_Gender_CodeChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event Gender_Gender_CodeChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.Gender_Gender_CodeChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.Gender_Gender_CodeChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property PersonHasParents() As Nullable(Of Boolean)
			Get
				Return Me.Death.Person.PersonHasParents
			End Get
			Set(ByVal Value As Nullable(Of Boolean))
				Me.Death.Person.PersonHasParents = Value
			End Set
		End Property
		Public Custom Event PersonHasParentsChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.PersonHasParentsChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.PersonHasParentsChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event PersonHasParentsChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.PersonHasParentsChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.PersonHasParentsChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property ValueType1DoesSomethingElseWith() As ValueType1
			Get
				Return Me.Death.Person.ValueType1DoesSomethingElseWith
			End Get
			Set(ByVal Value As ValueType1)
				Me.Death.Person.ValueType1DoesSomethingElseWith = Value
			End Set
		End Property
		Public Custom Event ValueType1DoesSomethingElseWithChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.ValueType1DoesSomethingElseWithChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.ValueType1DoesSomethingElseWithChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event ValueType1DoesSomethingElseWithChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.ValueType1DoesSomethingElseWithChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.ValueType1DoesSomethingElseWithChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property MalePerson() As MalePerson
			Get
				Return Me.Death.Person.MalePerson
			End Get
			Set(ByVal Value As MalePerson)
				Me.Death.Person.MalePerson = Value
			End Set
		End Property
		Public Custom Event MalePersonChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.MalePersonChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.MalePersonChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event MalePersonChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.MalePersonChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.MalePersonChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property FemalePerson() As FemalePerson
			Get
				Return Me.Death.Person.FemalePerson
			End Get
			Set(ByVal Value As FemalePerson)
				Me.Death.Person.FemalePerson = Value
			End Set
		End Property
		Public Custom Event FemalePersonChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.FemalePersonChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.FemalePersonChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event FemalePersonChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.FemalePersonChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.FemalePersonChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property ChildPerson() As ChildPerson
			Get
				Return Me.Death.Person.ChildPerson
			End Get
			Set(ByVal Value As ChildPerson)
				Me.Death.Person.ChildPerson = Value
			End Set
		End Property
		Public Custom Event ChildPersonChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.ChildPersonChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.ChildPersonChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event ChildPersonChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.ChildPersonChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.ChildPersonChanged, Value
			End RemoveHandler
		End Event
		Public Overridable ReadOnly Property PersonDrivesCarAsDrivenByPerson() As ICollection(Of PersonDrivesCar)
			Get
				Return Me.Death.Person.PersonDrivesCarAsDrivenByPerson
			End Get
		End Property
		Public Overridable ReadOnly Property PersonBoughtCarFromPersonOnDateAsBuyer() As ICollection(Of PersonBoughtCarFromPersonOnDate)
			Get
				Return Me.Death.Person.PersonBoughtCarFromPersonOnDateAsBuyer
			End Get
		End Property
		Public Overridable ReadOnly Property PersonBoughtCarFromPersonOnDateAsSeller() As ICollection(Of PersonBoughtCarFromPersonOnDate)
			Get
				Return Me.Death.Person.PersonBoughtCarFromPersonOnDateAsSeller
			End Get
		End Property
		Public Overridable ReadOnly Property PersonHasNickNameAsPerson() As ICollection(Of PersonHasNickName)
			Get
				Return Me.Death.Person.PersonHasNickNameAsPerson
			End Get
		End Property
		Public Overridable ReadOnly Property Task() As ICollection(Of Task)
			Get
				Return Me.Death.Person.Task
			End Get
		End Property
		Public Overridable ReadOnly Property ValueType1DoesSomethingWith() As ICollection(Of ValueType1)
			Get
				Return Me.Death.Person.ValueType1DoesSomethingWith
			End Get
		End Property
	End Class
	#End Region
	#Region "Task"
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	Public MustInherit Partial Class Task
		Implements INotifyPropertyChanged
		Protected Sub New()
		End Sub
		Private ReadOnly Events As System.Delegate() = New System.Delegate(2) {}
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
		Public MustOverride ReadOnly Property Context() As SampleModelContext
		Public Custom Event PersonChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaisePersonChangingEvent(ByVal newValue As Person) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Person)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangingEventArgs(Of Person)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Person) = New PropertyChangingEventArgs(Of Person)(Me.Person, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event PersonChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaisePersonChangedEvent(ByVal oldValue As Person)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Person)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangedEventArgs(Of Person)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Person)(oldValue, Me.Person), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Person")
			End If
		End Sub
		Public MustOverride Property Person() As Person
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Overridable Function ToString(ByVal provider As IFormatProvider) As String
			Return String.Format(provider, "Task{0}{{{0}{1}Person = {2}{0}}}", Environment.NewLine, "", "TODO: Recursively call ToString for customTypes...")
		End Function
	End Class
	#End Region
	#Region "ValueType1"
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	Public MustInherit Partial Class ValueType1
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
		Public MustOverride ReadOnly Property Context() As SampleModelContext
		Public Custom Event ValueType1ValueChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseValueType1ValueChangingEvent(ByVal newValue As Integer) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Integer)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangingEventArgs(Of Integer)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Integer) = New PropertyChangingEventArgs(Of Integer)(Me.ValueType1Value, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event ValueType1ValueChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseValueType1ValueChangedEvent(ByVal oldValue As Integer)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Integer)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangedEventArgs(Of Integer)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Integer)(oldValue, Me.ValueType1Value), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("ValueType1Value")
			End If
		End Sub
		Public Custom Event DoesSomethingWithPersonChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Function RaiseDoesSomethingWithPersonChangingEvent(ByVal newValue As Person) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Person)) = TryCast(Me.Events(2), EventHandler(Of PropertyChangingEventArgs(Of Person)))
			If eventHandler IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Person) = New PropertyChangingEventArgs(Of Person)(Me.DoesSomethingWithPerson, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event DoesSomethingWithPersonChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030")> _
		Protected Sub RaiseDoesSomethingWithPersonChangedEvent(ByVal oldValue As Person)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Person)) = TryCast(Me.Events(2), EventHandler(Of PropertyChangedEventArgs(Of Person)))
			If eventHandler IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Person)(oldValue, Me.DoesSomethingWithPerson), New System.AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("DoesSomethingWithPerson")
			End If
		End Sub
		Public MustOverride Property ValueType1Value() As Integer
		Public MustOverride Property DoesSomethingWithPerson() As Person
		Public MustOverride ReadOnly Property DoesSomethingElseWithPerson() As ICollection(Of Person)
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Overridable Function ToString(ByVal provider As IFormatProvider) As String
			Return String.Format(provider, "ValueType1{0}{{{0}{1}ValueType1Value = ""{2}"",{0}{1}DoesSomethingWithPerson = {3}{0}}}", Environment.NewLine, "", Me.ValueType1Value, "TODO: Recursively call ToString for customTypes...")
		End Function
	End Class
	#End Region
	#Region "ISampleModelContext"
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	Public Interface ISampleModelContext
		ReadOnly Property IsDeserializing() As Boolean
		Function GetPersonDrivesCarByInternalUniquenessConstraint18(ByVal DrivesCar_vin As Integer, ByVal DrivenByPerson As Person) As PersonDrivesCar
		Function GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint23(ByVal Buyer As Person, ByVal CarSold_vin As Integer, ByVal Seller As Person) As PersonBoughtCarFromPersonOnDate
		Function GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint24(ByVal SaleDate_YMD As Integer, ByVal Seller As Person, ByVal CarSold_vin As Integer) As PersonBoughtCarFromPersonOnDate
		Function GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint25(ByVal CarSold_vin As Integer, ByVal SaleDate_YMD As Integer, ByVal Buyer As Person) As PersonBoughtCarFromPersonOnDate
		Function GetReviewByInternalUniquenessConstraint26(ByVal Car_vin As Integer, ByVal Criteria_Name As String) As Review
		Function GetPersonHasNickNameByInternalUniquenessConstraint33(ByVal NickName As String, ByVal Person As Person) As PersonHasNickName
		Function GetChildPersonByExternalUniquenessConstraint3(ByVal Father As MalePerson, ByVal BirthOrder_BirthOrder_Nr As Integer, ByVal Mother As FemalePerson) As ChildPerson
		Function GetPersonByExternalUniquenessConstraint1(ByVal FirstName As String, ByVal Date_YMD As Integer) As Person
		Function GetPersonByExternalUniquenessConstraint2(ByVal LastName As String, ByVal Date_YMD As Integer) As Person
		Function GetPersonBySocialSecurityNumber(ByVal SocialSecurityNumber As String) As Person
		Function GetPersonByOwnsCar_vin(ByVal OwnsCar_vin As Nullable(Of Integer)) As Person
		Function GetValueType1ByValueType1Value(ByVal ValueType1Value As Integer) As ValueType1
		Function CreatePersonDrivesCar(ByVal DrivesCar_vin As Integer, ByVal DrivenByPerson As Person) As PersonDrivesCar
		ReadOnly Property PersonDrivesCarCollection() As ReadOnlyCollection(Of PersonDrivesCar)
		Function CreatePersonBoughtCarFromPersonOnDate(ByVal CarSold_vin As Integer, ByVal SaleDate_YMD As Integer, ByVal Buyer As Person, ByVal Seller As Person) As PersonBoughtCarFromPersonOnDate
		ReadOnly Property PersonBoughtCarFromPersonOnDateCollection() As ReadOnlyCollection(Of PersonBoughtCarFromPersonOnDate)
		Function CreateReview(ByVal Car_vin As Integer, ByVal Rating_Nr_Integer As Integer, ByVal Criteria_Name As String) As Review
		ReadOnly Property ReviewCollection() As ReadOnlyCollection(Of Review)
		Function CreatePersonHasNickName(ByVal NickName As String, ByVal Person As Person) As PersonHasNickName
		ReadOnly Property PersonHasNickNameCollection() As ReadOnlyCollection(Of PersonHasNickName)
		Function CreatePerson(ByVal FirstName As String, ByVal Date_YMD As Integer, ByVal LastName As String, ByVal Gender_Gender_Code As String) As Person
		ReadOnly Property PersonCollection() As ReadOnlyCollection(Of Person)
		Function CreateMalePerson(ByVal Person As Person) As MalePerson
		ReadOnly Property MalePersonCollection() As ReadOnlyCollection(Of MalePerson)
		Function CreateFemalePerson(ByVal Person As Person) As FemalePerson
		ReadOnly Property FemalePersonCollection() As ReadOnlyCollection(Of FemalePerson)
		Function CreateChildPerson(ByVal BirthOrder_BirthOrder_Nr As Integer, ByVal Father As MalePerson, ByVal Mother As FemalePerson, ByVal Person As Person) As ChildPerson
		ReadOnly Property ChildPersonCollection() As ReadOnlyCollection(Of ChildPerson)
		Function CreateDeath(ByVal DeathCause_DeathCause_Type As String, ByVal Person As Person) As Death
		ReadOnly Property DeathCollection() As ReadOnlyCollection(Of Death)
		Function CreateNaturalDeath(ByVal Death As Death) As NaturalDeath
		ReadOnly Property NaturalDeathCollection() As ReadOnlyCollection(Of NaturalDeath)
		Function CreateUnnaturalDeath(ByVal Death As Death) As UnnaturalDeath
		ReadOnly Property UnnaturalDeathCollection() As ReadOnlyCollection(Of UnnaturalDeath)
		Function CreateTask() As Task
		ReadOnly Property TaskCollection() As ReadOnlyCollection(Of Task)
		Function CreateValueType1(ByVal ValueType1Value As Integer) As ValueType1
		ReadOnly Property ValueType1Collection() As ReadOnlyCollection(Of ValueType1)
	End Interface
	#End Region
	#Region "SampleModelContext"
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	Public NotInheritable Class SampleModelContext
		Implements ISampleModelContext
		Public Sub New()
			Dim PersonDrivesCarList As List(Of PersonDrivesCar) = New List(Of PersonDrivesCar)()
			Me.myPersonDrivesCarList = PersonDrivesCarList
			Me.myPersonDrivesCarReadOnlyCollection = New ReadOnlyCollection(Of PersonDrivesCar)(PersonDrivesCarList)
			Dim PersonBoughtCarFromPersonOnDateList As List(Of PersonBoughtCarFromPersonOnDate) = New List(Of PersonBoughtCarFromPersonOnDate)()
			Me.myPersonBoughtCarFromPersonOnDateList = PersonBoughtCarFromPersonOnDateList
			Me.myPersonBoughtCarFromPersonOnDateReadOnlyCollection = New ReadOnlyCollection(Of PersonBoughtCarFromPersonOnDate)(PersonBoughtCarFromPersonOnDateList)
			Dim ReviewList As List(Of Review) = New List(Of Review)()
			Me.myReviewList = ReviewList
			Me.myReviewReadOnlyCollection = New ReadOnlyCollection(Of Review)(ReviewList)
			Dim PersonHasNickNameList As List(Of PersonHasNickName) = New List(Of PersonHasNickName)()
			Me.myPersonHasNickNameList = PersonHasNickNameList
			Me.myPersonHasNickNameReadOnlyCollection = New ReadOnlyCollection(Of PersonHasNickName)(PersonHasNickNameList)
			Dim PersonList As List(Of Person) = New List(Of Person)()
			Me.myPersonList = PersonList
			Me.myPersonReadOnlyCollection = New ReadOnlyCollection(Of Person)(PersonList)
			Dim MalePersonList As List(Of MalePerson) = New List(Of MalePerson)()
			Me.myMalePersonList = MalePersonList
			Me.myMalePersonReadOnlyCollection = New ReadOnlyCollection(Of MalePerson)(MalePersonList)
			Dim FemalePersonList As List(Of FemalePerson) = New List(Of FemalePerson)()
			Me.myFemalePersonList = FemalePersonList
			Me.myFemalePersonReadOnlyCollection = New ReadOnlyCollection(Of FemalePerson)(FemalePersonList)
			Dim ChildPersonList As List(Of ChildPerson) = New List(Of ChildPerson)()
			Me.myChildPersonList = ChildPersonList
			Me.myChildPersonReadOnlyCollection = New ReadOnlyCollection(Of ChildPerson)(ChildPersonList)
			Dim DeathList As List(Of Death) = New List(Of Death)()
			Me.myDeathList = DeathList
			Me.myDeathReadOnlyCollection = New ReadOnlyCollection(Of Death)(DeathList)
			Dim NaturalDeathList As List(Of NaturalDeath) = New List(Of NaturalDeath)()
			Me.myNaturalDeathList = NaturalDeathList
			Me.myNaturalDeathReadOnlyCollection = New ReadOnlyCollection(Of NaturalDeath)(NaturalDeathList)
			Dim UnnaturalDeathList As List(Of UnnaturalDeath) = New List(Of UnnaturalDeath)()
			Me.myUnnaturalDeathList = UnnaturalDeathList
			Me.myUnnaturalDeathReadOnlyCollection = New ReadOnlyCollection(Of UnnaturalDeath)(UnnaturalDeathList)
			Dim TaskList As List(Of Task) = New List(Of Task)()
			Me.myTaskList = TaskList
			Me.myTaskReadOnlyCollection = New ReadOnlyCollection(Of Task)(TaskList)
			Dim ValueType1List As List(Of ValueType1) = New List(Of ValueType1)()
			Me.myValueType1List = ValueType1List
			Me.myValueType1ReadOnlyCollection = New ReadOnlyCollection(Of ValueType1)(ValueType1List)
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
			ISampleModelContext.IsDeserializing
			Get
				Return Me.myIsDeserializing
			End Get
		End Property
		Private ReadOnly myInternalUniquenessConstraint18Dictionary As Dictionary(Of Tuple(Of Integer, Person), PersonDrivesCar) = New Dictionary(Of Tuple(Of Integer, Person), PersonDrivesCar)()
		Public Function GetPersonDrivesCarByInternalUniquenessConstraint18(ByVal DrivesCar_vin As Integer, ByVal DrivenByPerson As Person) As PersonDrivesCar Implements _
			ISampleModelContext.GetPersonDrivesCarByInternalUniquenessConstraint18
			Return Me.myInternalUniquenessConstraint18Dictionary(Tuple.CreateTuple(Of Integer, Person)(DrivesCar_vin, DrivenByPerson))
		End Function
		Private Function OnInternalUniquenessConstraint18Changing(ByVal instance As PersonDrivesCar, ByVal newValue As Tuple(Of Integer, Person)) As Boolean
			If newValue IsNot Nothing Then
				Dim currentInstance As PersonDrivesCar = instance
				If Me.myInternalUniquenessConstraint18Dictionary.TryGetValue(newValue, currentInstance) Then
					Return currentInstance Is instance
				End If
			End If
			Return True
		End Function
		Private Sub OnInternalUniquenessConstraint18Changed(ByVal instance As PersonDrivesCar, ByVal oldValue As Tuple(Of Integer, Person), ByVal newValue As Tuple(Of Integer, Person))
			If oldValue IsNot Nothing Then
				Me.myInternalUniquenessConstraint18Dictionary.Remove(oldValue)
			End If
			If newValue IsNot Nothing Then
				Me.myInternalUniquenessConstraint18Dictionary.Add(newValue, instance)
			End If
		End Sub
		Private ReadOnly myInternalUniquenessConstraint23Dictionary As Dictionary(Of Tuple(Of Person, Integer, Person), PersonBoughtCarFromPersonOnDate) = New Dictionary(Of Tuple(Of Person, Integer, Person), PersonBoughtCarFromPersonOnDate)()
		Public Function GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint23(ByVal Buyer As Person, ByVal CarSold_vin As Integer, ByVal Seller As Person) As PersonBoughtCarFromPersonOnDate Implements _
			ISampleModelContext.GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint23
			Return Me.myInternalUniquenessConstraint23Dictionary(Tuple.CreateTuple(Of Person, Integer, Person)(Buyer, CarSold_vin, Seller))
		End Function
		Private Function OnInternalUniquenessConstraint23Changing(ByVal instance As PersonBoughtCarFromPersonOnDate, ByVal newValue As Tuple(Of Person, Integer, Person)) As Boolean
			If newValue IsNot Nothing Then
				Dim currentInstance As PersonBoughtCarFromPersonOnDate = instance
				If Me.myInternalUniquenessConstraint23Dictionary.TryGetValue(newValue, currentInstance) Then
					Return currentInstance Is instance
				End If
			End If
			Return True
		End Function
		Private Sub OnInternalUniquenessConstraint23Changed(ByVal instance As PersonBoughtCarFromPersonOnDate, ByVal oldValue As Tuple(Of Person, Integer, Person), ByVal newValue As Tuple(Of Person, Integer, Person))
			If oldValue IsNot Nothing Then
				Me.myInternalUniquenessConstraint23Dictionary.Remove(oldValue)
			End If
			If newValue IsNot Nothing Then
				Me.myInternalUniquenessConstraint23Dictionary.Add(newValue, instance)
			End If
		End Sub
		Private ReadOnly myInternalUniquenessConstraint24Dictionary As Dictionary(Of Tuple(Of Integer, Person, Integer), PersonBoughtCarFromPersonOnDate) = New Dictionary(Of Tuple(Of Integer, Person, Integer), PersonBoughtCarFromPersonOnDate)()
		Public Function GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint24(ByVal SaleDate_YMD As Integer, ByVal Seller As Person, ByVal CarSold_vin As Integer) As PersonBoughtCarFromPersonOnDate Implements _
			ISampleModelContext.GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint24
			Return Me.myInternalUniquenessConstraint24Dictionary(Tuple.CreateTuple(Of Integer, Person, Integer)(SaleDate_YMD, Seller, CarSold_vin))
		End Function
		Private Function OnInternalUniquenessConstraint24Changing(ByVal instance As PersonBoughtCarFromPersonOnDate, ByVal newValue As Tuple(Of Integer, Person, Integer)) As Boolean
			If newValue IsNot Nothing Then
				Dim currentInstance As PersonBoughtCarFromPersonOnDate = instance
				If Me.myInternalUniquenessConstraint24Dictionary.TryGetValue(newValue, currentInstance) Then
					Return currentInstance Is instance
				End If
			End If
			Return True
		End Function
		Private Sub OnInternalUniquenessConstraint24Changed(ByVal instance As PersonBoughtCarFromPersonOnDate, ByVal oldValue As Tuple(Of Integer, Person, Integer), ByVal newValue As Tuple(Of Integer, Person, Integer))
			If oldValue IsNot Nothing Then
				Me.myInternalUniquenessConstraint24Dictionary.Remove(oldValue)
			End If
			If newValue IsNot Nothing Then
				Me.myInternalUniquenessConstraint24Dictionary.Add(newValue, instance)
			End If
		End Sub
		Private ReadOnly myInternalUniquenessConstraint25Dictionary As Dictionary(Of Tuple(Of Integer, Integer, Person), PersonBoughtCarFromPersonOnDate) = New Dictionary(Of Tuple(Of Integer, Integer, Person), PersonBoughtCarFromPersonOnDate)()
		Public Function GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint25(ByVal CarSold_vin As Integer, ByVal SaleDate_YMD As Integer, ByVal Buyer As Person) As PersonBoughtCarFromPersonOnDate Implements _
			ISampleModelContext.GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint25
			Return Me.myInternalUniquenessConstraint25Dictionary(Tuple.CreateTuple(Of Integer, Integer, Person)(CarSold_vin, SaleDate_YMD, Buyer))
		End Function
		Private Function OnInternalUniquenessConstraint25Changing(ByVal instance As PersonBoughtCarFromPersonOnDate, ByVal newValue As Tuple(Of Integer, Integer, Person)) As Boolean
			If newValue IsNot Nothing Then
				Dim currentInstance As PersonBoughtCarFromPersonOnDate = instance
				If Me.myInternalUniquenessConstraint25Dictionary.TryGetValue(newValue, currentInstance) Then
					Return currentInstance Is instance
				End If
			End If
			Return True
		End Function
		Private Sub OnInternalUniquenessConstraint25Changed(ByVal instance As PersonBoughtCarFromPersonOnDate, ByVal oldValue As Tuple(Of Integer, Integer, Person), ByVal newValue As Tuple(Of Integer, Integer, Person))
			If oldValue IsNot Nothing Then
				Me.myInternalUniquenessConstraint25Dictionary.Remove(oldValue)
			End If
			If newValue IsNot Nothing Then
				Me.myInternalUniquenessConstraint25Dictionary.Add(newValue, instance)
			End If
		End Sub
		Private ReadOnly myInternalUniquenessConstraint26Dictionary As Dictionary(Of Tuple(Of Integer, String), Review) = New Dictionary(Of Tuple(Of Integer, String), Review)()
		Public Function GetReviewByInternalUniquenessConstraint26(ByVal Car_vin As Integer, ByVal Criteria_Name As String) As Review Implements _
			ISampleModelContext.GetReviewByInternalUniquenessConstraint26
			Return Me.myInternalUniquenessConstraint26Dictionary(Tuple.CreateTuple(Of Integer, String)(Car_vin, Criteria_Name))
		End Function
		Private Function OnInternalUniquenessConstraint26Changing(ByVal instance As Review, ByVal newValue As Tuple(Of Integer, String)) As Boolean
			If newValue IsNot Nothing Then
				Dim currentInstance As Review = instance
				If Me.myInternalUniquenessConstraint26Dictionary.TryGetValue(newValue, currentInstance) Then
					Return currentInstance Is instance
				End If
			End If
			Return True
		End Function
		Private Sub OnInternalUniquenessConstraint26Changed(ByVal instance As Review, ByVal oldValue As Tuple(Of Integer, String), ByVal newValue As Tuple(Of Integer, String))
			If oldValue IsNot Nothing Then
				Me.myInternalUniquenessConstraint26Dictionary.Remove(oldValue)
			End If
			If newValue IsNot Nothing Then
				Me.myInternalUniquenessConstraint26Dictionary.Add(newValue, instance)
			End If
		End Sub
		Private ReadOnly myInternalUniquenessConstraint33Dictionary As Dictionary(Of Tuple(Of String, Person), PersonHasNickName) = New Dictionary(Of Tuple(Of String, Person), PersonHasNickName)()
		Public Function GetPersonHasNickNameByInternalUniquenessConstraint33(ByVal NickName As String, ByVal Person As Person) As PersonHasNickName Implements _
			ISampleModelContext.GetPersonHasNickNameByInternalUniquenessConstraint33
			Return Me.myInternalUniquenessConstraint33Dictionary(Tuple.CreateTuple(Of String, Person)(NickName, Person))
		End Function
		Private Function OnInternalUniquenessConstraint33Changing(ByVal instance As PersonHasNickName, ByVal newValue As Tuple(Of String, Person)) As Boolean
			If newValue IsNot Nothing Then
				Dim currentInstance As PersonHasNickName = instance
				If Me.myInternalUniquenessConstraint33Dictionary.TryGetValue(newValue, currentInstance) Then
					Return currentInstance Is instance
				End If
			End If
			Return True
		End Function
		Private Sub OnInternalUniquenessConstraint33Changed(ByVal instance As PersonHasNickName, ByVal oldValue As Tuple(Of String, Person), ByVal newValue As Tuple(Of String, Person))
			If oldValue IsNot Nothing Then
				Me.myInternalUniquenessConstraint33Dictionary.Remove(oldValue)
			End If
			If newValue IsNot Nothing Then
				Me.myInternalUniquenessConstraint33Dictionary.Add(newValue, instance)
			End If
		End Sub
		Private ReadOnly myExternalUniquenessConstraint3Dictionary As Dictionary(Of Tuple(Of MalePerson, Integer, FemalePerson), ChildPerson) = New Dictionary(Of Tuple(Of MalePerson, Integer, FemalePerson), ChildPerson)()
		Public Function GetChildPersonByExternalUniquenessConstraint3(ByVal Father As MalePerson, ByVal BirthOrder_BirthOrder_Nr As Integer, ByVal Mother As FemalePerson) As ChildPerson Implements _
			ISampleModelContext.GetChildPersonByExternalUniquenessConstraint3
			Return Me.myExternalUniquenessConstraint3Dictionary(Tuple.CreateTuple(Of MalePerson, Integer, FemalePerson)(Father, BirthOrder_BirthOrder_Nr, Mother))
		End Function
		Private Function OnExternalUniquenessConstraint3Changing(ByVal instance As ChildPerson, ByVal newValue As Tuple(Of MalePerson, Integer, FemalePerson)) As Boolean
			If newValue IsNot Nothing Then
				Dim currentInstance As ChildPerson = instance
				If Me.myExternalUniquenessConstraint3Dictionary.TryGetValue(newValue, currentInstance) Then
					Return currentInstance Is instance
				End If
			End If
			Return True
		End Function
		Private Sub OnExternalUniquenessConstraint3Changed(ByVal instance As ChildPerson, ByVal oldValue As Tuple(Of MalePerson, Integer, FemalePerson), ByVal newValue As Tuple(Of MalePerson, Integer, FemalePerson))
			If oldValue IsNot Nothing Then
				Me.myExternalUniquenessConstraint3Dictionary.Remove(oldValue)
			End If
			If newValue IsNot Nothing Then
				Me.myExternalUniquenessConstraint3Dictionary.Add(newValue, instance)
			End If
		End Sub
		Private ReadOnly myExternalUniquenessConstraint1Dictionary As Dictionary(Of Tuple(Of String, Integer), Person) = New Dictionary(Of Tuple(Of String, Integer), Person)()
		Public Function GetPersonByExternalUniquenessConstraint1(ByVal FirstName As String, ByVal Date_YMD As Integer) As Person Implements _
			ISampleModelContext.GetPersonByExternalUniquenessConstraint1
			Return Me.myExternalUniquenessConstraint1Dictionary(Tuple.CreateTuple(Of String, Integer)(FirstName, Date_YMD))
		End Function
		Private Function OnExternalUniquenessConstraint1Changing(ByVal instance As Person, ByVal newValue As Tuple(Of String, Integer)) As Boolean
			If newValue IsNot Nothing Then
				Dim currentInstance As Person = instance
				If Me.myExternalUniquenessConstraint1Dictionary.TryGetValue(newValue, currentInstance) Then
					Return currentInstance Is instance
				End If
			End If
			Return True
		End Function
		Private Sub OnExternalUniquenessConstraint1Changed(ByVal instance As Person, ByVal oldValue As Tuple(Of String, Integer), ByVal newValue As Tuple(Of String, Integer))
			If oldValue IsNot Nothing Then
				Me.myExternalUniquenessConstraint1Dictionary.Remove(oldValue)
			End If
			If newValue IsNot Nothing Then
				Me.myExternalUniquenessConstraint1Dictionary.Add(newValue, instance)
			End If
		End Sub
		Private ReadOnly myExternalUniquenessConstraint2Dictionary As Dictionary(Of Tuple(Of String, Integer), Person) = New Dictionary(Of Tuple(Of String, Integer), Person)()
		Public Function GetPersonByExternalUniquenessConstraint2(ByVal LastName As String, ByVal Date_YMD As Integer) As Person Implements _
			ISampleModelContext.GetPersonByExternalUniquenessConstraint2
			Return Me.myExternalUniquenessConstraint2Dictionary(Tuple.CreateTuple(Of String, Integer)(LastName, Date_YMD))
		End Function
		Private Function OnExternalUniquenessConstraint2Changing(ByVal instance As Person, ByVal newValue As Tuple(Of String, Integer)) As Boolean
			If newValue IsNot Nothing Then
				Dim currentInstance As Person = instance
				If Me.myExternalUniquenessConstraint2Dictionary.TryGetValue(newValue, currentInstance) Then
					Return currentInstance Is instance
				End If
			End If
			Return True
		End Function
		Private Sub OnExternalUniquenessConstraint2Changed(ByVal instance As Person, ByVal oldValue As Tuple(Of String, Integer), ByVal newValue As Tuple(Of String, Integer))
			If oldValue IsNot Nothing Then
				Me.myExternalUniquenessConstraint2Dictionary.Remove(oldValue)
			End If
			If newValue IsNot Nothing Then
				Me.myExternalUniquenessConstraint2Dictionary.Add(newValue, instance)
			End If
		End Sub
		Private ReadOnly myPersonSocialSecurityNumberDictionary As Dictionary(Of String, Person) = New Dictionary(Of String, Person)()
		Public Function GetPersonBySocialSecurityNumber(ByVal SocialSecurityNumber As String) As Person Implements _
			ISampleModelContext.GetPersonBySocialSecurityNumber
			Return Me.myPersonSocialSecurityNumberDictionary(SocialSecurityNumber)
		End Function
		Private ReadOnly myPersonOwnsCar_vinDictionary As Dictionary(Of Nullable(Of Integer), Person) = New Dictionary(Of Nullable(Of Integer), Person)()
		Public Function GetPersonByOwnsCar_vin(ByVal OwnsCar_vin As Nullable(Of Integer)) As Person Implements _
			ISampleModelContext.GetPersonByOwnsCar_vin
			Return Me.myPersonOwnsCar_vinDictionary(OwnsCar_vin)
		End Function
		Private ReadOnly myValueType1ValueType1ValueDictionary As Dictionary(Of Integer, ValueType1) = New Dictionary(Of Integer, ValueType1)()
		Public Function GetValueType1ByValueType1Value(ByVal ValueType1Value As Integer) As ValueType1 Implements _
			ISampleModelContext.GetValueType1ByValueType1Value
			Return Me.myValueType1ValueType1ValueDictionary(ValueType1Value)
		End Function
		Private Function OnPersonDrivesCarDrivesCar_vinChanging(ByVal instance As PersonDrivesCar, ByVal newValue As Integer) As Boolean
			If instance IsNot Nothing Then
				If Not (Me.OnInternalUniquenessConstraint18Changing(instance, Tuple.CreateTuple(Of Integer, Person)(newValue, instance.DrivenByPerson))) Then
					Return False
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnPersonDrivesCarDrivesCar_vinChanged(ByVal instance As PersonDrivesCar, ByVal oldValue As Nullable(Of Integer))
			Dim InternalUniquenessConstraint18OldValueTuple As Tuple(Of Integer, Person)
			If oldValue IsNot Nothing Then
				InternalUniquenessConstraint18OldValueTuple = Tuple.CreateTuple(Of Integer, Person)(oldValue.Value, instance.DrivenByPerson)
			Else
				InternalUniquenessConstraint18OldValueTuple = Nothing
			End If
			Me.OnInternalUniquenessConstraint18Changed(instance, InternalUniquenessConstraint18OldValueTuple, Tuple.CreateTuple(Of Integer, Person)(instance.DrivesCar_vin, instance.DrivenByPerson))
		End Sub
		<SuppressMessageAttribute("Microsoft.Usage", "CA2208")> _
		<SuppressMessageAttribute("Microsoft.Globalization", "CA1303")> _
		Private Function OnPersonDrivesCarDrivenByPersonChanging(ByVal instance As PersonDrivesCar, ByVal newValue As Person) As Boolean
			If Me IsNot newValue.Context Then
				Throw New ArgumentException("All objects in a relationship must be part of the same Context.", "value")
			End If
			If instance IsNot Nothing Then
				If Not (Me.OnInternalUniquenessConstraint18Changing(instance, Tuple.CreateTuple(Of Integer, Person)(instance.DrivesCar_vin, newValue))) Then
					Return False
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnPersonDrivesCarDrivenByPersonChanged(ByVal instance As PersonDrivesCar, ByVal oldValue As Person)
			instance.DrivenByPerson.PersonDrivesCarAsDrivenByPerson.Add(instance)
			Dim InternalUniquenessConstraint18OldValueTuple As Tuple(Of Integer, Person)
			If oldValue IsNot Nothing Then
				oldValue.PersonDrivesCarAsDrivenByPerson.Remove(instance)
				InternalUniquenessConstraint18OldValueTuple = Tuple.CreateTuple(Of Integer, Person)(instance.DrivesCar_vin, oldValue)
			Else
				InternalUniquenessConstraint18OldValueTuple = Nothing
			End If
			Me.OnInternalUniquenessConstraint18Changed(instance, InternalUniquenessConstraint18OldValueTuple, Tuple.CreateTuple(Of Integer, Person)(instance.DrivesCar_vin, instance.DrivenByPerson))
		End Sub
		Public Function CreatePersonDrivesCar(ByVal DrivesCar_vin As Integer, ByVal DrivenByPerson As Person) As PersonDrivesCar
			If Not (Me.IsDeserializing) Then
				If Not (Me.OnPersonDrivesCarDrivesCar_vinChanging(Nothing, DrivesCar_vin)) Then
					Throw New ArgumentException("Argument failed constraint enforcement.", "DrivesCar_vin")
				End If
				If Not (Me.OnPersonDrivesCarDrivenByPersonChanging(Nothing, DrivenByPerson)) Then
					Throw New ArgumentException("Argument failed constraint enforcement.", "DrivenByPerson")
				End If
			End If
			Return New PersonDrivesCarCore(Me, DrivesCar_vin, DrivenByPerson)
		End Function
		Private ReadOnly myPersonDrivesCarList As List(Of PersonDrivesCar)
		Private ReadOnly myPersonDrivesCarReadOnlyCollection As ReadOnlyCollection(Of PersonDrivesCar)
		Public ReadOnly Property PersonDrivesCarCollection() As ReadOnlyCollection(Of PersonDrivesCar) Implements _
			ISampleModelContext.PersonDrivesCarCollection
			Get
				Return Me.myPersonDrivesCarReadOnlyCollection
			End Get
		End Property
		#Region "PersonDrivesCarCore"
		Private NotInheritable Class PersonDrivesCarCore
			Inherits PersonDrivesCar
			Public Sub New(ByVal context As SampleModelContext, ByVal DrivesCar_vin As Integer, ByVal DrivenByPerson As Person)
				Me.myContext = context
				Me.myDrivesCar_vin = DrivesCar_vin
				context.OnPersonDrivesCarDrivesCar_vinChanged(Me, Nothing)
				Me.myDrivenByPerson = DrivenByPerson
				context.OnPersonDrivesCarDrivenByPersonChanged(Me, Nothing)
				context.myPersonDrivesCarList.Add(Me)
			End Sub
			Private ReadOnly myContext As SampleModelContext
			Public Overrides ReadOnly Property Context() As SampleModelContext
				Get
					Return Me.myContext
				End Get
			End Property
			Private myDrivesCar_vin As Integer
			Public Overrides Property DrivesCar_vin() As Integer
				Get
					Return Me.myDrivesCar_vin
				End Get
				Set(ByVal Value As Integer)
					If Not (Object.Equals(Me.DrivesCar_vin, Value)) Then
						If Me.Context.OnPersonDrivesCarDrivesCar_vinChanging(Me, Value) Then
							If MyBase.RaiseDrivesCar_vinChangingEvent(Value) Then
								Dim oldValue As Integer = Me.DrivesCar_vin
								Me.myDrivesCar_vin = Value
								Me.Context.OnPersonDrivesCarDrivesCar_vinChanged(Me, oldValue)
								MyBase.RaiseDrivesCar_vinChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private myDrivenByPerson As Person
			Public Overrides Property DrivenByPerson() As Person
				Get
					Return Me.myDrivenByPerson
				End Get
				Set(ByVal Value As Person)
					If Value Is Nothing Then
						Return
					End If
					If Not (Object.Equals(Me.DrivenByPerson, Value)) Then
						If Me.Context.OnPersonDrivesCarDrivenByPersonChanging(Me, Value) Then
							If MyBase.RaiseDrivenByPersonChangingEvent(Value) Then
								Dim oldValue As Person = Me.DrivenByPerson
								Me.myDrivenByPerson = Value
								Me.Context.OnPersonDrivesCarDrivenByPersonChanged(Me, oldValue)
								MyBase.RaiseDrivenByPersonChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
		End Class
		#End Region
		Private Function OnPersonBoughtCarFromPersonOnDateCarSold_vinChanging(ByVal instance As PersonBoughtCarFromPersonOnDate, ByVal newValue As Integer) As Boolean
			If instance IsNot Nothing Then
				If Not (Me.OnInternalUniquenessConstraint23Changing(instance, Tuple.CreateTuple(Of Person, Integer, Person)(instance.Buyer, newValue, instance.Seller))) Then
					Return False
				End If
				If Not (Me.OnInternalUniquenessConstraint24Changing(instance, Tuple.CreateTuple(Of Integer, Person, Integer)(instance.SaleDate_YMD, instance.Seller, newValue))) Then
					Return False
				End If
				If Not (Me.OnInternalUniquenessConstraint25Changing(instance, Tuple.CreateTuple(Of Integer, Integer, Person)(newValue, instance.SaleDate_YMD, instance.Buyer))) Then
					Return False
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnPersonBoughtCarFromPersonOnDateCarSold_vinChanged(ByVal instance As PersonBoughtCarFromPersonOnDate, ByVal oldValue As Nullable(Of Integer))
			Dim InternalUniquenessConstraint23OldValueTuple As Tuple(Of Person, Integer, Person)
			Dim InternalUniquenessConstraint24OldValueTuple As Tuple(Of Integer, Person, Integer)
			Dim InternalUniquenessConstraint25OldValueTuple As Tuple(Of Integer, Integer, Person)
			If oldValue IsNot Nothing Then
				InternalUniquenessConstraint23OldValueTuple = Tuple.CreateTuple(Of Person, Integer, Person)(instance.Buyer, oldValue.Value, instance.Seller)
				InternalUniquenessConstraint24OldValueTuple = Tuple.CreateTuple(Of Integer, Person, Integer)(instance.SaleDate_YMD, instance.Seller, oldValue.Value)
				InternalUniquenessConstraint25OldValueTuple = Tuple.CreateTuple(Of Integer, Integer, Person)(oldValue.Value, instance.SaleDate_YMD, instance.Buyer)
			Else
				InternalUniquenessConstraint23OldValueTuple = Nothing
				InternalUniquenessConstraint24OldValueTuple = Nothing
				InternalUniquenessConstraint25OldValueTuple = Nothing
			End If
			Me.OnInternalUniquenessConstraint23Changed(instance, InternalUniquenessConstraint23OldValueTuple, Tuple.CreateTuple(Of Person, Integer, Person)(instance.Buyer, instance.CarSold_vin, instance.Seller))
			Me.OnInternalUniquenessConstraint24Changed(instance, InternalUniquenessConstraint24OldValueTuple, Tuple.CreateTuple(Of Integer, Person, Integer)(instance.SaleDate_YMD, instance.Seller, instance.CarSold_vin))
			Me.OnInternalUniquenessConstraint25Changed(instance, InternalUniquenessConstraint25OldValueTuple, Tuple.CreateTuple(Of Integer, Integer, Person)(instance.CarSold_vin, instance.SaleDate_YMD, instance.Buyer))
		End Sub
		Private Function OnPersonBoughtCarFromPersonOnDateSaleDate_YMDChanging(ByVal instance As PersonBoughtCarFromPersonOnDate, ByVal newValue As Integer) As Boolean
			If instance IsNot Nothing Then
				If Not (Me.OnInternalUniquenessConstraint24Changing(instance, Tuple.CreateTuple(Of Integer, Person, Integer)(newValue, instance.Seller, instance.CarSold_vin))) Then
					Return False
				End If
				If Not (Me.OnInternalUniquenessConstraint25Changing(instance, Tuple.CreateTuple(Of Integer, Integer, Person)(instance.CarSold_vin, newValue, instance.Buyer))) Then
					Return False
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnPersonBoughtCarFromPersonOnDateSaleDate_YMDChanged(ByVal instance As PersonBoughtCarFromPersonOnDate, ByVal oldValue As Nullable(Of Integer))
			Dim InternalUniquenessConstraint24OldValueTuple As Tuple(Of Integer, Person, Integer)
			Dim InternalUniquenessConstraint25OldValueTuple As Tuple(Of Integer, Integer, Person)
			If oldValue IsNot Nothing Then
				InternalUniquenessConstraint24OldValueTuple = Tuple.CreateTuple(Of Integer, Person, Integer)(oldValue.Value, instance.Seller, instance.CarSold_vin)
				InternalUniquenessConstraint25OldValueTuple = Tuple.CreateTuple(Of Integer, Integer, Person)(instance.CarSold_vin, oldValue.Value, instance.Buyer)
			Else
				InternalUniquenessConstraint24OldValueTuple = Nothing
				InternalUniquenessConstraint25OldValueTuple = Nothing
			End If
			Me.OnInternalUniquenessConstraint24Changed(instance, InternalUniquenessConstraint24OldValueTuple, Tuple.CreateTuple(Of Integer, Person, Integer)(instance.SaleDate_YMD, instance.Seller, instance.CarSold_vin))
			Me.OnInternalUniquenessConstraint25Changed(instance, InternalUniquenessConstraint25OldValueTuple, Tuple.CreateTuple(Of Integer, Integer, Person)(instance.CarSold_vin, instance.SaleDate_YMD, instance.Buyer))
		End Sub
		<SuppressMessageAttribute("Microsoft.Usage", "CA2208")> _
		<SuppressMessageAttribute("Microsoft.Globalization", "CA1303")> _
		Private Function OnPersonBoughtCarFromPersonOnDateBuyerChanging(ByVal instance As PersonBoughtCarFromPersonOnDate, ByVal newValue As Person) As Boolean
			If Me IsNot newValue.Context Then
				Throw New ArgumentException("All objects in a relationship must be part of the same Context.", "value")
			End If
			If instance IsNot Nothing Then
				If Not (Me.OnInternalUniquenessConstraint23Changing(instance, Tuple.CreateTuple(Of Person, Integer, Person)(newValue, instance.CarSold_vin, instance.Seller))) Then
					Return False
				End If
				If Not (Me.OnInternalUniquenessConstraint25Changing(instance, Tuple.CreateTuple(Of Integer, Integer, Person)(instance.CarSold_vin, instance.SaleDate_YMD, newValue))) Then
					Return False
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnPersonBoughtCarFromPersonOnDateBuyerChanged(ByVal instance As PersonBoughtCarFromPersonOnDate, ByVal oldValue As Person)
			instance.Buyer.PersonBoughtCarFromPersonOnDateAsBuyer.Add(instance)
			Dim InternalUniquenessConstraint23OldValueTuple As Tuple(Of Person, Integer, Person)
			Dim InternalUniquenessConstraint25OldValueTuple As Tuple(Of Integer, Integer, Person)
			If oldValue IsNot Nothing Then
				oldValue.PersonBoughtCarFromPersonOnDateAsBuyer.Remove(instance)
				InternalUniquenessConstraint23OldValueTuple = Tuple.CreateTuple(Of Person, Integer, Person)(oldValue, instance.CarSold_vin, instance.Seller)
				InternalUniquenessConstraint25OldValueTuple = Tuple.CreateTuple(Of Integer, Integer, Person)(instance.CarSold_vin, instance.SaleDate_YMD, oldValue)
			Else
				InternalUniquenessConstraint23OldValueTuple = Nothing
				InternalUniquenessConstraint25OldValueTuple = Nothing
			End If
			Me.OnInternalUniquenessConstraint23Changed(instance, InternalUniquenessConstraint23OldValueTuple, Tuple.CreateTuple(Of Person, Integer, Person)(instance.Buyer, instance.CarSold_vin, instance.Seller))
			Me.OnInternalUniquenessConstraint25Changed(instance, InternalUniquenessConstraint25OldValueTuple, Tuple.CreateTuple(Of Integer, Integer, Person)(instance.CarSold_vin, instance.SaleDate_YMD, instance.Buyer))
		End Sub
		<SuppressMessageAttribute("Microsoft.Usage", "CA2208")> _
		<SuppressMessageAttribute("Microsoft.Globalization", "CA1303")> _
		Private Function OnPersonBoughtCarFromPersonOnDateSellerChanging(ByVal instance As PersonBoughtCarFromPersonOnDate, ByVal newValue As Person) As Boolean
			If Me IsNot newValue.Context Then
				Throw New ArgumentException("All objects in a relationship must be part of the same Context.", "value")
			End If
			If instance IsNot Nothing Then
				If Not (Me.OnInternalUniquenessConstraint23Changing(instance, Tuple.CreateTuple(Of Person, Integer, Person)(instance.Buyer, instance.CarSold_vin, newValue))) Then
					Return False
				End If
				If Not (Me.OnInternalUniquenessConstraint24Changing(instance, Tuple.CreateTuple(Of Integer, Person, Integer)(instance.SaleDate_YMD, newValue, instance.CarSold_vin))) Then
					Return False
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnPersonBoughtCarFromPersonOnDateSellerChanged(ByVal instance As PersonBoughtCarFromPersonOnDate, ByVal oldValue As Person)
			instance.Seller.PersonBoughtCarFromPersonOnDateAsSeller.Add(instance)
			Dim InternalUniquenessConstraint23OldValueTuple As Tuple(Of Person, Integer, Person)
			Dim InternalUniquenessConstraint24OldValueTuple As Tuple(Of Integer, Person, Integer)
			If oldValue IsNot Nothing Then
				oldValue.PersonBoughtCarFromPersonOnDateAsSeller.Remove(instance)
				InternalUniquenessConstraint23OldValueTuple = Tuple.CreateTuple(Of Person, Integer, Person)(instance.Buyer, instance.CarSold_vin, oldValue)
				InternalUniquenessConstraint24OldValueTuple = Tuple.CreateTuple(Of Integer, Person, Integer)(instance.SaleDate_YMD, oldValue, instance.CarSold_vin)
			Else
				InternalUniquenessConstraint23OldValueTuple = Nothing
				InternalUniquenessConstraint24OldValueTuple = Nothing
			End If
			Me.OnInternalUniquenessConstraint23Changed(instance, InternalUniquenessConstraint23OldValueTuple, Tuple.CreateTuple(Of Person, Integer, Person)(instance.Buyer, instance.CarSold_vin, instance.Seller))
			Me.OnInternalUniquenessConstraint24Changed(instance, InternalUniquenessConstraint24OldValueTuple, Tuple.CreateTuple(Of Integer, Person, Integer)(instance.SaleDate_YMD, instance.Seller, instance.CarSold_vin))
		End Sub
		Public Function CreatePersonBoughtCarFromPersonOnDate(ByVal CarSold_vin As Integer, ByVal SaleDate_YMD As Integer, ByVal Buyer As Person, ByVal Seller As Person) As PersonBoughtCarFromPersonOnDate
			If Not (Me.IsDeserializing) Then
				If Not (Me.OnPersonBoughtCarFromPersonOnDateCarSold_vinChanging(Nothing, CarSold_vin)) Then
					Throw New ArgumentException("Argument failed constraint enforcement.", "CarSold_vin")
				End If
				If Not (Me.OnPersonBoughtCarFromPersonOnDateSaleDate_YMDChanging(Nothing, SaleDate_YMD)) Then
					Throw New ArgumentException("Argument failed constraint enforcement.", "SaleDate_YMD")
				End If
				If Not (Me.OnPersonBoughtCarFromPersonOnDateBuyerChanging(Nothing, Buyer)) Then
					Throw New ArgumentException("Argument failed constraint enforcement.", "Buyer")
				End If
				If Not (Me.OnPersonBoughtCarFromPersonOnDateSellerChanging(Nothing, Seller)) Then
					Throw New ArgumentException("Argument failed constraint enforcement.", "Seller")
				End If
			End If
			Return New PersonBoughtCarFromPersonOnDateCore(Me, CarSold_vin, SaleDate_YMD, Buyer, Seller)
		End Function
		Private ReadOnly myPersonBoughtCarFromPersonOnDateList As List(Of PersonBoughtCarFromPersonOnDate)
		Private ReadOnly myPersonBoughtCarFromPersonOnDateReadOnlyCollection As ReadOnlyCollection(Of PersonBoughtCarFromPersonOnDate)
		Public ReadOnly Property PersonBoughtCarFromPersonOnDateCollection() As ReadOnlyCollection(Of PersonBoughtCarFromPersonOnDate) Implements _
			ISampleModelContext.PersonBoughtCarFromPersonOnDateCollection
			Get
				Return Me.myPersonBoughtCarFromPersonOnDateReadOnlyCollection
			End Get
		End Property
		#Region "PersonBoughtCarFromPersonOnDateCore"
		Private NotInheritable Class PersonBoughtCarFromPersonOnDateCore
			Inherits PersonBoughtCarFromPersonOnDate
			Public Sub New(ByVal context As SampleModelContext, ByVal CarSold_vin As Integer, ByVal SaleDate_YMD As Integer, ByVal Buyer As Person, ByVal Seller As Person)
				Me.myContext = context
				Me.myCarSold_vin = CarSold_vin
				context.OnPersonBoughtCarFromPersonOnDateCarSold_vinChanged(Me, Nothing)
				Me.mySaleDate_YMD = SaleDate_YMD
				context.OnPersonBoughtCarFromPersonOnDateSaleDate_YMDChanged(Me, Nothing)
				Me.myBuyer = Buyer
				context.OnPersonBoughtCarFromPersonOnDateBuyerChanged(Me, Nothing)
				Me.mySeller = Seller
				context.OnPersonBoughtCarFromPersonOnDateSellerChanged(Me, Nothing)
				context.myPersonBoughtCarFromPersonOnDateList.Add(Me)
			End Sub
			Private ReadOnly myContext As SampleModelContext
			Public Overrides ReadOnly Property Context() As SampleModelContext
				Get
					Return Me.myContext
				End Get
			End Property
			Private myCarSold_vin As Integer
			Public Overrides Property CarSold_vin() As Integer
				Get
					Return Me.myCarSold_vin
				End Get
				Set(ByVal Value As Integer)
					If Not (Object.Equals(Me.CarSold_vin, Value)) Then
						If Me.Context.OnPersonBoughtCarFromPersonOnDateCarSold_vinChanging(Me, Value) Then
							If MyBase.RaiseCarSold_vinChangingEvent(Value) Then
								Dim oldValue As Integer = Me.CarSold_vin
								Me.myCarSold_vin = Value
								Me.Context.OnPersonBoughtCarFromPersonOnDateCarSold_vinChanged(Me, oldValue)
								MyBase.RaiseCarSold_vinChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private mySaleDate_YMD As Integer
			Public Overrides Property SaleDate_YMD() As Integer
				Get
					Return Me.mySaleDate_YMD
				End Get
				Set(ByVal Value As Integer)
					If Not (Object.Equals(Me.SaleDate_YMD, Value)) Then
						If Me.Context.OnPersonBoughtCarFromPersonOnDateSaleDate_YMDChanging(Me, Value) Then
							If MyBase.RaiseSaleDate_YMDChangingEvent(Value) Then
								Dim oldValue As Integer = Me.SaleDate_YMD
								Me.mySaleDate_YMD = Value
								Me.Context.OnPersonBoughtCarFromPersonOnDateSaleDate_YMDChanged(Me, oldValue)
								MyBase.RaiseSaleDate_YMDChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private myBuyer As Person
			Public Overrides Property Buyer() As Person
				Get
					Return Me.myBuyer
				End Get
				Set(ByVal Value As Person)
					If Value Is Nothing Then
						Return
					End If
					If Not (Object.Equals(Me.Buyer, Value)) Then
						If Me.Context.OnPersonBoughtCarFromPersonOnDateBuyerChanging(Me, Value) Then
							If MyBase.RaiseBuyerChangingEvent(Value) Then
								Dim oldValue As Person = Me.Buyer
								Me.myBuyer = Value
								Me.Context.OnPersonBoughtCarFromPersonOnDateBuyerChanged(Me, oldValue)
								MyBase.RaiseBuyerChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private mySeller As Person
			Public Overrides Property Seller() As Person
				Get
					Return Me.mySeller
				End Get
				Set(ByVal Value As Person)
					If Value Is Nothing Then
						Return
					End If
					If Not (Object.Equals(Me.Seller, Value)) Then
						If Me.Context.OnPersonBoughtCarFromPersonOnDateSellerChanging(Me, Value) Then
							If MyBase.RaiseSellerChangingEvent(Value) Then
								Dim oldValue As Person = Me.Seller
								Me.mySeller = Value
								Me.Context.OnPersonBoughtCarFromPersonOnDateSellerChanged(Me, oldValue)
								MyBase.RaiseSellerChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
		End Class
		#End Region
		Private Function OnReviewCar_vinChanging(ByVal instance As Review, ByVal newValue As Integer) As Boolean
			If instance IsNot Nothing Then
				If Not (Me.OnInternalUniquenessConstraint26Changing(instance, Tuple.CreateTuple(Of Integer, String)(newValue, instance.Criteria_Name))) Then
					Return False
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnReviewCar_vinChanged(ByVal instance As Review, ByVal oldValue As Nullable(Of Integer))
			Dim InternalUniquenessConstraint26OldValueTuple As Tuple(Of Integer, String)
			If oldValue IsNot Nothing Then
				InternalUniquenessConstraint26OldValueTuple = Tuple.CreateTuple(Of Integer, String)(oldValue.Value, instance.Criteria_Name)
			Else
				InternalUniquenessConstraint26OldValueTuple = Nothing
			End If
			Me.OnInternalUniquenessConstraint26Changed(instance, InternalUniquenessConstraint26OldValueTuple, Tuple.CreateTuple(Of Integer, String)(instance.Car_vin, instance.Criteria_Name))
		End Sub
		Private Function OnReviewRating_Nr_IntegerChanging(ByVal instance As Review, ByVal newValue As Integer) As Boolean
			Return True
		End Function
		Private Function OnReviewCriteria_NameChanging(ByVal instance As Review, ByVal newValue As String) As Boolean
			If instance IsNot Nothing Then
				If Not (Me.OnInternalUniquenessConstraint26Changing(instance, Tuple.CreateTuple(Of Integer, String)(instance.Car_vin, newValue))) Then
					Return False
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnReviewCriteria_NameChanged(ByVal instance As Review, ByVal oldValue As String)
			Dim InternalUniquenessConstraint26OldValueTuple As Tuple(Of Integer, String)
			If oldValue IsNot Nothing Then
				InternalUniquenessConstraint26OldValueTuple = Tuple.CreateTuple(Of Integer, String)(instance.Car_vin, oldValue)
			Else
				InternalUniquenessConstraint26OldValueTuple = Nothing
			End If
			Me.OnInternalUniquenessConstraint26Changed(instance, InternalUniquenessConstraint26OldValueTuple, Tuple.CreateTuple(Of Integer, String)(instance.Car_vin, instance.Criteria_Name))
		End Sub
		Public Function CreateReview(ByVal Car_vin As Integer, ByVal Rating_Nr_Integer As Integer, ByVal Criteria_Name As String) As Review
			If Not (Me.IsDeserializing) Then
				If Not (Me.OnReviewCar_vinChanging(Nothing, Car_vin)) Then
					Throw New ArgumentException("Argument failed constraint enforcement.", "Car_vin")
				End If
				If Not (Me.OnReviewRating_Nr_IntegerChanging(Nothing, Rating_Nr_Integer)) Then
					Throw New ArgumentException("Argument failed constraint enforcement.", "Rating_Nr_Integer")
				End If
				If Not (Me.OnReviewCriteria_NameChanging(Nothing, Criteria_Name)) Then
					Throw New ArgumentException("Argument failed constraint enforcement.", "Criteria_Name")
				End If
			End If
			Return New ReviewCore(Me, Car_vin, Rating_Nr_Integer, Criteria_Name)
		End Function
		Private ReadOnly myReviewList As List(Of Review)
		Private ReadOnly myReviewReadOnlyCollection As ReadOnlyCollection(Of Review)
		Public ReadOnly Property ReviewCollection() As ReadOnlyCollection(Of Review) Implements _
			ISampleModelContext.ReviewCollection
			Get
				Return Me.myReviewReadOnlyCollection
			End Get
		End Property
		#Region "ReviewCore"
		Private NotInheritable Class ReviewCore
			Inherits Review
			Public Sub New(ByVal context As SampleModelContext, ByVal Car_vin As Integer, ByVal Rating_Nr_Integer As Integer, ByVal Criteria_Name As String)
				Me.myContext = context
				Me.myCar_vin = Car_vin
				context.OnReviewCar_vinChanged(Me, Nothing)
				Me.myRating_Nr_Integer = Rating_Nr_Integer
				Me.myCriteria_Name = Criteria_Name
				context.OnReviewCriteria_NameChanged(Me, Nothing)
				context.myReviewList.Add(Me)
			End Sub
			Private ReadOnly myContext As SampleModelContext
			Public Overrides ReadOnly Property Context() As SampleModelContext
				Get
					Return Me.myContext
				End Get
			End Property
			Private myCar_vin As Integer
			Public Overrides Property Car_vin() As Integer
				Get
					Return Me.myCar_vin
				End Get
				Set(ByVal Value As Integer)
					If Not (Object.Equals(Me.Car_vin, Value)) Then
						If Me.Context.OnReviewCar_vinChanging(Me, Value) Then
							If MyBase.RaiseCar_vinChangingEvent(Value) Then
								Dim oldValue As Integer = Me.Car_vin
								Me.myCar_vin = Value
								Me.Context.OnReviewCar_vinChanged(Me, oldValue)
								MyBase.RaiseCar_vinChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private myRating_Nr_Integer As Integer
			Public Overrides Property Rating_Nr_Integer() As Integer
				Get
					Return Me.myRating_Nr_Integer
				End Get
				Set(ByVal Value As Integer)
					If Not (Object.Equals(Me.Rating_Nr_Integer, Value)) Then
						If Me.Context.OnReviewRating_Nr_IntegerChanging(Me, Value) Then
							If MyBase.RaiseRating_Nr_IntegerChangingEvent(Value) Then
								Dim oldValue As Integer = Me.Rating_Nr_Integer
								Me.myRating_Nr_Integer = Value
								MyBase.RaiseRating_Nr_IntegerChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private myCriteria_Name As String
			Public Overrides Property Criteria_Name() As String
				Get
					Return Me.myCriteria_Name
				End Get
				Set(ByVal Value As String)
					If Value Is Nothing Then
						Return
					End If
					If Not (Object.Equals(Me.Criteria_Name, Value)) Then
						If Me.Context.OnReviewCriteria_NameChanging(Me, Value) Then
							If MyBase.RaiseCriteria_NameChangingEvent(Value) Then
								Dim oldValue As String = Me.Criteria_Name
								Me.myCriteria_Name = Value
								Me.Context.OnReviewCriteria_NameChanged(Me, oldValue)
								MyBase.RaiseCriteria_NameChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
		End Class
		#End Region
		Private Function OnPersonHasNickNameNickNameChanging(ByVal instance As PersonHasNickName, ByVal newValue As String) As Boolean
			If instance IsNot Nothing Then
				If Not (Me.OnInternalUniquenessConstraint33Changing(instance, Tuple.CreateTuple(Of String, Person)(newValue, instance.Person))) Then
					Return False
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnPersonHasNickNameNickNameChanged(ByVal instance As PersonHasNickName, ByVal oldValue As String)
			Dim InternalUniquenessConstraint33OldValueTuple As Tuple(Of String, Person)
			If oldValue IsNot Nothing Then
				InternalUniquenessConstraint33OldValueTuple = Tuple.CreateTuple(Of String, Person)(oldValue, instance.Person)
			Else
				InternalUniquenessConstraint33OldValueTuple = Nothing
			End If
			Me.OnInternalUniquenessConstraint33Changed(instance, InternalUniquenessConstraint33OldValueTuple, Tuple.CreateTuple(Of String, Person)(instance.NickName, instance.Person))
		End Sub
		<SuppressMessageAttribute("Microsoft.Usage", "CA2208")> _
		<SuppressMessageAttribute("Microsoft.Globalization", "CA1303")> _
		Private Function OnPersonHasNickNamePersonChanging(ByVal instance As PersonHasNickName, ByVal newValue As Person) As Boolean
			If Me IsNot newValue.Context Then
				Throw New ArgumentException("All objects in a relationship must be part of the same Context.", "value")
			End If
			If instance IsNot Nothing Then
				If Not (Me.OnInternalUniquenessConstraint33Changing(instance, Tuple.CreateTuple(Of String, Person)(instance.NickName, newValue))) Then
					Return False
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnPersonHasNickNamePersonChanged(ByVal instance As PersonHasNickName, ByVal oldValue As Person)
			instance.Person.PersonHasNickNameAsPerson.Add(instance)
			Dim InternalUniquenessConstraint33OldValueTuple As Tuple(Of String, Person)
			If oldValue IsNot Nothing Then
				oldValue.PersonHasNickNameAsPerson.Remove(instance)
				InternalUniquenessConstraint33OldValueTuple = Tuple.CreateTuple(Of String, Person)(instance.NickName, oldValue)
			Else
				InternalUniquenessConstraint33OldValueTuple = Nothing
			End If
			Me.OnInternalUniquenessConstraint33Changed(instance, InternalUniquenessConstraint33OldValueTuple, Tuple.CreateTuple(Of String, Person)(instance.NickName, instance.Person))
		End Sub
		Public Function CreatePersonHasNickName(ByVal NickName As String, ByVal Person As Person) As PersonHasNickName
			If Not (Me.IsDeserializing) Then
				If Not (Me.OnPersonHasNickNameNickNameChanging(Nothing, NickName)) Then
					Throw New ArgumentException("Argument failed constraint enforcement.", "NickName")
				End If
				If Not (Me.OnPersonHasNickNamePersonChanging(Nothing, Person)) Then
					Throw New ArgumentException("Argument failed constraint enforcement.", "Person")
				End If
			End If
			Return New PersonHasNickNameCore(Me, NickName, Person)
		End Function
		Private ReadOnly myPersonHasNickNameList As List(Of PersonHasNickName)
		Private ReadOnly myPersonHasNickNameReadOnlyCollection As ReadOnlyCollection(Of PersonHasNickName)
		Public ReadOnly Property PersonHasNickNameCollection() As ReadOnlyCollection(Of PersonHasNickName) Implements _
			ISampleModelContext.PersonHasNickNameCollection
			Get
				Return Me.myPersonHasNickNameReadOnlyCollection
			End Get
		End Property
		#Region "PersonHasNickNameCore"
		Private NotInheritable Class PersonHasNickNameCore
			Inherits PersonHasNickName
			Public Sub New(ByVal context As SampleModelContext, ByVal NickName As String, ByVal Person As Person)
				Me.myContext = context
				Me.myNickName = NickName
				context.OnPersonHasNickNameNickNameChanged(Me, Nothing)
				Me.myPerson = Person
				context.OnPersonHasNickNamePersonChanged(Me, Nothing)
				context.myPersonHasNickNameList.Add(Me)
			End Sub
			Private ReadOnly myContext As SampleModelContext
			Public Overrides ReadOnly Property Context() As SampleModelContext
				Get
					Return Me.myContext
				End Get
			End Property
			Private myNickName As String
			Public Overrides Property NickName() As String
				Get
					Return Me.myNickName
				End Get
				Set(ByVal Value As String)
					If Value Is Nothing Then
						Return
					End If
					If Not (Object.Equals(Me.NickName, Value)) Then
						If Me.Context.OnPersonHasNickNameNickNameChanging(Me, Value) Then
							If MyBase.RaiseNickNameChangingEvent(Value) Then
								Dim oldValue As String = Me.NickName
								Me.myNickName = Value
								Me.Context.OnPersonHasNickNameNickNameChanged(Me, oldValue)
								MyBase.RaiseNickNameChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private myPerson As Person
			Public Overrides Property Person() As Person
				Get
					Return Me.myPerson
				End Get
				Set(ByVal Value As Person)
					If Value Is Nothing Then
						Return
					End If
					If Not (Object.Equals(Me.Person, Value)) Then
						If Me.Context.OnPersonHasNickNamePersonChanging(Me, Value) Then
							If MyBase.RaisePersonChangingEvent(Value) Then
								Dim oldValue As Person = Me.Person
								Me.myPerson = Value
								Me.Context.OnPersonHasNickNamePersonChanged(Me, oldValue)
								MyBase.RaisePersonChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
		End Class
		#End Region
		Private Function OnPersonFirstNameChanging(ByVal instance As Person, ByVal newValue As String) As Boolean
			If instance IsNot Nothing Then
				If Not (Me.OnExternalUniquenessConstraint1Changing(instance, Tuple.CreateTuple(Of String, Integer)(newValue, instance.Date_YMD))) Then
					Return False
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnPersonFirstNameChanged(ByVal instance As Person, ByVal oldValue As String)
			Dim ExternalUniquenessConstraint1OldValueTuple As Tuple(Of String, Integer)
			If oldValue IsNot Nothing Then
				ExternalUniquenessConstraint1OldValueTuple = Tuple.CreateTuple(Of String, Integer)(oldValue, instance.Date_YMD)
			Else
				ExternalUniquenessConstraint1OldValueTuple = Nothing
			End If
			Me.OnExternalUniquenessConstraint1Changed(instance, ExternalUniquenessConstraint1OldValueTuple, Tuple.CreateTuple(Of String, Integer)(instance.FirstName, instance.Date_YMD))
		End Sub
		Private Function OnPersonDate_YMDChanging(ByVal instance As Person, ByVal newValue As Integer) As Boolean
			If instance IsNot Nothing Then
				If Not (Me.OnExternalUniquenessConstraint1Changing(instance, Tuple.CreateTuple(Of String, Integer)(instance.FirstName, newValue))) Then
					Return False
				End If
				If Not (Me.OnExternalUniquenessConstraint2Changing(instance, Tuple.CreateTuple(Of String, Integer)(instance.LastName, newValue))) Then
					Return False
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnPersonDate_YMDChanged(ByVal instance As Person, ByVal oldValue As Nullable(Of Integer))
			Dim ExternalUniquenessConstraint1OldValueTuple As Tuple(Of String, Integer)
			Dim ExternalUniquenessConstraint2OldValueTuple As Tuple(Of String, Integer)
			If oldValue IsNot Nothing Then
				ExternalUniquenessConstraint1OldValueTuple = Tuple.CreateTuple(Of String, Integer)(instance.FirstName, oldValue.Value)
				ExternalUniquenessConstraint2OldValueTuple = Tuple.CreateTuple(Of String, Integer)(instance.LastName, oldValue.Value)
			Else
				ExternalUniquenessConstraint1OldValueTuple = Nothing
				ExternalUniquenessConstraint2OldValueTuple = Nothing
			End If
			Me.OnExternalUniquenessConstraint1Changed(instance, ExternalUniquenessConstraint1OldValueTuple, Tuple.CreateTuple(Of String, Integer)(instance.FirstName, instance.Date_YMD))
			Me.OnExternalUniquenessConstraint2Changed(instance, ExternalUniquenessConstraint2OldValueTuple, Tuple.CreateTuple(Of String, Integer)(instance.LastName, instance.Date_YMD))
		End Sub
		Private Function OnPersonLastNameChanging(ByVal instance As Person, ByVal newValue As String) As Boolean
			If instance IsNot Nothing Then
				If Not (Me.OnExternalUniquenessConstraint2Changing(instance, Tuple.CreateTuple(Of String, Integer)(newValue, instance.Date_YMD))) Then
					Return False
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnPersonLastNameChanged(ByVal instance As Person, ByVal oldValue As String)
			Dim ExternalUniquenessConstraint2OldValueTuple As Tuple(Of String, Integer)
			If oldValue IsNot Nothing Then
				ExternalUniquenessConstraint2OldValueTuple = Tuple.CreateTuple(Of String, Integer)(oldValue, instance.Date_YMD)
			Else
				ExternalUniquenessConstraint2OldValueTuple = Nothing
			End If
			Me.OnExternalUniquenessConstraint2Changed(instance, ExternalUniquenessConstraint2OldValueTuple, Tuple.CreateTuple(Of String, Integer)(instance.LastName, instance.Date_YMD))
		End Sub
		Private Function OnPersonSocialSecurityNumberChanging(ByVal instance As Person, ByVal newValue As String) As Boolean
			If newValue IsNot Nothing Then
				Dim currentInstance As Person = instance
				If Me.myPersonSocialSecurityNumberDictionary.TryGetValue(newValue, currentInstance) Then
					If Not (Object.Equals(currentInstance, instance)) Then
						Return False
					End If
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnPersonSocialSecurityNumberChanged(ByVal instance As Person, ByVal oldValue As String)
			If instance.SocialSecurityNumber IsNot Nothing Then
				Me.myPersonSocialSecurityNumberDictionary.Add(instance.SocialSecurityNumber, instance)
			End If
			If oldValue IsNot Nothing Then
				Me.myPersonSocialSecurityNumberDictionary.Remove(oldValue)
			Else
			End If
		End Sub
		Private Function OnPersonHatType_ColorARGBChanging(ByVal instance As Person, ByVal newValue As Nullable(Of Integer)) As Boolean
			Return True
		End Function
		Private Function OnPersonHatType_HatTypeStyle_HatTypeStyle_DescriptionChanging(ByVal instance As Person, ByVal newValue As String) As Boolean
			Return True
		End Function
		Private Function OnPersonOwnsCar_vinChanging(ByVal instance As Person, ByVal newValue As Nullable(Of Integer)) As Boolean
			If newValue IsNot Nothing Then
				Dim currentInstance As Person = instance
				If Me.myPersonOwnsCar_vinDictionary.TryGetValue(newValue, currentInstance) Then
					If Not (Object.Equals(currentInstance, instance)) Then
						Return False
					End If
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnPersonOwnsCar_vinChanged(ByVal instance As Person, ByVal oldValue As Nullable(Of Integer))
			If instance.OwnsCar_vin IsNot Nothing Then
				Me.myPersonOwnsCar_vinDictionary.Add(instance.OwnsCar_vin, instance)
			End If
			If oldValue IsNot Nothing Then
				Me.myPersonOwnsCar_vinDictionary.Remove(oldValue)
			Else
			End If
		End Sub
		Private Function OnPersonGender_Gender_CodeChanging(ByVal instance As Person, ByVal newValue As String) As Boolean
			Return True
		End Function
		Private Function OnPersonPersonHasParentsChanging(ByVal instance As Person, ByVal newValue As Nullable(Of Boolean)) As Boolean
			Return True
		End Function
		<SuppressMessageAttribute("Microsoft.Usage", "CA2208")> _
		<SuppressMessageAttribute("Microsoft.Globalization", "CA1303")> _
		Private Function OnPersonValueType1DoesSomethingElseWithChanging(ByVal instance As Person, ByVal newValue As ValueType1) As Boolean
			If newValue IsNot Nothing Then
				If Me IsNot newValue.Context Then
					Throw New ArgumentException("All objects in a relationship must be part of the same Context.", "value")
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnPersonValueType1DoesSomethingElseWithChanged(ByVal instance As Person, ByVal oldValue As ValueType1)
			If instance.ValueType1DoesSomethingElseWith IsNot Nothing Then
				instance.ValueType1DoesSomethingElseWith.DoesSomethingElseWithPerson.Add(instance)
			End If
			If oldValue IsNot Nothing Then
				oldValue.DoesSomethingElseWithPerson.Remove(instance)
			Else
			End If
		End Sub
		<SuppressMessageAttribute("Microsoft.Usage", "CA2208")> _
		<SuppressMessageAttribute("Microsoft.Globalization", "CA1303")> _
		Private Function OnPersonMalePersonChanging(ByVal instance As Person, ByVal newValue As MalePerson) As Boolean
			If newValue IsNot Nothing Then
				If Me IsNot newValue.Context Then
					Throw New ArgumentException("All objects in a relationship must be part of the same Context.", "value")
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnPersonMalePersonChanged(ByVal instance As Person, ByVal oldValue As MalePerson)
			If instance.MalePerson IsNot Nothing Then
				instance.MalePerson.Person = instance
			End If
			If oldValue IsNot Nothing Then
				oldValue.Person = Nothing
			Else
			End If
		End Sub
		<SuppressMessageAttribute("Microsoft.Usage", "CA2208")> _
		<SuppressMessageAttribute("Microsoft.Globalization", "CA1303")> _
		Private Function OnPersonFemalePersonChanging(ByVal instance As Person, ByVal newValue As FemalePerson) As Boolean
			If newValue IsNot Nothing Then
				If Me IsNot newValue.Context Then
					Throw New ArgumentException("All objects in a relationship must be part of the same Context.", "value")
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnPersonFemalePersonChanged(ByVal instance As Person, ByVal oldValue As FemalePerson)
			If instance.FemalePerson IsNot Nothing Then
				instance.FemalePerson.Person = instance
			End If
			If oldValue IsNot Nothing Then
				oldValue.Person = Nothing
			Else
			End If
		End Sub
		<SuppressMessageAttribute("Microsoft.Usage", "CA2208")> _
		<SuppressMessageAttribute("Microsoft.Globalization", "CA1303")> _
		Private Function OnPersonChildPersonChanging(ByVal instance As Person, ByVal newValue As ChildPerson) As Boolean
			If newValue IsNot Nothing Then
				If Me IsNot newValue.Context Then
					Throw New ArgumentException("All objects in a relationship must be part of the same Context.", "value")
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnPersonChildPersonChanged(ByVal instance As Person, ByVal oldValue As ChildPerson)
			If instance.ChildPerson IsNot Nothing Then
				instance.ChildPerson.Person = instance
			End If
			If oldValue IsNot Nothing Then
				oldValue.Person = Nothing
			Else
			End If
		End Sub
		<SuppressMessageAttribute("Microsoft.Usage", "CA2208")> _
		<SuppressMessageAttribute("Microsoft.Globalization", "CA1303")> _
		Private Function OnPersonDeathChanging(ByVal instance As Person, ByVal newValue As Death) As Boolean
			If newValue IsNot Nothing Then
				If Me IsNot newValue.Context Then
					Throw New ArgumentException("All objects in a relationship must be part of the same Context.", "value")
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnPersonDeathChanged(ByVal instance As Person, ByVal oldValue As Death)
			If instance.Death IsNot Nothing Then
				instance.Death.Person = instance
			End If
			If oldValue IsNot Nothing Then
				oldValue.Person = Nothing
			Else
			End If
		End Sub
		Private Function OnPersonPersonDrivesCarAsDrivenByPersonAdding(ByVal instance As Person, ByVal value As PersonDrivesCar) As Boolean
			Return True
		End Function
		Private Sub OnPersonPersonDrivesCarAsDrivenByPersonAdded(ByVal instance As Person, ByVal value As PersonDrivesCar)
			If value IsNot Nothing Then
				value.DrivenByPerson = instance
			End If
		End Sub
		Private Function OnPersonPersonDrivesCarAsDrivenByPersonRemoving(ByVal instance As Person, ByVal value As PersonDrivesCar) As Boolean
			Return True
		End Function
		Private Sub OnPersonPersonDrivesCarAsDrivenByPersonRemoved(ByVal instance As Person, ByVal value As PersonDrivesCar)
			If value IsNot Nothing Then
				value.DrivenByPerson = Nothing
			End If
		End Sub
		Private Function OnPersonPersonBoughtCarFromPersonOnDateAsBuyerAdding(ByVal instance As Person, ByVal value As PersonBoughtCarFromPersonOnDate) As Boolean
			Return True
		End Function
		Private Sub OnPersonPersonBoughtCarFromPersonOnDateAsBuyerAdded(ByVal instance As Person, ByVal value As PersonBoughtCarFromPersonOnDate)
			If value IsNot Nothing Then
				value.Buyer = instance
			End If
		End Sub
		Private Function OnPersonPersonBoughtCarFromPersonOnDateAsBuyerRemoving(ByVal instance As Person, ByVal value As PersonBoughtCarFromPersonOnDate) As Boolean
			Return True
		End Function
		Private Sub OnPersonPersonBoughtCarFromPersonOnDateAsBuyerRemoved(ByVal instance As Person, ByVal value As PersonBoughtCarFromPersonOnDate)
			If value IsNot Nothing Then
				value.Buyer = Nothing
			End If
		End Sub
		Private Function OnPersonPersonBoughtCarFromPersonOnDateAsSellerAdding(ByVal instance As Person, ByVal value As PersonBoughtCarFromPersonOnDate) As Boolean
			Return True
		End Function
		Private Sub OnPersonPersonBoughtCarFromPersonOnDateAsSellerAdded(ByVal instance As Person, ByVal value As PersonBoughtCarFromPersonOnDate)
			If value IsNot Nothing Then
				value.Seller = instance
			End If
		End Sub
		Private Function OnPersonPersonBoughtCarFromPersonOnDateAsSellerRemoving(ByVal instance As Person, ByVal value As PersonBoughtCarFromPersonOnDate) As Boolean
			Return True
		End Function
		Private Sub OnPersonPersonBoughtCarFromPersonOnDateAsSellerRemoved(ByVal instance As Person, ByVal value As PersonBoughtCarFromPersonOnDate)
			If value IsNot Nothing Then
				value.Seller = Nothing
			End If
		End Sub
		Private Function OnPersonPersonHasNickNameAsPersonAdding(ByVal instance As Person, ByVal value As PersonHasNickName) As Boolean
			Return True
		End Function
		Private Sub OnPersonPersonHasNickNameAsPersonAdded(ByVal instance As Person, ByVal value As PersonHasNickName)
			If value IsNot Nothing Then
				value.Person = instance
			End If
		End Sub
		Private Function OnPersonPersonHasNickNameAsPersonRemoving(ByVal instance As Person, ByVal value As PersonHasNickName) As Boolean
			Return True
		End Function
		Private Sub OnPersonPersonHasNickNameAsPersonRemoved(ByVal instance As Person, ByVal value As PersonHasNickName)
			If value IsNot Nothing Then
				value.Person = Nothing
			End If
		End Sub
		Private Function OnPersonTaskAdding(ByVal instance As Person, ByVal value As Task) As Boolean
			Return True
		End Function
		Private Sub OnPersonTaskAdded(ByVal instance As Person, ByVal value As Task)
			If value IsNot Nothing Then
				value.Person = instance
			End If
		End Sub
		Private Function OnPersonTaskRemoving(ByVal instance As Person, ByVal value As Task) As Boolean
			Return True
		End Function
		Private Sub OnPersonTaskRemoved(ByVal instance As Person, ByVal value As Task)
			If value IsNot Nothing Then
				value.Person = Nothing
			End If
		End Sub
		Private Function OnPersonValueType1DoesSomethingWithAdding(ByVal instance As Person, ByVal value As ValueType1) As Boolean
			Return True
		End Function
		Private Sub OnPersonValueType1DoesSomethingWithAdded(ByVal instance As Person, ByVal value As ValueType1)
			If value IsNot Nothing Then
				value.DoesSomethingWithPerson = instance
			End If
		End Sub
		Private Function OnPersonValueType1DoesSomethingWithRemoving(ByVal instance As Person, ByVal value As ValueType1) As Boolean
			Return True
		End Function
		Private Sub OnPersonValueType1DoesSomethingWithRemoved(ByVal instance As Person, ByVal value As ValueType1)
			If value IsNot Nothing Then
				value.DoesSomethingWithPerson = Nothing
			End If
		End Sub
		Public Function CreatePerson(ByVal FirstName As String, ByVal Date_YMD As Integer, ByVal LastName As String, ByVal Gender_Gender_Code As String) As Person
			If Not (Me.IsDeserializing) Then
				If Not (Me.OnPersonFirstNameChanging(Nothing, FirstName)) Then
					Throw New ArgumentException("Argument failed constraint enforcement.", "FirstName")
				End If
				If Not (Me.OnPersonDate_YMDChanging(Nothing, Date_YMD)) Then
					Throw New ArgumentException("Argument failed constraint enforcement.", "Date_YMD")
				End If
				If Not (Me.OnPersonLastNameChanging(Nothing, LastName)) Then
					Throw New ArgumentException("Argument failed constraint enforcement.", "LastName")
				End If
				If Not (Me.OnPersonGender_Gender_CodeChanging(Nothing, Gender_Gender_Code)) Then
					Throw New ArgumentException("Argument failed constraint enforcement.", "Gender_Gender_Code")
				End If
			End If
			Return New PersonCore(Me, FirstName, Date_YMD, LastName, Gender_Gender_Code)
		End Function
		Private ReadOnly myPersonList As List(Of Person)
		Private ReadOnly myPersonReadOnlyCollection As ReadOnlyCollection(Of Person)
		Public ReadOnly Property PersonCollection() As ReadOnlyCollection(Of Person) Implements _
			ISampleModelContext.PersonCollection
			Get
				Return Me.myPersonReadOnlyCollection
			End Get
		End Property
		#Region "PersonCore"
		Private NotInheritable Class PersonCore
			Inherits Person
			Public Sub New(ByVal context As SampleModelContext, ByVal FirstName As String, ByVal Date_YMD As Integer, ByVal LastName As String, ByVal Gender_Gender_Code As String)
				Me.myContext = context
				Me.myPersonDrivesCarAsDrivenByPerson = New ConstraintEnforcementCollection(Of Person, PersonDrivesCar)(Me, New PotentialCollectionModificationCallback(Of Person, PersonDrivesCar)(context.OnPersonPersonDrivesCarAsDrivenByPersonAdding), New CommittedCollectionModificationCallback(Of Person, PersonDrivesCar)(context.OnPersonPersonDrivesCarAsDrivenByPersonAdded), New PotentialCollectionModificationCallback(Of Person, PersonDrivesCar)(context.OnPersonPersonDrivesCarAsDrivenByPersonRemoving), New CommittedCollectionModificationCallback(Of Person, PersonDrivesCar)(context.OnPersonPersonDrivesCarAsDrivenByPersonRemoved))
				Me.myPersonBoughtCarFromPersonOnDateAsBuyer = New ConstraintEnforcementCollection(Of Person, PersonBoughtCarFromPersonOnDate)(Me, New PotentialCollectionModificationCallback(Of Person, PersonBoughtCarFromPersonOnDate)(context.OnPersonPersonBoughtCarFromPersonOnDateAsBuyerAdding), New CommittedCollectionModificationCallback(Of Person, PersonBoughtCarFromPersonOnDate)(context.OnPersonPersonBoughtCarFromPersonOnDateAsBuyerAdded), New PotentialCollectionModificationCallback(Of Person, PersonBoughtCarFromPersonOnDate)(context.OnPersonPersonBoughtCarFromPersonOnDateAsBuyerRemoving), New CommittedCollectionModificationCallback(Of Person, PersonBoughtCarFromPersonOnDate)(context.OnPersonPersonBoughtCarFromPersonOnDateAsBuyerRemoved))
				Me.myPersonBoughtCarFromPersonOnDateAsSeller = New ConstraintEnforcementCollection(Of Person, PersonBoughtCarFromPersonOnDate)(Me, New PotentialCollectionModificationCallback(Of Person, PersonBoughtCarFromPersonOnDate)(context.OnPersonPersonBoughtCarFromPersonOnDateAsSellerAdding), New CommittedCollectionModificationCallback(Of Person, PersonBoughtCarFromPersonOnDate)(context.OnPersonPersonBoughtCarFromPersonOnDateAsSellerAdded), New PotentialCollectionModificationCallback(Of Person, PersonBoughtCarFromPersonOnDate)(context.OnPersonPersonBoughtCarFromPersonOnDateAsSellerRemoving), New CommittedCollectionModificationCallback(Of Person, PersonBoughtCarFromPersonOnDate)(context.OnPersonPersonBoughtCarFromPersonOnDateAsSellerRemoved))
				Me.myPersonHasNickNameAsPerson = New ConstraintEnforcementCollection(Of Person, PersonHasNickName)(Me, New PotentialCollectionModificationCallback(Of Person, PersonHasNickName)(context.OnPersonPersonHasNickNameAsPersonAdding), New CommittedCollectionModificationCallback(Of Person, PersonHasNickName)(context.OnPersonPersonHasNickNameAsPersonAdded), New PotentialCollectionModificationCallback(Of Person, PersonHasNickName)(context.OnPersonPersonHasNickNameAsPersonRemoving), New CommittedCollectionModificationCallback(Of Person, PersonHasNickName)(context.OnPersonPersonHasNickNameAsPersonRemoved))
				Me.myTask = New ConstraintEnforcementCollection(Of Person, Task)(Me, New PotentialCollectionModificationCallback(Of Person, Task)(context.OnPersonTaskAdding), New CommittedCollectionModificationCallback(Of Person, Task)(context.OnPersonTaskAdded), New PotentialCollectionModificationCallback(Of Person, Task)(context.OnPersonTaskRemoving), New CommittedCollectionModificationCallback(Of Person, Task)(context.OnPersonTaskRemoved))
				Me.myValueType1DoesSomethingWith = New ConstraintEnforcementCollection(Of Person, ValueType1)(Me, New PotentialCollectionModificationCallback(Of Person, ValueType1)(context.OnPersonValueType1DoesSomethingWithAdding), New CommittedCollectionModificationCallback(Of Person, ValueType1)(context.OnPersonValueType1DoesSomethingWithAdded), New PotentialCollectionModificationCallback(Of Person, ValueType1)(context.OnPersonValueType1DoesSomethingWithRemoving), New CommittedCollectionModificationCallback(Of Person, ValueType1)(context.OnPersonValueType1DoesSomethingWithRemoved))
				Me.myFirstName = FirstName
				context.OnPersonFirstNameChanged(Me, Nothing)
				Me.myDate_YMD = Date_YMD
				context.OnPersonDate_YMDChanged(Me, Nothing)
				Me.myLastName = LastName
				context.OnPersonLastNameChanged(Me, Nothing)
				Me.myGender_Gender_Code = Gender_Gender_Code
				context.myPersonList.Add(Me)
			End Sub
			Private ReadOnly myContext As SampleModelContext
			Public Overrides ReadOnly Property Context() As SampleModelContext
				Get
					Return Me.myContext
				End Get
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
								Me.Context.OnPersonFirstNameChanged(Me, oldValue)
								MyBase.RaiseFirstNameChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private myDate_YMD As Integer
			Public Overrides Property Date_YMD() As Integer
				Get
					Return Me.myDate_YMD
				End Get
				Set(ByVal Value As Integer)
					If Not (Object.Equals(Me.Date_YMD, Value)) Then
						If Me.Context.OnPersonDate_YMDChanging(Me, Value) Then
							If MyBase.RaiseDate_YMDChangingEvent(Value) Then
								Dim oldValue As Integer = Me.Date_YMD
								Me.myDate_YMD = Value
								Me.Context.OnPersonDate_YMDChanged(Me, oldValue)
								MyBase.RaiseDate_YMDChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
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
								Me.Context.OnPersonLastNameChanged(Me, oldValue)
								MyBase.RaiseLastNameChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private mySocialSecurityNumber As String
			Public Overrides Property SocialSecurityNumber() As String
				Get
					Return Me.mySocialSecurityNumber
				End Get
				Set(ByVal Value As String)
					If Not (Object.Equals(Me.SocialSecurityNumber, Value)) Then
						If Me.Context.OnPersonSocialSecurityNumberChanging(Me, Value) Then
							If MyBase.RaiseSocialSecurityNumberChangingEvent(Value) Then
								Dim oldValue As String = Me.SocialSecurityNumber
								Me.mySocialSecurityNumber = Value
								Me.Context.OnPersonSocialSecurityNumberChanged(Me, oldValue)
								MyBase.RaiseSocialSecurityNumberChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private myHatType_ColorARGB As Nullable(Of Integer)
			Public Overrides Property HatType_ColorARGB() As Nullable(Of Integer)
				Get
					Return Me.myHatType_ColorARGB
				End Get
				Set(ByVal Value As Nullable(Of Integer))
					If Not (Object.Equals(Me.HatType_ColorARGB, Value)) Then
						If Me.Context.OnPersonHatType_ColorARGBChanging(Me, Value) Then
							If MyBase.RaiseHatType_ColorARGBChangingEvent(Value) Then
								Dim oldValue As Nullable(Of Integer) = Me.HatType_ColorARGB
								Me.myHatType_ColorARGB = Value
								MyBase.RaiseHatType_ColorARGBChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private myHatType_HatTypeStyle_HatTypeStyle_Description As String
			Public Overrides Property HatType_HatTypeStyle_HatTypeStyle_Description() As String
				Get
					Return Me.myHatType_HatTypeStyle_HatTypeStyle_Description
				End Get
				Set(ByVal Value As String)
					If Not (Object.Equals(Me.HatType_HatTypeStyle_HatTypeStyle_Description, Value)) Then
						If Me.Context.OnPersonHatType_HatTypeStyle_HatTypeStyle_DescriptionChanging(Me, Value) Then
							If MyBase.RaiseHatType_HatTypeStyle_HatTypeStyle_DescriptionChangingEvent(Value) Then
								Dim oldValue As String = Me.HatType_HatTypeStyle_HatTypeStyle_Description
								Me.myHatType_HatTypeStyle_HatTypeStyle_Description = Value
								MyBase.RaiseHatType_HatTypeStyle_HatTypeStyle_DescriptionChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private myOwnsCar_vin As Nullable(Of Integer)
			Public Overrides Property OwnsCar_vin() As Nullable(Of Integer)
				Get
					Return Me.myOwnsCar_vin
				End Get
				Set(ByVal Value As Nullable(Of Integer))
					If Not (Object.Equals(Me.OwnsCar_vin, Value)) Then
						If Me.Context.OnPersonOwnsCar_vinChanging(Me, Value) Then
							If MyBase.RaiseOwnsCar_vinChangingEvent(Value) Then
								Dim oldValue As Nullable(Of Integer) = Me.OwnsCar_vin
								Me.myOwnsCar_vin = Value
								Me.Context.OnPersonOwnsCar_vinChanged(Me, oldValue)
								MyBase.RaiseOwnsCar_vinChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private myGender_Gender_Code As String
			Public Overrides Property Gender_Gender_Code() As String
				Get
					Return Me.myGender_Gender_Code
				End Get
				Set(ByVal Value As String)
					If Value Is Nothing Then
						Return
					End If
					If Not (Object.Equals(Me.Gender_Gender_Code, Value)) Then
						If Me.Context.OnPersonGender_Gender_CodeChanging(Me, Value) Then
							If MyBase.RaiseGender_Gender_CodeChangingEvent(Value) Then
								Dim oldValue As String = Me.Gender_Gender_Code
								Me.myGender_Gender_Code = Value
								MyBase.RaiseGender_Gender_CodeChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private myPersonHasParents As Nullable(Of Boolean)
			Public Overrides Property PersonHasParents() As Nullable(Of Boolean)
				Get
					Return Me.myPersonHasParents
				End Get
				Set(ByVal Value As Nullable(Of Boolean))
					If Not (Object.Equals(Me.PersonHasParents, Value)) Then
						If Me.Context.OnPersonPersonHasParentsChanging(Me, Value) Then
							If MyBase.RaisePersonHasParentsChangingEvent(Value) Then
								Dim oldValue As Nullable(Of Boolean) = Me.PersonHasParents
								Me.myPersonHasParents = Value
								MyBase.RaisePersonHasParentsChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private myValueType1DoesSomethingElseWith As ValueType1
			Public Overrides Property ValueType1DoesSomethingElseWith() As ValueType1
				Get
					Return Me.myValueType1DoesSomethingElseWith
				End Get
				Set(ByVal Value As ValueType1)
					If Not (Object.Equals(Me.ValueType1DoesSomethingElseWith, Value)) Then
						If Me.Context.OnPersonValueType1DoesSomethingElseWithChanging(Me, Value) Then
							If MyBase.RaiseValueType1DoesSomethingElseWithChangingEvent(Value) Then
								Dim oldValue As ValueType1 = Me.ValueType1DoesSomethingElseWith
								Me.myValueType1DoesSomethingElseWith = Value
								Me.Context.OnPersonValueType1DoesSomethingElseWithChanged(Me, oldValue)
								MyBase.RaiseValueType1DoesSomethingElseWithChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private myMalePerson As MalePerson
			Public Overrides Property MalePerson() As MalePerson
				Get
					Return Me.myMalePerson
				End Get
				Set(ByVal Value As MalePerson)
					If Not (Object.Equals(Me.MalePerson, Value)) Then
						If Me.Context.OnPersonMalePersonChanging(Me, Value) Then
							If MyBase.RaiseMalePersonChangingEvent(Value) Then
								Dim oldValue As MalePerson = Me.MalePerson
								Me.myMalePerson = Value
								Me.Context.OnPersonMalePersonChanged(Me, oldValue)
								MyBase.RaiseMalePersonChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private myFemalePerson As FemalePerson
			Public Overrides Property FemalePerson() As FemalePerson
				Get
					Return Me.myFemalePerson
				End Get
				Set(ByVal Value As FemalePerson)
					If Not (Object.Equals(Me.FemalePerson, Value)) Then
						If Me.Context.OnPersonFemalePersonChanging(Me, Value) Then
							If MyBase.RaiseFemalePersonChangingEvent(Value) Then
								Dim oldValue As FemalePerson = Me.FemalePerson
								Me.myFemalePerson = Value
								Me.Context.OnPersonFemalePersonChanged(Me, oldValue)
								MyBase.RaiseFemalePersonChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private myChildPerson As ChildPerson
			Public Overrides Property ChildPerson() As ChildPerson
				Get
					Return Me.myChildPerson
				End Get
				Set(ByVal Value As ChildPerson)
					If Not (Object.Equals(Me.ChildPerson, Value)) Then
						If Me.Context.OnPersonChildPersonChanging(Me, Value) Then
							If MyBase.RaiseChildPersonChangingEvent(Value) Then
								Dim oldValue As ChildPerson = Me.ChildPerson
								Me.myChildPerson = Value
								Me.Context.OnPersonChildPersonChanged(Me, oldValue)
								MyBase.RaiseChildPersonChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private myDeath As Death
			Public Overrides Property Death() As Death
				Get
					Return Me.myDeath
				End Get
				Set(ByVal Value As Death)
					If Not (Object.Equals(Me.Death, Value)) Then
						If Me.Context.OnPersonDeathChanging(Me, Value) Then
							If MyBase.RaiseDeathChangingEvent(Value) Then
								Dim oldValue As Death = Me.Death
								Me.myDeath = Value
								Me.Context.OnPersonDeathChanged(Me, oldValue)
								MyBase.RaiseDeathChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private ReadOnly myPersonDrivesCarAsDrivenByPerson As ICollection(Of PersonDrivesCar)
			Public Overrides ReadOnly Property PersonDrivesCarAsDrivenByPerson() As ICollection(Of PersonDrivesCar)
				Get
					Return Me.myPersonDrivesCarAsDrivenByPerson
				End Get
			End Property
			Private ReadOnly myPersonBoughtCarFromPersonOnDateAsBuyer As ICollection(Of PersonBoughtCarFromPersonOnDate)
			Public Overrides ReadOnly Property PersonBoughtCarFromPersonOnDateAsBuyer() As ICollection(Of PersonBoughtCarFromPersonOnDate)
				Get
					Return Me.myPersonBoughtCarFromPersonOnDateAsBuyer
				End Get
			End Property
			Private ReadOnly myPersonBoughtCarFromPersonOnDateAsSeller As ICollection(Of PersonBoughtCarFromPersonOnDate)
			Public Overrides ReadOnly Property PersonBoughtCarFromPersonOnDateAsSeller() As ICollection(Of PersonBoughtCarFromPersonOnDate)
				Get
					Return Me.myPersonBoughtCarFromPersonOnDateAsSeller
				End Get
			End Property
			Private ReadOnly myPersonHasNickNameAsPerson As ICollection(Of PersonHasNickName)
			Public Overrides ReadOnly Property PersonHasNickNameAsPerson() As ICollection(Of PersonHasNickName)
				Get
					Return Me.myPersonHasNickNameAsPerson
				End Get
			End Property
			Private ReadOnly myTask As ICollection(Of Task)
			Public Overrides ReadOnly Property Task() As ICollection(Of Task)
				Get
					Return Me.myTask
				End Get
			End Property
			Private ReadOnly myValueType1DoesSomethingWith As ICollection(Of ValueType1)
			Public Overrides ReadOnly Property ValueType1DoesSomethingWith() As ICollection(Of ValueType1)
				Get
					Return Me.myValueType1DoesSomethingWith
				End Get
			End Property
		End Class
		#End Region
		<SuppressMessageAttribute("Microsoft.Usage", "CA2208")> _
		<SuppressMessageAttribute("Microsoft.Globalization", "CA1303")> _
		Private Function OnMalePersonPersonChanging(ByVal instance As MalePerson, ByVal newValue As Person) As Boolean
			If Me IsNot newValue.Context Then
				Throw New ArgumentException("All objects in a relationship must be part of the same Context.", "value")
			End If
			Return True
		End Function
		Private Overloads Sub OnMalePersonPersonChanged(ByVal instance As MalePerson, ByVal oldValue As Person)
			instance.Person.MalePerson = instance
			If oldValue IsNot Nothing Then
				oldValue.MalePerson = Nothing
			Else
			End If
		End Sub
		Private Function OnMalePersonChildPersonAdding(ByVal instance As MalePerson, ByVal value As ChildPerson) As Boolean
			Return True
		End Function
		Private Sub OnMalePersonChildPersonAdded(ByVal instance As MalePerson, ByVal value As ChildPerson)
			If value IsNot Nothing Then
				value.Father = instance
			End If
		End Sub
		Private Function OnMalePersonChildPersonRemoving(ByVal instance As MalePerson, ByVal value As ChildPerson) As Boolean
			Return True
		End Function
		Private Sub OnMalePersonChildPersonRemoved(ByVal instance As MalePerson, ByVal value As ChildPerson)
			If value IsNot Nothing Then
				value.Father = Nothing
			End If
		End Sub
		Public Function CreateMalePerson(ByVal Person As Person) As MalePerson
			If Not (Me.IsDeserializing) Then
				If Not (Me.OnMalePersonPersonChanging(Nothing, Person)) Then
					Throw New ArgumentException("Argument failed constraint enforcement.", "Person")
				End If
			End If
			Return New MalePersonCore(Me, Person)
		End Function
		Private ReadOnly myMalePersonList As List(Of MalePerson)
		Private ReadOnly myMalePersonReadOnlyCollection As ReadOnlyCollection(Of MalePerson)
		Public ReadOnly Property MalePersonCollection() As ReadOnlyCollection(Of MalePerson) Implements _
			ISampleModelContext.MalePersonCollection
			Get
				Return Me.myMalePersonReadOnlyCollection
			End Get
		End Property
		#Region "MalePersonCore"
		Private NotInheritable Class MalePersonCore
			Inherits MalePerson
			Public Sub New(ByVal context As SampleModelContext, ByVal Person As Person)
				Me.myContext = context
				Me.myChildPerson = New ConstraintEnforcementCollection(Of MalePerson, ChildPerson)(Me, New PotentialCollectionModificationCallback(Of MalePerson, ChildPerson)(context.OnMalePersonChildPersonAdding), New CommittedCollectionModificationCallback(Of MalePerson, ChildPerson)(context.OnMalePersonChildPersonAdded), New PotentialCollectionModificationCallback(Of MalePerson, ChildPerson)(context.OnMalePersonChildPersonRemoving), New CommittedCollectionModificationCallback(Of MalePerson, ChildPerson)(context.OnMalePersonChildPersonRemoved))
				Me.myPerson = Person
				context.OnMalePersonPersonChanged(Me, Nothing)
				context.myMalePersonList.Add(Me)
			End Sub
			Private ReadOnly myContext As SampleModelContext
			Public Overrides ReadOnly Property Context() As SampleModelContext
				Get
					Return Me.myContext
				End Get
			End Property
			Private myPerson As Person
			Public Overrides Property Person() As Person
				Get
					Return Me.myPerson
				End Get
				Set(ByVal Value As Person)
					If Value Is Nothing Then
						Return
					End If
					If Not (Object.Equals(Me.Person, Value)) Then
						If Me.Context.OnMalePersonPersonChanging(Me, Value) Then
							If MyBase.RaisePersonChangingEvent(Value) Then
								Dim oldValue As Person = Me.Person
								Me.myPerson = Value
								Me.Context.OnMalePersonPersonChanged(Me, oldValue)
								MyBase.RaisePersonChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private ReadOnly myChildPerson As ICollection(Of ChildPerson)
			Public Overrides ReadOnly Property ChildPerson() As ICollection(Of ChildPerson)
				Get
					Return Me.myChildPerson
				End Get
			End Property
		End Class
		#End Region
		<SuppressMessageAttribute("Microsoft.Usage", "CA2208")> _
		<SuppressMessageAttribute("Microsoft.Globalization", "CA1303")> _
		Private Function OnFemalePersonPersonChanging(ByVal instance As FemalePerson, ByVal newValue As Person) As Boolean
			If Me IsNot newValue.Context Then
				Throw New ArgumentException("All objects in a relationship must be part of the same Context.", "value")
			End If
			Return True
		End Function
		Private Overloads Sub OnFemalePersonPersonChanged(ByVal instance As FemalePerson, ByVal oldValue As Person)
			instance.Person.FemalePerson = instance
			If oldValue IsNot Nothing Then
				oldValue.FemalePerson = Nothing
			Else
			End If
		End Sub
		Private Function OnFemalePersonChildPersonAdding(ByVal instance As FemalePerson, ByVal value As ChildPerson) As Boolean
			Return True
		End Function
		Private Sub OnFemalePersonChildPersonAdded(ByVal instance As FemalePerson, ByVal value As ChildPerson)
			If value IsNot Nothing Then
				value.Mother = instance
			End If
		End Sub
		Private Function OnFemalePersonChildPersonRemoving(ByVal instance As FemalePerson, ByVal value As ChildPerson) As Boolean
			Return True
		End Function
		Private Sub OnFemalePersonChildPersonRemoved(ByVal instance As FemalePerson, ByVal value As ChildPerson)
			If value IsNot Nothing Then
				value.Mother = Nothing
			End If
		End Sub
		Public Function CreateFemalePerson(ByVal Person As Person) As FemalePerson
			If Not (Me.IsDeserializing) Then
				If Not (Me.OnFemalePersonPersonChanging(Nothing, Person)) Then
					Throw New ArgumentException("Argument failed constraint enforcement.", "Person")
				End If
			End If
			Return New FemalePersonCore(Me, Person)
		End Function
		Private ReadOnly myFemalePersonList As List(Of FemalePerson)
		Private ReadOnly myFemalePersonReadOnlyCollection As ReadOnlyCollection(Of FemalePerson)
		Public ReadOnly Property FemalePersonCollection() As ReadOnlyCollection(Of FemalePerson) Implements _
			ISampleModelContext.FemalePersonCollection
			Get
				Return Me.myFemalePersonReadOnlyCollection
			End Get
		End Property
		#Region "FemalePersonCore"
		Private NotInheritable Class FemalePersonCore
			Inherits FemalePerson
			Public Sub New(ByVal context As SampleModelContext, ByVal Person As Person)
				Me.myContext = context
				Me.myChildPerson = New ConstraintEnforcementCollection(Of FemalePerson, ChildPerson)(Me, New PotentialCollectionModificationCallback(Of FemalePerson, ChildPerson)(context.OnFemalePersonChildPersonAdding), New CommittedCollectionModificationCallback(Of FemalePerson, ChildPerson)(context.OnFemalePersonChildPersonAdded), New PotentialCollectionModificationCallback(Of FemalePerson, ChildPerson)(context.OnFemalePersonChildPersonRemoving), New CommittedCollectionModificationCallback(Of FemalePerson, ChildPerson)(context.OnFemalePersonChildPersonRemoved))
				Me.myPerson = Person
				context.OnFemalePersonPersonChanged(Me, Nothing)
				context.myFemalePersonList.Add(Me)
			End Sub
			Private ReadOnly myContext As SampleModelContext
			Public Overrides ReadOnly Property Context() As SampleModelContext
				Get
					Return Me.myContext
				End Get
			End Property
			Private myPerson As Person
			Public Overrides Property Person() As Person
				Get
					Return Me.myPerson
				End Get
				Set(ByVal Value As Person)
					If Value Is Nothing Then
						Return
					End If
					If Not (Object.Equals(Me.Person, Value)) Then
						If Me.Context.OnFemalePersonPersonChanging(Me, Value) Then
							If MyBase.RaisePersonChangingEvent(Value) Then
								Dim oldValue As Person = Me.Person
								Me.myPerson = Value
								Me.Context.OnFemalePersonPersonChanged(Me, oldValue)
								MyBase.RaisePersonChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private ReadOnly myChildPerson As ICollection(Of ChildPerson)
			Public Overrides ReadOnly Property ChildPerson() As ICollection(Of ChildPerson)
				Get
					Return Me.myChildPerson
				End Get
			End Property
		End Class
		#End Region
		Private Function OnChildPersonBirthOrder_BirthOrder_NrChanging(ByVal instance As ChildPerson, ByVal newValue As Integer) As Boolean
			If instance IsNot Nothing Then
				If Not (Me.OnExternalUniquenessConstraint3Changing(instance, Tuple.CreateTuple(Of MalePerson, Integer, FemalePerson)(instance.Father, newValue, instance.Mother))) Then
					Return False
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnChildPersonBirthOrder_BirthOrder_NrChanged(ByVal instance As ChildPerson, ByVal oldValue As Nullable(Of Integer))
			Dim ExternalUniquenessConstraint3OldValueTuple As Tuple(Of MalePerson, Integer, FemalePerson)
			If oldValue IsNot Nothing Then
				ExternalUniquenessConstraint3OldValueTuple = Tuple.CreateTuple(Of MalePerson, Integer, FemalePerson)(instance.Father, oldValue.Value, instance.Mother)
			Else
				ExternalUniquenessConstraint3OldValueTuple = Nothing
			End If
			Me.OnExternalUniquenessConstraint3Changed(instance, ExternalUniquenessConstraint3OldValueTuple, Tuple.CreateTuple(Of MalePerson, Integer, FemalePerson)(instance.Father, instance.BirthOrder_BirthOrder_Nr, instance.Mother))
		End Sub
		<SuppressMessageAttribute("Microsoft.Usage", "CA2208")> _
		<SuppressMessageAttribute("Microsoft.Globalization", "CA1303")> _
		Private Function OnChildPersonFatherChanging(ByVal instance As ChildPerson, ByVal newValue As MalePerson) As Boolean
			If Me IsNot newValue.Context Then
				Throw New ArgumentException("All objects in a relationship must be part of the same Context.", "value")
			End If
			If instance IsNot Nothing Then
				If Not (Me.OnExternalUniquenessConstraint3Changing(instance, Tuple.CreateTuple(Of MalePerson, Integer, FemalePerson)(newValue, instance.BirthOrder_BirthOrder_Nr, instance.Mother))) Then
					Return False
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnChildPersonFatherChanged(ByVal instance As ChildPerson, ByVal oldValue As MalePerson)
			instance.Father.ChildPerson.Add(instance)
			Dim ExternalUniquenessConstraint3OldValueTuple As Tuple(Of MalePerson, Integer, FemalePerson)
			If oldValue IsNot Nothing Then
				oldValue.ChildPerson.Remove(instance)
				ExternalUniquenessConstraint3OldValueTuple = Tuple.CreateTuple(Of MalePerson, Integer, FemalePerson)(oldValue, instance.BirthOrder_BirthOrder_Nr, instance.Mother)
			Else
				ExternalUniquenessConstraint3OldValueTuple = Nothing
			End If
			Me.OnExternalUniquenessConstraint3Changed(instance, ExternalUniquenessConstraint3OldValueTuple, Tuple.CreateTuple(Of MalePerson, Integer, FemalePerson)(instance.Father, instance.BirthOrder_BirthOrder_Nr, instance.Mother))
		End Sub
		<SuppressMessageAttribute("Microsoft.Usage", "CA2208")> _
		<SuppressMessageAttribute("Microsoft.Globalization", "CA1303")> _
		Private Function OnChildPersonMotherChanging(ByVal instance As ChildPerson, ByVal newValue As FemalePerson) As Boolean
			If Me IsNot newValue.Context Then
				Throw New ArgumentException("All objects in a relationship must be part of the same Context.", "value")
			End If
			If instance IsNot Nothing Then
				If Not (Me.OnExternalUniquenessConstraint3Changing(instance, Tuple.CreateTuple(Of MalePerson, Integer, FemalePerson)(instance.Father, instance.BirthOrder_BirthOrder_Nr, newValue))) Then
					Return False
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnChildPersonMotherChanged(ByVal instance As ChildPerson, ByVal oldValue As FemalePerson)
			instance.Mother.ChildPerson.Add(instance)
			Dim ExternalUniquenessConstraint3OldValueTuple As Tuple(Of MalePerson, Integer, FemalePerson)
			If oldValue IsNot Nothing Then
				oldValue.ChildPerson.Remove(instance)
				ExternalUniquenessConstraint3OldValueTuple = Tuple.CreateTuple(Of MalePerson, Integer, FemalePerson)(instance.Father, instance.BirthOrder_BirthOrder_Nr, oldValue)
			Else
				ExternalUniquenessConstraint3OldValueTuple = Nothing
			End If
			Me.OnExternalUniquenessConstraint3Changed(instance, ExternalUniquenessConstraint3OldValueTuple, Tuple.CreateTuple(Of MalePerson, Integer, FemalePerson)(instance.Father, instance.BirthOrder_BirthOrder_Nr, instance.Mother))
		End Sub
		<SuppressMessageAttribute("Microsoft.Usage", "CA2208")> _
		<SuppressMessageAttribute("Microsoft.Globalization", "CA1303")> _
		Private Function OnChildPersonPersonChanging(ByVal instance As ChildPerson, ByVal newValue As Person) As Boolean
			If Me IsNot newValue.Context Then
				Throw New ArgumentException("All objects in a relationship must be part of the same Context.", "value")
			End If
			Return True
		End Function
		Private Overloads Sub OnChildPersonPersonChanged(ByVal instance As ChildPerson, ByVal oldValue As Person)
			instance.Person.ChildPerson = instance
			If oldValue IsNot Nothing Then
				oldValue.ChildPerson = Nothing
			Else
			End If
		End Sub
		Public Function CreateChildPerson(ByVal BirthOrder_BirthOrder_Nr As Integer, ByVal Father As MalePerson, ByVal Mother As FemalePerson, ByVal Person As Person) As ChildPerson
			If Not (Me.IsDeserializing) Then
				If Not (Me.OnChildPersonBirthOrder_BirthOrder_NrChanging(Nothing, BirthOrder_BirthOrder_Nr)) Then
					Throw New ArgumentException("Argument failed constraint enforcement.", "BirthOrder_BirthOrder_Nr")
				End If
				If Not (Me.OnChildPersonFatherChanging(Nothing, Father)) Then
					Throw New ArgumentException("Argument failed constraint enforcement.", "Father")
				End If
				If Not (Me.OnChildPersonMotherChanging(Nothing, Mother)) Then
					Throw New ArgumentException("Argument failed constraint enforcement.", "Mother")
				End If
				If Not (Me.OnChildPersonPersonChanging(Nothing, Person)) Then
					Throw New ArgumentException("Argument failed constraint enforcement.", "Person")
				End If
			End If
			Return New ChildPersonCore(Me, BirthOrder_BirthOrder_Nr, Father, Mother, Person)
		End Function
		Private ReadOnly myChildPersonList As List(Of ChildPerson)
		Private ReadOnly myChildPersonReadOnlyCollection As ReadOnlyCollection(Of ChildPerson)
		Public ReadOnly Property ChildPersonCollection() As ReadOnlyCollection(Of ChildPerson) Implements _
			ISampleModelContext.ChildPersonCollection
			Get
				Return Me.myChildPersonReadOnlyCollection
			End Get
		End Property
		#Region "ChildPersonCore"
		Private NotInheritable Class ChildPersonCore
			Inherits ChildPerson
			Public Sub New(ByVal context As SampleModelContext, ByVal BirthOrder_BirthOrder_Nr As Integer, ByVal Father As MalePerson, ByVal Mother As FemalePerson, ByVal Person As Person)
				Me.myContext = context
				Me.myBirthOrder_BirthOrder_Nr = BirthOrder_BirthOrder_Nr
				context.OnChildPersonBirthOrder_BirthOrder_NrChanged(Me, Nothing)
				Me.myFather = Father
				context.OnChildPersonFatherChanged(Me, Nothing)
				Me.myMother = Mother
				context.OnChildPersonMotherChanged(Me, Nothing)
				Me.myPerson = Person
				context.OnChildPersonPersonChanged(Me, Nothing)
				context.myChildPersonList.Add(Me)
			End Sub
			Private ReadOnly myContext As SampleModelContext
			Public Overrides ReadOnly Property Context() As SampleModelContext
				Get
					Return Me.myContext
				End Get
			End Property
			Private myBirthOrder_BirthOrder_Nr As Integer
			Public Overrides Property BirthOrder_BirthOrder_Nr() As Integer
				Get
					Return Me.myBirthOrder_BirthOrder_Nr
				End Get
				Set(ByVal Value As Integer)
					If Not (Object.Equals(Me.BirthOrder_BirthOrder_Nr, Value)) Then
						If Me.Context.OnChildPersonBirthOrder_BirthOrder_NrChanging(Me, Value) Then
							If MyBase.RaiseBirthOrder_BirthOrder_NrChangingEvent(Value) Then
								Dim oldValue As Integer = Me.BirthOrder_BirthOrder_Nr
								Me.myBirthOrder_BirthOrder_Nr = Value
								Me.Context.OnChildPersonBirthOrder_BirthOrder_NrChanged(Me, oldValue)
								MyBase.RaiseBirthOrder_BirthOrder_NrChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private myFather As MalePerson
			Public Overrides Property Father() As MalePerson
				Get
					Return Me.myFather
				End Get
				Set(ByVal Value As MalePerson)
					If Value Is Nothing Then
						Return
					End If
					If Not (Object.Equals(Me.Father, Value)) Then
						If Me.Context.OnChildPersonFatherChanging(Me, Value) Then
							If MyBase.RaiseFatherChangingEvent(Value) Then
								Dim oldValue As MalePerson = Me.Father
								Me.myFather = Value
								Me.Context.OnChildPersonFatherChanged(Me, oldValue)
								MyBase.RaiseFatherChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private myMother As FemalePerson
			Public Overrides Property Mother() As FemalePerson
				Get
					Return Me.myMother
				End Get
				Set(ByVal Value As FemalePerson)
					If Value Is Nothing Then
						Return
					End If
					If Not (Object.Equals(Me.Mother, Value)) Then
						If Me.Context.OnChildPersonMotherChanging(Me, Value) Then
							If MyBase.RaiseMotherChangingEvent(Value) Then
								Dim oldValue As FemalePerson = Me.Mother
								Me.myMother = Value
								Me.Context.OnChildPersonMotherChanged(Me, oldValue)
								MyBase.RaiseMotherChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private myPerson As Person
			Public Overrides Property Person() As Person
				Get
					Return Me.myPerson
				End Get
				Set(ByVal Value As Person)
					If Value Is Nothing Then
						Return
					End If
					If Not (Object.Equals(Me.Person, Value)) Then
						If Me.Context.OnChildPersonPersonChanging(Me, Value) Then
							If MyBase.RaisePersonChangingEvent(Value) Then
								Dim oldValue As Person = Me.Person
								Me.myPerson = Value
								Me.Context.OnChildPersonPersonChanged(Me, oldValue)
								MyBase.RaisePersonChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
		End Class
		#End Region
		Private Function OnDeathDate_YMDChanging(ByVal instance As Death, ByVal newValue As Nullable(Of Integer)) As Boolean
			Return True
		End Function
		Private Function OnDeathDeathCause_DeathCause_TypeChanging(ByVal instance As Death, ByVal newValue As String) As Boolean
			Return True
		End Function
		<SuppressMessageAttribute("Microsoft.Usage", "CA2208")> _
		<SuppressMessageAttribute("Microsoft.Globalization", "CA1303")> _
		Private Function OnDeathNaturalDeathChanging(ByVal instance As Death, ByVal newValue As NaturalDeath) As Boolean
			If newValue IsNot Nothing Then
				If Me IsNot newValue.Context Then
					Throw New ArgumentException("All objects in a relationship must be part of the same Context.", "value")
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnDeathNaturalDeathChanged(ByVal instance As Death, ByVal oldValue As NaturalDeath)
			If instance.NaturalDeath IsNot Nothing Then
				instance.NaturalDeath.Death = instance
			End If
			If oldValue IsNot Nothing Then
				oldValue.Death = Nothing
			Else
			End If
		End Sub
		<SuppressMessageAttribute("Microsoft.Usage", "CA2208")> _
		<SuppressMessageAttribute("Microsoft.Globalization", "CA1303")> _
		Private Function OnDeathUnnaturalDeathChanging(ByVal instance As Death, ByVal newValue As UnnaturalDeath) As Boolean
			If newValue IsNot Nothing Then
				If Me IsNot newValue.Context Then
					Throw New ArgumentException("All objects in a relationship must be part of the same Context.", "value")
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnDeathUnnaturalDeathChanged(ByVal instance As Death, ByVal oldValue As UnnaturalDeath)
			If instance.UnnaturalDeath IsNot Nothing Then
				instance.UnnaturalDeath.Death = instance
			End If
			If oldValue IsNot Nothing Then
				oldValue.Death = Nothing
			Else
			End If
		End Sub
		<SuppressMessageAttribute("Microsoft.Usage", "CA2208")> _
		<SuppressMessageAttribute("Microsoft.Globalization", "CA1303")> _
		Private Function OnDeathPersonChanging(ByVal instance As Death, ByVal newValue As Person) As Boolean
			If Me IsNot newValue.Context Then
				Throw New ArgumentException("All objects in a relationship must be part of the same Context.", "value")
			End If
			Return True
		End Function
		Private Overloads Sub OnDeathPersonChanged(ByVal instance As Death, ByVal oldValue As Person)
			instance.Person.Death = instance
			If oldValue IsNot Nothing Then
				oldValue.Death = Nothing
			Else
			End If
		End Sub
		Public Function CreateDeath(ByVal DeathCause_DeathCause_Type As String, ByVal Person As Person) As Death
			If Not (Me.IsDeserializing) Then
				If Not (Me.OnDeathDeathCause_DeathCause_TypeChanging(Nothing, DeathCause_DeathCause_Type)) Then
					Throw New ArgumentException("Argument failed constraint enforcement.", "DeathCause_DeathCause_Type")
				End If
				If Not (Me.OnDeathPersonChanging(Nothing, Person)) Then
					Throw New ArgumentException("Argument failed constraint enforcement.", "Person")
				End If
			End If
			Return New DeathCore(Me, DeathCause_DeathCause_Type, Person)
		End Function
		Private ReadOnly myDeathList As List(Of Death)
		Private ReadOnly myDeathReadOnlyCollection As ReadOnlyCollection(Of Death)
		Public ReadOnly Property DeathCollection() As ReadOnlyCollection(Of Death) Implements _
			ISampleModelContext.DeathCollection
			Get
				Return Me.myDeathReadOnlyCollection
			End Get
		End Property
		#Region "DeathCore"
		Private NotInheritable Class DeathCore
			Inherits Death
			Public Sub New(ByVal context As SampleModelContext, ByVal DeathCause_DeathCause_Type As String, ByVal Person As Person)
				Me.myContext = context
				Me.myDeathCause_DeathCause_Type = DeathCause_DeathCause_Type
				Me.myPerson = Person
				context.OnDeathPersonChanged(Me, Nothing)
				context.myDeathList.Add(Me)
			End Sub
			Private ReadOnly myContext As SampleModelContext
			Public Overrides ReadOnly Property Context() As SampleModelContext
				Get
					Return Me.myContext
				End Get
			End Property
			Private myDate_YMD As Nullable(Of Integer)
			Public Overrides Property Date_YMD() As Nullable(Of Integer)
				Get
					Return Me.myDate_YMD
				End Get
				Set(ByVal Value As Nullable(Of Integer))
					If Not (Object.Equals(Me.Date_YMD, Value)) Then
						If Me.Context.OnDeathDate_YMDChanging(Me, Value) Then
							If MyBase.RaiseDate_YMDChangingEvent(Value) Then
								Dim oldValue As Nullable(Of Integer) = Me.Date_YMD
								Me.myDate_YMD = Value
								MyBase.RaiseDate_YMDChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private myDeathCause_DeathCause_Type As String
			Public Overrides Property DeathCause_DeathCause_Type() As String
				Get
					Return Me.myDeathCause_DeathCause_Type
				End Get
				Set(ByVal Value As String)
					If Value Is Nothing Then
						Return
					End If
					If Not (Object.Equals(Me.DeathCause_DeathCause_Type, Value)) Then
						If Me.Context.OnDeathDeathCause_DeathCause_TypeChanging(Me, Value) Then
							If MyBase.RaiseDeathCause_DeathCause_TypeChangingEvent(Value) Then
								Dim oldValue As String = Me.DeathCause_DeathCause_Type
								Me.myDeathCause_DeathCause_Type = Value
								MyBase.RaiseDeathCause_DeathCause_TypeChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private myNaturalDeath As NaturalDeath
			Public Overrides Property NaturalDeath() As NaturalDeath
				Get
					Return Me.myNaturalDeath
				End Get
				Set(ByVal Value As NaturalDeath)
					If Not (Object.Equals(Me.NaturalDeath, Value)) Then
						If Me.Context.OnDeathNaturalDeathChanging(Me, Value) Then
							If MyBase.RaiseNaturalDeathChangingEvent(Value) Then
								Dim oldValue As NaturalDeath = Me.NaturalDeath
								Me.myNaturalDeath = Value
								Me.Context.OnDeathNaturalDeathChanged(Me, oldValue)
								MyBase.RaiseNaturalDeathChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private myUnnaturalDeath As UnnaturalDeath
			Public Overrides Property UnnaturalDeath() As UnnaturalDeath
				Get
					Return Me.myUnnaturalDeath
				End Get
				Set(ByVal Value As UnnaturalDeath)
					If Not (Object.Equals(Me.UnnaturalDeath, Value)) Then
						If Me.Context.OnDeathUnnaturalDeathChanging(Me, Value) Then
							If MyBase.RaiseUnnaturalDeathChangingEvent(Value) Then
								Dim oldValue As UnnaturalDeath = Me.UnnaturalDeath
								Me.myUnnaturalDeath = Value
								Me.Context.OnDeathUnnaturalDeathChanged(Me, oldValue)
								MyBase.RaiseUnnaturalDeathChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private myPerson As Person
			Public Overrides Property Person() As Person
				Get
					Return Me.myPerson
				End Get
				Set(ByVal Value As Person)
					If Value Is Nothing Then
						Return
					End If
					If Not (Object.Equals(Me.Person, Value)) Then
						If Me.Context.OnDeathPersonChanging(Me, Value) Then
							If MyBase.RaisePersonChangingEvent(Value) Then
								Dim oldValue As Person = Me.Person
								Me.myPerson = Value
								Me.Context.OnDeathPersonChanged(Me, oldValue)
								MyBase.RaisePersonChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
		End Class
		#End Region
		Private Function OnNaturalDeathNaturalDeathIsFromProstateCancerChanging(ByVal instance As NaturalDeath, ByVal newValue As Nullable(Of Boolean)) As Boolean
			Return True
		End Function
		<SuppressMessageAttribute("Microsoft.Usage", "CA2208")> _
		<SuppressMessageAttribute("Microsoft.Globalization", "CA1303")> _
		Private Function OnNaturalDeathDeathChanging(ByVal instance As NaturalDeath, ByVal newValue As Death) As Boolean
			If Me IsNot newValue.Context Then
				Throw New ArgumentException("All objects in a relationship must be part of the same Context.", "value")
			End If
			Return True
		End Function
		Private Overloads Sub OnNaturalDeathDeathChanged(ByVal instance As NaturalDeath, ByVal oldValue As Death)
			instance.Death.NaturalDeath = instance
			If oldValue IsNot Nothing Then
				oldValue.NaturalDeath = Nothing
			Else
			End If
		End Sub
		Public Function CreateNaturalDeath(ByVal Death As Death) As NaturalDeath
			If Not (Me.IsDeserializing) Then
				If Not (Me.OnNaturalDeathDeathChanging(Nothing, Death)) Then
					Throw New ArgumentException("Argument failed constraint enforcement.", "Death")
				End If
			End If
			Return New NaturalDeathCore(Me, Death)
		End Function
		Private ReadOnly myNaturalDeathList As List(Of NaturalDeath)
		Private ReadOnly myNaturalDeathReadOnlyCollection As ReadOnlyCollection(Of NaturalDeath)
		Public ReadOnly Property NaturalDeathCollection() As ReadOnlyCollection(Of NaturalDeath) Implements _
			ISampleModelContext.NaturalDeathCollection
			Get
				Return Me.myNaturalDeathReadOnlyCollection
			End Get
		End Property
		#Region "NaturalDeathCore"
		Private NotInheritable Class NaturalDeathCore
			Inherits NaturalDeath
			Public Sub New(ByVal context As SampleModelContext, ByVal Death As Death)
				Me.myContext = context
				Me.myDeath = Death
				context.OnNaturalDeathDeathChanged(Me, Nothing)
				context.myNaturalDeathList.Add(Me)
			End Sub
			Private ReadOnly myContext As SampleModelContext
			Public Overrides ReadOnly Property Context() As SampleModelContext
				Get
					Return Me.myContext
				End Get
			End Property
			Private myNaturalDeathIsFromProstateCancer As Nullable(Of Boolean)
			Public Overrides Property NaturalDeathIsFromProstateCancer() As Nullable(Of Boolean)
				Get
					Return Me.myNaturalDeathIsFromProstateCancer
				End Get
				Set(ByVal Value As Nullable(Of Boolean))
					If Not (Object.Equals(Me.NaturalDeathIsFromProstateCancer, Value)) Then
						If Me.Context.OnNaturalDeathNaturalDeathIsFromProstateCancerChanging(Me, Value) Then
							If MyBase.RaiseNaturalDeathIsFromProstateCancerChangingEvent(Value) Then
								Dim oldValue As Nullable(Of Boolean) = Me.NaturalDeathIsFromProstateCancer
								Me.myNaturalDeathIsFromProstateCancer = Value
								MyBase.RaiseNaturalDeathIsFromProstateCancerChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private myDeath As Death
			Public Overrides Property Death() As Death
				Get
					Return Me.myDeath
				End Get
				Set(ByVal Value As Death)
					If Value Is Nothing Then
						Return
					End If
					If Not (Object.Equals(Me.Death, Value)) Then
						If Me.Context.OnNaturalDeathDeathChanging(Me, Value) Then
							If MyBase.RaiseDeathChangingEvent(Value) Then
								Dim oldValue As Death = Me.Death
								Me.myDeath = Value
								Me.Context.OnNaturalDeathDeathChanged(Me, oldValue)
								MyBase.RaiseDeathChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
		End Class
		#End Region
		Private Function OnUnnaturalDeathUnnaturalDeathIsViolentChanging(ByVal instance As UnnaturalDeath, ByVal newValue As Nullable(Of Boolean)) As Boolean
			Return True
		End Function
		Private Function OnUnnaturalDeathUnnaturalDeathIsBloodyChanging(ByVal instance As UnnaturalDeath, ByVal newValue As Nullable(Of Boolean)) As Boolean
			Return True
		End Function
		<SuppressMessageAttribute("Microsoft.Usage", "CA2208")> _
		<SuppressMessageAttribute("Microsoft.Globalization", "CA1303")> _
		Private Function OnUnnaturalDeathDeathChanging(ByVal instance As UnnaturalDeath, ByVal newValue As Death) As Boolean
			If Me IsNot newValue.Context Then
				Throw New ArgumentException("All objects in a relationship must be part of the same Context.", "value")
			End If
			Return True
		End Function
		Private Overloads Sub OnUnnaturalDeathDeathChanged(ByVal instance As UnnaturalDeath, ByVal oldValue As Death)
			instance.Death.UnnaturalDeath = instance
			If oldValue IsNot Nothing Then
				oldValue.UnnaturalDeath = Nothing
			Else
			End If
		End Sub
		Public Function CreateUnnaturalDeath(ByVal Death As Death) As UnnaturalDeath
			If Not (Me.IsDeserializing) Then
				If Not (Me.OnUnnaturalDeathDeathChanging(Nothing, Death)) Then
					Throw New ArgumentException("Argument failed constraint enforcement.", "Death")
				End If
			End If
			Return New UnnaturalDeathCore(Me, Death)
		End Function
		Private ReadOnly myUnnaturalDeathList As List(Of UnnaturalDeath)
		Private ReadOnly myUnnaturalDeathReadOnlyCollection As ReadOnlyCollection(Of UnnaturalDeath)
		Public ReadOnly Property UnnaturalDeathCollection() As ReadOnlyCollection(Of UnnaturalDeath) Implements _
			ISampleModelContext.UnnaturalDeathCollection
			Get
				Return Me.myUnnaturalDeathReadOnlyCollection
			End Get
		End Property
		#Region "UnnaturalDeathCore"
		Private NotInheritable Class UnnaturalDeathCore
			Inherits UnnaturalDeath
			Public Sub New(ByVal context As SampleModelContext, ByVal Death As Death)
				Me.myContext = context
				Me.myDeath = Death
				context.OnUnnaturalDeathDeathChanged(Me, Nothing)
				context.myUnnaturalDeathList.Add(Me)
			End Sub
			Private ReadOnly myContext As SampleModelContext
			Public Overrides ReadOnly Property Context() As SampleModelContext
				Get
					Return Me.myContext
				End Get
			End Property
			Private myUnnaturalDeathIsViolent As Nullable(Of Boolean)
			Public Overrides Property UnnaturalDeathIsViolent() As Nullable(Of Boolean)
				Get
					Return Me.myUnnaturalDeathIsViolent
				End Get
				Set(ByVal Value As Nullable(Of Boolean))
					If Not (Object.Equals(Me.UnnaturalDeathIsViolent, Value)) Then
						If Me.Context.OnUnnaturalDeathUnnaturalDeathIsViolentChanging(Me, Value) Then
							If MyBase.RaiseUnnaturalDeathIsViolentChangingEvent(Value) Then
								Dim oldValue As Nullable(Of Boolean) = Me.UnnaturalDeathIsViolent
								Me.myUnnaturalDeathIsViolent = Value
								MyBase.RaiseUnnaturalDeathIsViolentChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private myUnnaturalDeathIsBloody As Nullable(Of Boolean)
			Public Overrides Property UnnaturalDeathIsBloody() As Nullable(Of Boolean)
				Get
					Return Me.myUnnaturalDeathIsBloody
				End Get
				Set(ByVal Value As Nullable(Of Boolean))
					If Not (Object.Equals(Me.UnnaturalDeathIsBloody, Value)) Then
						If Me.Context.OnUnnaturalDeathUnnaturalDeathIsBloodyChanging(Me, Value) Then
							If MyBase.RaiseUnnaturalDeathIsBloodyChangingEvent(Value) Then
								Dim oldValue As Nullable(Of Boolean) = Me.UnnaturalDeathIsBloody
								Me.myUnnaturalDeathIsBloody = Value
								MyBase.RaiseUnnaturalDeathIsBloodyChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private myDeath As Death
			Public Overrides Property Death() As Death
				Get
					Return Me.myDeath
				End Get
				Set(ByVal Value As Death)
					If Value Is Nothing Then
						Return
					End If
					If Not (Object.Equals(Me.Death, Value)) Then
						If Me.Context.OnUnnaturalDeathDeathChanging(Me, Value) Then
							If MyBase.RaiseDeathChangingEvent(Value) Then
								Dim oldValue As Death = Me.Death
								Me.myDeath = Value
								Me.Context.OnUnnaturalDeathDeathChanged(Me, oldValue)
								MyBase.RaiseDeathChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
		End Class
		#End Region
		<SuppressMessageAttribute("Microsoft.Usage", "CA2208")> _
		<SuppressMessageAttribute("Microsoft.Globalization", "CA1303")> _
		Private Function OnTaskPersonChanging(ByVal instance As Task, ByVal newValue As Person) As Boolean
			If newValue IsNot Nothing Then
				If Me IsNot newValue.Context Then
					Throw New ArgumentException("All objects in a relationship must be part of the same Context.", "value")
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnTaskPersonChanged(ByVal instance As Task, ByVal oldValue As Person)
			If instance.Person IsNot Nothing Then
				instance.Person.Task.Add(instance)
			End If
			If oldValue IsNot Nothing Then
				oldValue.Task.Remove(instance)
			Else
			End If
		End Sub
		Public Function CreateTask() As Task
			If Not (Me.IsDeserializing) Then
			End If
			Return New TaskCore(Me)
		End Function
		Private ReadOnly myTaskList As List(Of Task)
		Private ReadOnly myTaskReadOnlyCollection As ReadOnlyCollection(Of Task)
		Public ReadOnly Property TaskCollection() As ReadOnlyCollection(Of Task) Implements _
			ISampleModelContext.TaskCollection
			Get
				Return Me.myTaskReadOnlyCollection
			End Get
		End Property
		#Region "TaskCore"
		Private NotInheritable Class TaskCore
			Inherits Task
			Public Sub New(ByVal context As SampleModelContext)
				Me.myContext = context
				context.myTaskList.Add(Me)
			End Sub
			Private ReadOnly myContext As SampleModelContext
			Public Overrides ReadOnly Property Context() As SampleModelContext
				Get
					Return Me.myContext
				End Get
			End Property
			Private myPerson As Person
			Public Overrides Property Person() As Person
				Get
					Return Me.myPerson
				End Get
				Set(ByVal Value As Person)
					If Not (Object.Equals(Me.Person, Value)) Then
						If Me.Context.OnTaskPersonChanging(Me, Value) Then
							If MyBase.RaisePersonChangingEvent(Value) Then
								Dim oldValue As Person = Me.Person
								Me.myPerson = Value
								Me.Context.OnTaskPersonChanged(Me, oldValue)
								MyBase.RaisePersonChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
		End Class
		#End Region
		Private Function OnValueType1ValueType1ValueChanging(ByVal instance As ValueType1, ByVal newValue As Integer) As Boolean
			Dim currentInstance As ValueType1 = instance
			If Me.myValueType1ValueType1ValueDictionary.TryGetValue(newValue, currentInstance) Then
				If Not (Object.Equals(currentInstance, instance)) Then
					Return False
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnValueType1ValueType1ValueChanged(ByVal instance As ValueType1, ByVal oldValue As Nullable(Of Integer))
			Me.myValueType1ValueType1ValueDictionary.Add(instance.ValueType1Value, instance)
			If oldValue IsNot Nothing Then
				Me.myValueType1ValueType1ValueDictionary.Remove(oldValue.Value)
			Else
			End If
		End Sub
		<SuppressMessageAttribute("Microsoft.Usage", "CA2208")> _
		<SuppressMessageAttribute("Microsoft.Globalization", "CA1303")> _
		Private Function OnValueType1DoesSomethingWithPersonChanging(ByVal instance As ValueType1, ByVal newValue As Person) As Boolean
			If newValue IsNot Nothing Then
				If Me IsNot newValue.Context Then
					Throw New ArgumentException("All objects in a relationship must be part of the same Context.", "value")
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnValueType1DoesSomethingWithPersonChanged(ByVal instance As ValueType1, ByVal oldValue As Person)
			If instance.DoesSomethingWithPerson IsNot Nothing Then
				instance.DoesSomethingWithPerson.ValueType1DoesSomethingWith.Add(instance)
			End If
			If oldValue IsNot Nothing Then
				oldValue.ValueType1DoesSomethingWith.Remove(instance)
			Else
			End If
		End Sub
		Private Function OnValueType1DoesSomethingElseWithPersonAdding(ByVal instance As ValueType1, ByVal value As Person) As Boolean
			Return True
		End Function
		Private Sub OnValueType1DoesSomethingElseWithPersonAdded(ByVal instance As ValueType1, ByVal value As Person)
			If value IsNot Nothing Then
				value.ValueType1DoesSomethingElseWith = instance
			End If
		End Sub
		Private Function OnValueType1DoesSomethingElseWithPersonRemoving(ByVal instance As ValueType1, ByVal value As Person) As Boolean
			Return True
		End Function
		Private Sub OnValueType1DoesSomethingElseWithPersonRemoved(ByVal instance As ValueType1, ByVal value As Person)
			If value IsNot Nothing Then
				value.ValueType1DoesSomethingElseWith = Nothing
			End If
		End Sub
		Public Function CreateValueType1(ByVal ValueType1Value As Integer) As ValueType1
			If Not (Me.IsDeserializing) Then
				If Not (Me.OnValueType1ValueType1ValueChanging(Nothing, ValueType1Value)) Then
					Throw New ArgumentException("Argument failed constraint enforcement.", "ValueType1Value")
				End If
			End If
			Return New ValueType1Core(Me, ValueType1Value)
		End Function
		Private ReadOnly myValueType1List As List(Of ValueType1)
		Private ReadOnly myValueType1ReadOnlyCollection As ReadOnlyCollection(Of ValueType1)
		Public ReadOnly Property ValueType1Collection() As ReadOnlyCollection(Of ValueType1) Implements _
			ISampleModelContext.ValueType1Collection
			Get
				Return Me.myValueType1ReadOnlyCollection
			End Get
		End Property
		#Region "ValueType1Core"
		Private NotInheritable Class ValueType1Core
			Inherits ValueType1
			Public Sub New(ByVal context As SampleModelContext, ByVal ValueType1Value As Integer)
				Me.myContext = context
				Me.myDoesSomethingElseWithPerson = New ConstraintEnforcementCollection(Of ValueType1, Person)(Me, New PotentialCollectionModificationCallback(Of ValueType1, Person)(context.OnValueType1DoesSomethingElseWithPersonAdding), New CommittedCollectionModificationCallback(Of ValueType1, Person)(context.OnValueType1DoesSomethingElseWithPersonAdded), New PotentialCollectionModificationCallback(Of ValueType1, Person)(context.OnValueType1DoesSomethingElseWithPersonRemoving), New CommittedCollectionModificationCallback(Of ValueType1, Person)(context.OnValueType1DoesSomethingElseWithPersonRemoved))
				Me.myValueType1Value = ValueType1Value
				context.OnValueType1ValueType1ValueChanged(Me, Nothing)
				context.myValueType1List.Add(Me)
			End Sub
			Private ReadOnly myContext As SampleModelContext
			Public Overrides ReadOnly Property Context() As SampleModelContext
				Get
					Return Me.myContext
				End Get
			End Property
			Private myValueType1Value As Integer
			Public Overrides Property ValueType1Value() As Integer
				Get
					Return Me.myValueType1Value
				End Get
				Set(ByVal Value As Integer)
					If Not (Object.Equals(Me.ValueType1Value, Value)) Then
						If Me.Context.OnValueType1ValueType1ValueChanging(Me, Value) Then
							If MyBase.RaiseValueType1ValueChangingEvent(Value) Then
								Dim oldValue As Integer = Me.ValueType1Value
								Me.myValueType1Value = Value
								Me.Context.OnValueType1ValueType1ValueChanged(Me, oldValue)
								MyBase.RaiseValueType1ValueChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private myDoesSomethingWithPerson As Person
			Public Overrides Property DoesSomethingWithPerson() As Person
				Get
					Return Me.myDoesSomethingWithPerson
				End Get
				Set(ByVal Value As Person)
					If Not (Object.Equals(Me.DoesSomethingWithPerson, Value)) Then
						If Me.Context.OnValueType1DoesSomethingWithPersonChanging(Me, Value) Then
							If MyBase.RaiseDoesSomethingWithPersonChangingEvent(Value) Then
								Dim oldValue As Person = Me.DoesSomethingWithPerson
								Me.myDoesSomethingWithPerson = Value
								Me.Context.OnValueType1DoesSomethingWithPersonChanged(Me, oldValue)
								MyBase.RaiseDoesSomethingWithPersonChangedEvent(oldValue)
							End If
						End If
					End If
				End Set
			End Property
			Private ReadOnly myDoesSomethingElseWithPerson As ICollection(Of Person)
			Public Overrides ReadOnly Property DoesSomethingElseWithPerson() As ICollection(Of Person)
				Get
					Return Me.myDoesSomethingElseWithPerson
				End Get
			End Property
		End Class
		#End Region
	End Class
	#End Region
End Namespace
