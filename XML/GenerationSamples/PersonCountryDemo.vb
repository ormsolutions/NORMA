Imports System
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Xml
Imports SuppressMessageAttribute = System.Diagnostics.CodeAnalysis.SuppressMessageAttribute
Imports AccessedThroughPropertyAttribute = System.Runtime.CompilerServices.AccessedThroughPropertyAttribute
Imports GeneratedCodeAttribute = System.CodeDom.Compiler.GeneratedCodeAttribute
Imports StructLayoutAttribute = System.Runtime.InteropServices.StructLayoutAttribute
Imports LayoutKind = System.Runtime.InteropServices.LayoutKind
Imports CharSet = System.Runtime.InteropServices.CharSet
#Region "Global Support Classes"
Namespace System
	#Region "Tuple Support"
	<System.Serializable()> _
	<System.ComponentModel.ImmutableObjectAttribute(True)> _
	<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
	Public MustInherit Partial Class Tuple
		Protected Sub New()
		End Sub
		<System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow")> _
		Protected Shared Function RotateRight(ByVal value As Integer, ByVal places As Integer) As Integer
			places = places And &H1F
			If places = 0 Then
				Return value
			End If
			Dim mask As Integer = Not &H7FFFFFF >> (places - 1)
			Return ((value >> places) And Not mask) Or ((value << (32 - places)) And mask)
		End Function
		Public Overloads MustOverride Overrides Function ToString() As String
		Public Overloads MustOverride Function ToString(ByVal provider As System.IFormatProvider) As String
	End Class
	#End Region
	#Region "Binary (2-ary) Tuple"
	Public MustInherit Partial Class Tuple
		Public Overloads Shared Function CreateTuple(Of T1, T2)(ByVal item1 As T1, ByVal item2 As T2) As Tuple(Of T1, T2)
			If (item1 Is Nothing) OrElse (item2 Is Nothing) Then
				Return Nothing
			End If
			Return New Tuple(Of T1, T2)(item1, item2)
		End Function
	End Class
	<System.Serializable()> _
	<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
	Public NotInheritable Class Tuple(Of T1, T2)
		Inherits Tuple
		Implements System.IEquatable(Of Tuple(Of T1, T2))
		Private ReadOnly _item1 As T1
		Public ReadOnly Property Item1() As T1
			Get
				Return Me._item1
			End Get
		End Property
		Private ReadOnly _item2 As T2
		Public ReadOnly Property Item2() As T2
			Get
				Return Me._item2
			End Get
		End Property
		Public Sub New(ByVal item1 As T1, ByVal item2 As T2)
			If (item1 Is Nothing) OrElse (item2 Is Nothing) Then
				Throw New System.ArgumentNullException()
			End If
			Me._item1 = item1
			Me._item2 = item2
		End Sub
		Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
			Return Me.Equals(TryCast(obj, Tuple(Of T1, T2)))
		End Function
		Public Overloads Function Equals(ByVal other As Tuple(Of T1, T2)) As Boolean Implements _
			System.IEquatable(Of Tuple(Of T1, T2)).Equals
			If (CObj(other) Is Nothing) OrElse (Not (Me._item1.Equals(other._item1)) OrElse Not (Me._item2.Equals(other._item2))) Then
				Return False
			End If
			Return True
		End Function
		Public Overrides Function GetHashCode() As Integer
			Return Me._item1.GetHashCode() Xor Tuple.RotateRight(Me._item2.GetHashCode(), 1)
		End Function
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Overrides Function ToString(ByVal provider As System.IFormatProvider) As String
			Return String.Format(provider, "({1}, {2})", Me._item1, Me._item2)
		End Function
		Public Shared Operator =(ByVal tuple1 As Tuple(Of T1, T2), ByVal tuple2 As Tuple(Of T1, T2)) As Boolean
			If CObj(tuple1) Is Nothing Then
				Return CObj(tuple2) Is Nothing
			Else
				Return tuple1.Equals(tuple2)
			End If
		End Operator
		Public Shared Operator <>(ByVal tuple1 As Tuple(Of T1, T2), ByVal tuple2 As Tuple(Of T1, T2)) As Boolean
			Return Not (tuple1 = tuple2)
		End Operator
		Public Overloads Shared Widening Operator CType(ByVal tuple As Tuple(Of T1, T2)) As System.Collections.Generic.KeyValuePair(Of T1, T2)
			If CObj(tuple) Is Nothing Then
				Return Nothing
			Else
				Return New System.Collections.Generic.KeyValuePair(Of T1, T2)(tuple._item1, tuple._item2)
			End If
		End Operator
		Public Overloads Shared Narrowing Operator CType(ByVal keyValuePair As System.Collections.Generic.KeyValuePair(Of T1, T2)) As Tuple(Of T1, T2)
			If (keyValuePair.Key Is Nothing) OrElse (keyValuePair.Value Is Nothing) Then
				Throw New System.InvalidCastException()
			Else
				Return New Tuple(Of T1, T2)(keyValuePair.Key, keyValuePair.Value)
			End If
		End Operator
		Public Overloads Shared Widening Operator CType(ByVal tuple As Tuple(Of T1, T2)) As System.Collections.DictionaryEntry
			If CObj(tuple) Is Nothing Then
				Return Nothing
			Else
				Return New System.Collections.DictionaryEntry(tuple._item1, tuple._item2)
			End If
		End Operator
		Public Overloads Shared Narrowing Operator CType(ByVal dictionaryEntry As System.Collections.DictionaryEntry) As Tuple(Of T1, T2)
			Dim key As Object = dictionaryEntry.Key
			Dim value As Object = dictionaryEntry.Value
			If ((key Is Nothing) OrElse (value Is Nothing)) OrElse Not ((TypeOf key Is T1) AndAlso (TypeOf value Is T2)) Then
				Throw New System.InvalidCastException()
			Else
				Return New Tuple(Of T1, T2)(CType(key, T1), CType(value, T2))
			End If
		End Operator
	End Class
	#End Region
	#Region "Ternary (3-ary) Tuple"
	Public MustInherit Partial Class Tuple
		Public Overloads Shared Function CreateTuple(Of T1, T2, T3)(ByVal item1 As T1, ByVal item2 As T2, ByVal item3 As T3) As Tuple(Of T1, T2, T3)
			If (item1 Is Nothing) OrElse ((item2 Is Nothing) OrElse (item3 Is Nothing)) Then
				Return Nothing
			End If
			Return New Tuple(Of T1, T2, T3)(item1, item2, item3)
		End Function
	End Class
	<System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")> _
	<System.Serializable()> _
	<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
	Public NotInheritable Class Tuple(Of T1, T2, T3)
		Inherits Tuple
		Implements System.IEquatable(Of Tuple(Of T1, T2, T3))
		Private ReadOnly _item1 As T1
		Public ReadOnly Property Item1() As T1
			Get
				Return Me._item1
			End Get
		End Property
		Private ReadOnly _item2 As T2
		Public ReadOnly Property Item2() As T2
			Get
				Return Me._item2
			End Get
		End Property
		Private ReadOnly _item3 As T3
		Public ReadOnly Property Item3() As T3
			Get
				Return Me._item3
			End Get
		End Property
		Public Sub New(ByVal item1 As T1, ByVal item2 As T2, ByVal item3 As T3)
			If (item1 Is Nothing) OrElse ((item2 Is Nothing) OrElse (item3 Is Nothing)) Then
				Throw New System.ArgumentNullException()
			End If
			Me._item1 = item1
			Me._item2 = item2
			Me._item3 = item3
		End Sub
		Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
			Return Me.Equals(TryCast(obj, Tuple(Of T1, T2, T3)))
		End Function
		Public Overloads Function Equals(ByVal other As Tuple(Of T1, T2, T3)) As Boolean Implements _
			System.IEquatable(Of Tuple(Of T1, T2, T3)).Equals
			If (CObj(other) Is Nothing) OrElse (Not (Me._item1.Equals(other._item1)) OrElse (Not (Me._item2.Equals(other._item2)) OrElse Not (Me._item3.Equals(other._item3)))) Then
				Return False
			End If
			Return True
		End Function
		Public Overrides Function GetHashCode() As Integer
			Return Me._item1.GetHashCode() Xor (Tuple.RotateRight(Me._item2.GetHashCode(), 1) Xor Tuple.RotateRight(Me._item3.GetHashCode(), 2))
		End Function
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Overrides Function ToString(ByVal provider As System.IFormatProvider) As String
			Return String.Format(provider, "({1}, {2}, {3})", Me._item1, Me._item2, Me._item3)
		End Function
		Public Shared Operator =(ByVal tuple1 As Tuple(Of T1, T2, T3), ByVal tuple2 As Tuple(Of T1, T2, T3)) As Boolean
			If CObj(tuple1) Is Nothing Then
				Return CObj(tuple2) Is Nothing
			Else
				Return tuple1.Equals(tuple2)
			End If
		End Operator
		Public Shared Operator <>(ByVal tuple1 As Tuple(Of T1, T2, T3), ByVal tuple2 As Tuple(Of T1, T2, T3)) As Boolean
			Return Not (tuple1 = tuple2)
		End Operator
	End Class
	#End Region
	#Region "Quaternary (4-ary) Tuple"
	Public MustInherit Partial Class Tuple
		Public Overloads Shared Function CreateTuple(Of T1, T2, T3, T4)(ByVal item1 As T1, ByVal item2 As T2, ByVal item3 As T3, ByVal item4 As T4) As Tuple(Of T1, T2, T3, T4)
			If (item1 Is Nothing) OrElse ((item2 Is Nothing) OrElse ((item3 Is Nothing) OrElse (item4 Is Nothing))) Then
				Return Nothing
			End If
			Return New Tuple(Of T1, T2, T3, T4)(item1, item2, item3, item4)
		End Function
	End Class
	<System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")> _
	<System.Serializable()> _
	<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
	Public NotInheritable Class Tuple(Of T1, T2, T3, T4)
		Inherits Tuple
		Implements System.IEquatable(Of Tuple(Of T1, T2, T3, T4))
		Private ReadOnly _item1 As T1
		Public ReadOnly Property Item1() As T1
			Get
				Return Me._item1
			End Get
		End Property
		Private ReadOnly _item2 As T2
		Public ReadOnly Property Item2() As T2
			Get
				Return Me._item2
			End Get
		End Property
		Private ReadOnly _item3 As T3
		Public ReadOnly Property Item3() As T3
			Get
				Return Me._item3
			End Get
		End Property
		Private ReadOnly _item4 As T4
		Public ReadOnly Property Item4() As T4
			Get
				Return Me._item4
			End Get
		End Property
		Public Sub New(ByVal item1 As T1, ByVal item2 As T2, ByVal item3 As T3, ByVal item4 As T4)
			If (item1 Is Nothing) OrElse ((item2 Is Nothing) OrElse ((item3 Is Nothing) OrElse (item4 Is Nothing))) Then
				Throw New System.ArgumentNullException()
			End If
			Me._item1 = item1
			Me._item2 = item2
			Me._item3 = item3
			Me._item4 = item4
		End Sub
		Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
			Return Me.Equals(TryCast(obj, Tuple(Of T1, T2, T3, T4)))
		End Function
		Public Overloads Function Equals(ByVal other As Tuple(Of T1, T2, T3, T4)) As Boolean Implements _
			System.IEquatable(Of Tuple(Of T1, T2, T3, T4)).Equals
			If (CObj(other) Is Nothing) OrElse (Not (Me._item1.Equals(other._item1)) OrElse (Not (Me._item2.Equals(other._item2)) OrElse (Not (Me._item3.Equals(other._item3)) OrElse Not (Me._item4.Equals(other._item4))))) Then
				Return False
			End If
			Return True
		End Function
		Public Overrides Function GetHashCode() As Integer
			Return Me._item1.GetHashCode() Xor (Tuple.RotateRight(Me._item2.GetHashCode(), 1) Xor (Tuple.RotateRight(Me._item3.GetHashCode(), 2) Xor Tuple.RotateRight(Me._item4.GetHashCode(), 3)))
		End Function
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Overrides Function ToString(ByVal provider As System.IFormatProvider) As String
			Return String.Format(provider, "({1}, {2}, {3}, {4})", Me._item1, Me._item2, Me._item3, Me._item4)
		End Function
		Public Shared Operator =(ByVal tuple1 As Tuple(Of T1, T2, T3, T4), ByVal tuple2 As Tuple(Of T1, T2, T3, T4)) As Boolean
			If CObj(tuple1) Is Nothing Then
				Return CObj(tuple2) Is Nothing
			Else
				Return tuple1.Equals(tuple2)
			End If
		End Operator
		Public Shared Operator <>(ByVal tuple1 As Tuple(Of T1, T2, T3, T4), ByVal tuple2 As Tuple(Of T1, T2, T3, T4)) As Boolean
			Return Not (tuple1 = tuple2)
		End Operator
	End Class
	#End Region
	#Region "Quinary (5-ary) Tuple"
	Public MustInherit Partial Class Tuple
		Public Overloads Shared Function CreateTuple(Of T1, T2, T3, T4, T5)(ByVal item1 As T1, ByVal item2 As T2, ByVal item3 As T3, ByVal item4 As T4, ByVal item5 As T5) As Tuple(Of T1, T2, T3, T4, T5)
			If (item1 Is Nothing) OrElse ((item2 Is Nothing) OrElse ((item3 Is Nothing) OrElse ((item4 Is Nothing) OrElse (item5 Is Nothing)))) Then
				Return Nothing
			End If
			Return New Tuple(Of T1, T2, T3, T4, T5)(item1, item2, item3, item4, item5)
		End Function
	End Class
	<System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")> _
	<System.Serializable()> _
	<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
	Public NotInheritable Class Tuple(Of T1, T2, T3, T4, T5)
		Inherits Tuple
		Implements System.IEquatable(Of Tuple(Of T1, T2, T3, T4, T5))
		Private ReadOnly _item1 As T1
		Public ReadOnly Property Item1() As T1
			Get
				Return Me._item1
			End Get
		End Property
		Private ReadOnly _item2 As T2
		Public ReadOnly Property Item2() As T2
			Get
				Return Me._item2
			End Get
		End Property
		Private ReadOnly _item3 As T3
		Public ReadOnly Property Item3() As T3
			Get
				Return Me._item3
			End Get
		End Property
		Private ReadOnly _item4 As T4
		Public ReadOnly Property Item4() As T4
			Get
				Return Me._item4
			End Get
		End Property
		Private ReadOnly _item5 As T5
		Public ReadOnly Property Item5() As T5
			Get
				Return Me._item5
			End Get
		End Property
		Public Sub New(ByVal item1 As T1, ByVal item2 As T2, ByVal item3 As T3, ByVal item4 As T4, ByVal item5 As T5)
			If (item1 Is Nothing) OrElse ((item2 Is Nothing) OrElse ((item3 Is Nothing) OrElse ((item4 Is Nothing) OrElse (item5 Is Nothing)))) Then
				Throw New System.ArgumentNullException()
			End If
			Me._item1 = item1
			Me._item2 = item2
			Me._item3 = item3
			Me._item4 = item4
			Me._item5 = item5
		End Sub
		Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
			Return Me.Equals(TryCast(obj, Tuple(Of T1, T2, T3, T4, T5)))
		End Function
		Public Overloads Function Equals(ByVal other As Tuple(Of T1, T2, T3, T4, T5)) As Boolean Implements _
			System.IEquatable(Of Tuple(Of T1, T2, T3, T4, T5)).Equals
			If (CObj(other) Is Nothing) OrElse (Not (Me._item1.Equals(other._item1)) OrElse (Not (Me._item2.Equals(other._item2)) OrElse (Not (Me._item3.Equals(other._item3)) OrElse (Not (Me._item4.Equals(other._item4)) OrElse Not (Me._item5.Equals(other._item5)))))) Then
				Return False
			End If
			Return True
		End Function
		Public Overrides Function GetHashCode() As Integer
			Return Me._item1.GetHashCode() Xor (Tuple.RotateRight(Me._item2.GetHashCode(), 1) Xor (Tuple.RotateRight(Me._item3.GetHashCode(), 2) Xor (Tuple.RotateRight(Me._item4.GetHashCode(), 3) Xor Tuple.RotateRight(Me._item5.GetHashCode(), 4))))
		End Function
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Overrides Function ToString(ByVal provider As System.IFormatProvider) As String
			Return String.Format(provider, "({1}, {2}, {3}, {4}, {5})", Me._item1, Me._item2, Me._item3, Me._item4, Me._item5)
		End Function
		Public Shared Operator =(ByVal tuple1 As Tuple(Of T1, T2, T3, T4, T5), ByVal tuple2 As Tuple(Of T1, T2, T3, T4, T5)) As Boolean
			If CObj(tuple1) Is Nothing Then
				Return CObj(tuple2) Is Nothing
			Else
				Return tuple1.Equals(tuple2)
			End If
		End Operator
		Public Shared Operator <>(ByVal tuple1 As Tuple(Of T1, T2, T3, T4, T5), ByVal tuple2 As Tuple(Of T1, T2, T3, T4, T5)) As Boolean
			Return Not (tuple1 = tuple2)
		End Operator
	End Class
	#End Region
	#Region "Senary (6-ary) Tuple"
	Public MustInherit Partial Class Tuple
		Public Overloads Shared Function CreateTuple(Of T1, T2, T3, T4, T5, T6)(ByVal item1 As T1, ByVal item2 As T2, ByVal item3 As T3, ByVal item4 As T4, ByVal item5 As T5, ByVal item6 As T6) As Tuple(Of T1, T2, T3, T4, T5, T6)
			If (item1 Is Nothing) OrElse ((item2 Is Nothing) OrElse ((item3 Is Nothing) OrElse ((item4 Is Nothing) OrElse ((item5 Is Nothing) OrElse (item6 Is Nothing))))) Then
				Return Nothing
			End If
			Return New Tuple(Of T1, T2, T3, T4, T5, T6)(item1, item2, item3, item4, item5, item6)
		End Function
	End Class
	<System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")> _
	<System.Serializable()> _
	<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
	Public NotInheritable Class Tuple(Of T1, T2, T3, T4, T5, T6)
		Inherits Tuple
		Implements System.IEquatable(Of Tuple(Of T1, T2, T3, T4, T5, T6))
		Private ReadOnly _item1 As T1
		Public ReadOnly Property Item1() As T1
			Get
				Return Me._item1
			End Get
		End Property
		Private ReadOnly _item2 As T2
		Public ReadOnly Property Item2() As T2
			Get
				Return Me._item2
			End Get
		End Property
		Private ReadOnly _item3 As T3
		Public ReadOnly Property Item3() As T3
			Get
				Return Me._item3
			End Get
		End Property
		Private ReadOnly _item4 As T4
		Public ReadOnly Property Item4() As T4
			Get
				Return Me._item4
			End Get
		End Property
		Private ReadOnly _item5 As T5
		Public ReadOnly Property Item5() As T5
			Get
				Return Me._item5
			End Get
		End Property
		Private ReadOnly _item6 As T6
		Public ReadOnly Property Item6() As T6
			Get
				Return Me._item6
			End Get
		End Property
		Public Sub New(ByVal item1 As T1, ByVal item2 As T2, ByVal item3 As T3, ByVal item4 As T4, ByVal item5 As T5, ByVal item6 As T6)
			If (item1 Is Nothing) OrElse ((item2 Is Nothing) OrElse ((item3 Is Nothing) OrElse ((item4 Is Nothing) OrElse ((item5 Is Nothing) OrElse (item6 Is Nothing))))) Then
				Throw New System.ArgumentNullException()
			End If
			Me._item1 = item1
			Me._item2 = item2
			Me._item3 = item3
			Me._item4 = item4
			Me._item5 = item5
			Me._item6 = item6
		End Sub
		Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
			Return Me.Equals(TryCast(obj, Tuple(Of T1, T2, T3, T4, T5, T6)))
		End Function
		Public Overloads Function Equals(ByVal other As Tuple(Of T1, T2, T3, T4, T5, T6)) As Boolean Implements _
			System.IEquatable(Of Tuple(Of T1, T2, T3, T4, T5, T6)).Equals
			If (CObj(other) Is Nothing) OrElse (Not (Me._item1.Equals(other._item1)) OrElse (Not (Me._item2.Equals(other._item2)) OrElse (Not (Me._item3.Equals(other._item3)) OrElse (Not (Me._item4.Equals(other._item4)) OrElse (Not (Me._item5.Equals(other._item5)) OrElse Not (Me._item6.Equals(other._item6))))))) Then
				Return False
			End If
			Return True
		End Function
		Public Overrides Function GetHashCode() As Integer
			Return Me._item1.GetHashCode() Xor (Tuple.RotateRight(Me._item2.GetHashCode(), 1) Xor (Tuple.RotateRight(Me._item3.GetHashCode(), 2) Xor (Tuple.RotateRight(Me._item4.GetHashCode(), 3) Xor (Tuple.RotateRight(Me._item5.GetHashCode(), 4) Xor Tuple.RotateRight(Me._item6.GetHashCode(), 5)))))
		End Function
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Overrides Function ToString(ByVal provider As System.IFormatProvider) As String
			Return String.Format(provider, "({1}, {2}, {3}, {4}, {5}, {6})", Me._item1, Me._item2, Me._item3, Me._item4, Me._item5, Me._item6)
		End Function
		Public Shared Operator =(ByVal tuple1 As Tuple(Of T1, T2, T3, T4, T5, T6), ByVal tuple2 As Tuple(Of T1, T2, T3, T4, T5, T6)) As Boolean
			If CObj(tuple1) Is Nothing Then
				Return CObj(tuple2) Is Nothing
			Else
				Return tuple1.Equals(tuple2)
			End If
		End Operator
		Public Shared Operator <>(ByVal tuple1 As Tuple(Of T1, T2, T3, T4, T5, T6), ByVal tuple2 As Tuple(Of T1, T2, T3, T4, T5, T6)) As Boolean
			Return Not (tuple1 = tuple2)
		End Operator
	End Class
	#End Region
	#Region "Septenary (7-ary) Tuple"
	Public MustInherit Partial Class Tuple
		Public Overloads Shared Function CreateTuple(Of T1, T2, T3, T4, T5, T6, T7)(ByVal item1 As T1, ByVal item2 As T2, ByVal item3 As T3, ByVal item4 As T4, ByVal item5 As T5, ByVal item6 As T6, ByVal item7 As T7) As Tuple(Of T1, T2, T3, T4, T5, T6, T7)
			If (item1 Is Nothing) OrElse ((item2 Is Nothing) OrElse ((item3 Is Nothing) OrElse ((item4 Is Nothing) OrElse ((item5 Is Nothing) OrElse ((item6 Is Nothing) OrElse (item7 Is Nothing)))))) Then
				Return Nothing
			End If
			Return New Tuple(Of T1, T2, T3, T4, T5, T6, T7)(item1, item2, item3, item4, item5, item6, item7)
		End Function
	End Class
	<System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")> _
	<System.Serializable()> _
	<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
	Public NotInheritable Class Tuple(Of T1, T2, T3, T4, T5, T6, T7)
		Inherits Tuple
		Implements System.IEquatable(Of Tuple(Of T1, T2, T3, T4, T5, T6, T7))
		Private ReadOnly _item1 As T1
		Public ReadOnly Property Item1() As T1
			Get
				Return Me._item1
			End Get
		End Property
		Private ReadOnly _item2 As T2
		Public ReadOnly Property Item2() As T2
			Get
				Return Me._item2
			End Get
		End Property
		Private ReadOnly _item3 As T3
		Public ReadOnly Property Item3() As T3
			Get
				Return Me._item3
			End Get
		End Property
		Private ReadOnly _item4 As T4
		Public ReadOnly Property Item4() As T4
			Get
				Return Me._item4
			End Get
		End Property
		Private ReadOnly _item5 As T5
		Public ReadOnly Property Item5() As T5
			Get
				Return Me._item5
			End Get
		End Property
		Private ReadOnly _item6 As T6
		Public ReadOnly Property Item6() As T6
			Get
				Return Me._item6
			End Get
		End Property
		Private ReadOnly _item7 As T7
		Public ReadOnly Property Item7() As T7
			Get
				Return Me._item7
			End Get
		End Property
		Public Sub New(ByVal item1 As T1, ByVal item2 As T2, ByVal item3 As T3, ByVal item4 As T4, ByVal item5 As T5, ByVal item6 As T6, ByVal item7 As T7)
			If (item1 Is Nothing) OrElse ((item2 Is Nothing) OrElse ((item3 Is Nothing) OrElse ((item4 Is Nothing) OrElse ((item5 Is Nothing) OrElse ((item6 Is Nothing) OrElse (item7 Is Nothing)))))) Then
				Throw New System.ArgumentNullException()
			End If
			Me._item1 = item1
			Me._item2 = item2
			Me._item3 = item3
			Me._item4 = item4
			Me._item5 = item5
			Me._item6 = item6
			Me._item7 = item7
		End Sub
		Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
			Return Me.Equals(TryCast(obj, Tuple(Of T1, T2, T3, T4, T5, T6, T7)))
		End Function
		Public Overloads Function Equals(ByVal other As Tuple(Of T1, T2, T3, T4, T5, T6, T7)) As Boolean Implements _
			System.IEquatable(Of Tuple(Of T1, T2, T3, T4, T5, T6, T7)).Equals
			If (CObj(other) Is Nothing) OrElse (Not (Me._item1.Equals(other._item1)) OrElse (Not (Me._item2.Equals(other._item2)) OrElse (Not (Me._item3.Equals(other._item3)) OrElse (Not (Me._item4.Equals(other._item4)) OrElse (Not (Me._item5.Equals(other._item5)) OrElse (Not (Me._item6.Equals(other._item6)) OrElse Not (Me._item7.Equals(other._item7)))))))) Then
				Return False
			End If
			Return True
		End Function
		Public Overrides Function GetHashCode() As Integer
			Return Me._item1.GetHashCode() Xor (Tuple.RotateRight(Me._item2.GetHashCode(), 1) Xor (Tuple.RotateRight(Me._item3.GetHashCode(), 2) Xor (Tuple.RotateRight(Me._item4.GetHashCode(), 3) Xor (Tuple.RotateRight(Me._item5.GetHashCode(), 4) Xor (Tuple.RotateRight(Me._item6.GetHashCode(), 5) Xor Tuple.RotateRight(Me._item7.GetHashCode(), 6))))))
		End Function
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Overrides Function ToString(ByVal provider As System.IFormatProvider) As String
			Return String.Format(provider, "({1}, {2}, {3}, {4}, {5}, {6}, {7})", Me._item1, Me._item2, Me._item3, Me._item4, Me._item5, Me._item6, Me._item7)
		End Function
		Public Shared Operator =(ByVal tuple1 As Tuple(Of T1, T2, T3, T4, T5, T6, T7), ByVal tuple2 As Tuple(Of T1, T2, T3, T4, T5, T6, T7)) As Boolean
			If CObj(tuple1) Is Nothing Then
				Return CObj(tuple2) Is Nothing
			Else
				Return tuple1.Equals(tuple2)
			End If
		End Operator
		Public Shared Operator <>(ByVal tuple1 As Tuple(Of T1, T2, T3, T4, T5, T6, T7), ByVal tuple2 As Tuple(Of T1, T2, T3, T4, T5, T6, T7)) As Boolean
			Return Not (tuple1 = tuple2)
		End Operator
	End Class
	#End Region
	#Region "Octonary (8-ary) Tuple"
	Public MustInherit Partial Class Tuple
		Public Overloads Shared Function CreateTuple(Of T1, T2, T3, T4, T5, T6, T7, T8)(ByVal item1 As T1, ByVal item2 As T2, ByVal item3 As T3, ByVal item4 As T4, ByVal item5 As T5, ByVal item6 As T6, ByVal item7 As T7, ByVal item8 As T8) As Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8)
			If (item1 Is Nothing) OrElse ((item2 Is Nothing) OrElse ((item3 Is Nothing) OrElse ((item4 Is Nothing) OrElse ((item5 Is Nothing) OrElse ((item6 Is Nothing) OrElse ((item7 Is Nothing) OrElse (item8 Is Nothing))))))) Then
				Return Nothing
			End If
			Return New Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8)(item1, item2, item3, item4, item5, item6, item7, item8)
		End Function
	End Class
	<System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")> _
	<System.Serializable()> _
	<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
	Public NotInheritable Class Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8)
		Inherits Tuple
		Implements System.IEquatable(Of Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8))
		Private ReadOnly _item1 As T1
		Public ReadOnly Property Item1() As T1
			Get
				Return Me._item1
			End Get
		End Property
		Private ReadOnly _item2 As T2
		Public ReadOnly Property Item2() As T2
			Get
				Return Me._item2
			End Get
		End Property
		Private ReadOnly _item3 As T3
		Public ReadOnly Property Item3() As T3
			Get
				Return Me._item3
			End Get
		End Property
		Private ReadOnly _item4 As T4
		Public ReadOnly Property Item4() As T4
			Get
				Return Me._item4
			End Get
		End Property
		Private ReadOnly _item5 As T5
		Public ReadOnly Property Item5() As T5
			Get
				Return Me._item5
			End Get
		End Property
		Private ReadOnly _item6 As T6
		Public ReadOnly Property Item6() As T6
			Get
				Return Me._item6
			End Get
		End Property
		Private ReadOnly _item7 As T7
		Public ReadOnly Property Item7() As T7
			Get
				Return Me._item7
			End Get
		End Property
		Private ReadOnly _item8 As T8
		Public ReadOnly Property Item8() As T8
			Get
				Return Me._item8
			End Get
		End Property
		Public Sub New(ByVal item1 As T1, ByVal item2 As T2, ByVal item3 As T3, ByVal item4 As T4, ByVal item5 As T5, ByVal item6 As T6, ByVal item7 As T7, ByVal item8 As T8)
			If (item1 Is Nothing) OrElse ((item2 Is Nothing) OrElse ((item3 Is Nothing) OrElse ((item4 Is Nothing) OrElse ((item5 Is Nothing) OrElse ((item6 Is Nothing) OrElse ((item7 Is Nothing) OrElse (item8 Is Nothing))))))) Then
				Throw New System.ArgumentNullException()
			End If
			Me._item1 = item1
			Me._item2 = item2
			Me._item3 = item3
			Me._item4 = item4
			Me._item5 = item5
			Me._item6 = item6
			Me._item7 = item7
			Me._item8 = item8
		End Sub
		Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
			Return Me.Equals(TryCast(obj, Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8)))
		End Function
		Public Overloads Function Equals(ByVal other As Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8)) As Boolean Implements _
			System.IEquatable(Of Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8)).Equals
			If (CObj(other) Is Nothing) OrElse (Not (Me._item1.Equals(other._item1)) OrElse (Not (Me._item2.Equals(other._item2)) OrElse (Not (Me._item3.Equals(other._item3)) OrElse (Not (Me._item4.Equals(other._item4)) OrElse (Not (Me._item5.Equals(other._item5)) OrElse (Not (Me._item6.Equals(other._item6)) OrElse (Not (Me._item7.Equals(other._item7)) OrElse Not (Me._item8.Equals(other._item8))))))))) Then
				Return False
			End If
			Return True
		End Function
		Public Overrides Function GetHashCode() As Integer
			Return Me._item1.GetHashCode() Xor (Tuple.RotateRight(Me._item2.GetHashCode(), 1) Xor (Tuple.RotateRight(Me._item3.GetHashCode(), 2) Xor (Tuple.RotateRight(Me._item4.GetHashCode(), 3) Xor (Tuple.RotateRight(Me._item5.GetHashCode(), 4) Xor (Tuple.RotateRight(Me._item6.GetHashCode(), 5) Xor (Tuple.RotateRight(Me._item7.GetHashCode(), 6) Xor Tuple.RotateRight(Me._item8.GetHashCode(), 7)))))))
		End Function
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Overrides Function ToString(ByVal provider As System.IFormatProvider) As String
			Return String.Format(provider, "({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8})", Me._item1, Me._item2, Me._item3, Me._item4, Me._item5, Me._item6, Me._item7, Me._item8)
		End Function
		Public Shared Operator =(ByVal tuple1 As Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8), ByVal tuple2 As Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8)) As Boolean
			If CObj(tuple1) Is Nothing Then
				Return CObj(tuple2) Is Nothing
			Else
				Return tuple1.Equals(tuple2)
			End If
		End Operator
		Public Shared Operator <>(ByVal tuple1 As Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8), ByVal tuple2 As Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8)) As Boolean
			Return Not (tuple1 = tuple2)
		End Operator
	End Class
	#End Region
	#Region "Nonary (9-ary) Tuple"
	Public MustInherit Partial Class Tuple
		Public Overloads Shared Function CreateTuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)(ByVal item1 As T1, ByVal item2 As T2, ByVal item3 As T3, ByVal item4 As T4, ByVal item5 As T5, ByVal item6 As T6, ByVal item7 As T7, ByVal item8 As T8, ByVal item9 As T9) As Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)
			If (item1 Is Nothing) OrElse ((item2 Is Nothing) OrElse ((item3 Is Nothing) OrElse ((item4 Is Nothing) OrElse ((item5 Is Nothing) OrElse ((item6 Is Nothing) OrElse ((item7 Is Nothing) OrElse ((item8 Is Nothing) OrElse (item9 Is Nothing)))))))) Then
				Return Nothing
			End If
			Return New Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)(item1, item2, item3, item4, item5, item6, item7, item8, item9)
		End Function
	End Class
	<System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")> _
	<System.Serializable()> _
	<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
	Public NotInheritable Class Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)
		Inherits Tuple
		Implements System.IEquatable(Of Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9))
		Private ReadOnly _item1 As T1
		Public ReadOnly Property Item1() As T1
			Get
				Return Me._item1
			End Get
		End Property
		Private ReadOnly _item2 As T2
		Public ReadOnly Property Item2() As T2
			Get
				Return Me._item2
			End Get
		End Property
		Private ReadOnly _item3 As T3
		Public ReadOnly Property Item3() As T3
			Get
				Return Me._item3
			End Get
		End Property
		Private ReadOnly _item4 As T4
		Public ReadOnly Property Item4() As T4
			Get
				Return Me._item4
			End Get
		End Property
		Private ReadOnly _item5 As T5
		Public ReadOnly Property Item5() As T5
			Get
				Return Me._item5
			End Get
		End Property
		Private ReadOnly _item6 As T6
		Public ReadOnly Property Item6() As T6
			Get
				Return Me._item6
			End Get
		End Property
		Private ReadOnly _item7 As T7
		Public ReadOnly Property Item7() As T7
			Get
				Return Me._item7
			End Get
		End Property
		Private ReadOnly _item8 As T8
		Public ReadOnly Property Item8() As T8
			Get
				Return Me._item8
			End Get
		End Property
		Private ReadOnly _item9 As T9
		Public ReadOnly Property Item9() As T9
			Get
				Return Me._item9
			End Get
		End Property
		Public Sub New(ByVal item1 As T1, ByVal item2 As T2, ByVal item3 As T3, ByVal item4 As T4, ByVal item5 As T5, ByVal item6 As T6, ByVal item7 As T7, ByVal item8 As T8, ByVal item9 As T9)
			If (item1 Is Nothing) OrElse ((item2 Is Nothing) OrElse ((item3 Is Nothing) OrElse ((item4 Is Nothing) OrElse ((item5 Is Nothing) OrElse ((item6 Is Nothing) OrElse ((item7 Is Nothing) OrElse ((item8 Is Nothing) OrElse (item9 Is Nothing)))))))) Then
				Throw New System.ArgumentNullException()
			End If
			Me._item1 = item1
			Me._item2 = item2
			Me._item3 = item3
			Me._item4 = item4
			Me._item5 = item5
			Me._item6 = item6
			Me._item7 = item7
			Me._item8 = item8
			Me._item9 = item9
		End Sub
		Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
			Return Me.Equals(TryCast(obj, Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)))
		End Function
		Public Overloads Function Equals(ByVal other As Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)) As Boolean Implements _
			System.IEquatable(Of Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)).Equals
			If (CObj(other) Is Nothing) OrElse (Not (Me._item1.Equals(other._item1)) OrElse (Not (Me._item2.Equals(other._item2)) OrElse (Not (Me._item3.Equals(other._item3)) OrElse (Not (Me._item4.Equals(other._item4)) OrElse (Not (Me._item5.Equals(other._item5)) OrElse (Not (Me._item6.Equals(other._item6)) OrElse (Not (Me._item7.Equals(other._item7)) OrElse (Not (Me._item8.Equals(other._item8)) OrElse Not (Me._item9.Equals(other._item9)))))))))) Then
				Return False
			End If
			Return True
		End Function
		Public Overrides Function GetHashCode() As Integer
			Return Me._item1.GetHashCode() Xor (Tuple.RotateRight(Me._item2.GetHashCode(), 1) Xor (Tuple.RotateRight(Me._item3.GetHashCode(), 2) Xor (Tuple.RotateRight(Me._item4.GetHashCode(), 3) Xor (Tuple.RotateRight(Me._item5.GetHashCode(), 4) Xor (Tuple.RotateRight(Me._item6.GetHashCode(), 5) Xor (Tuple.RotateRight(Me._item7.GetHashCode(), 6) Xor (Tuple.RotateRight(Me._item8.GetHashCode(), 7) Xor Tuple.RotateRight(Me._item9.GetHashCode(), 8))))))))
		End Function
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Overrides Function ToString(ByVal provider As System.IFormatProvider) As String
			Return String.Format(provider, "({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9})", Me._item1, Me._item2, Me._item3, Me._item4, Me._item5, Me._item6, Me._item7, Me._item8, Me._item9)
		End Function
		Public Shared Operator =(ByVal tuple1 As Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9), ByVal tuple2 As Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)) As Boolean
			If CObj(tuple1) Is Nothing Then
				Return CObj(tuple2) Is Nothing
			Else
				Return tuple1.Equals(tuple2)
			End If
		End Operator
		Public Shared Operator <>(ByVal tuple1 As Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9), ByVal tuple2 As Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)) As Boolean
			Return Not (tuple1 = tuple2)
		End Operator
	End Class
	#End Region
	#Region "Denary (10-ary) Tuple"
	Public MustInherit Partial Class Tuple
		Public Overloads Shared Function CreateTuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)(ByVal item1 As T1, ByVal item2 As T2, ByVal item3 As T3, ByVal item4 As T4, ByVal item5 As T5, ByVal item6 As T6, ByVal item7 As T7, ByVal item8 As T8, ByVal item9 As T9, ByVal item10 As T10) As Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)
			If (item1 Is Nothing) OrElse ((item2 Is Nothing) OrElse ((item3 Is Nothing) OrElse ((item4 Is Nothing) OrElse ((item5 Is Nothing) OrElse ((item6 Is Nothing) OrElse ((item7 Is Nothing) OrElse ((item8 Is Nothing) OrElse ((item9 Is Nothing) OrElse (item10 Is Nothing))))))))) Then
				Return Nothing
			End If
			Return New Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)(item1, item2, item3, item4, item5, item6, item7, item8, item9, item10)
		End Function
	End Class
	<System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")> _
	<System.Serializable()> _
	<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
	Public NotInheritable Class Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)
		Inherits Tuple
		Implements System.IEquatable(Of Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10))
		Private ReadOnly _item1 As T1
		Public ReadOnly Property Item1() As T1
			Get
				Return Me._item1
			End Get
		End Property
		Private ReadOnly _item2 As T2
		Public ReadOnly Property Item2() As T2
			Get
				Return Me._item2
			End Get
		End Property
		Private ReadOnly _item3 As T3
		Public ReadOnly Property Item3() As T3
			Get
				Return Me._item3
			End Get
		End Property
		Private ReadOnly _item4 As T4
		Public ReadOnly Property Item4() As T4
			Get
				Return Me._item4
			End Get
		End Property
		Private ReadOnly _item5 As T5
		Public ReadOnly Property Item5() As T5
			Get
				Return Me._item5
			End Get
		End Property
		Private ReadOnly _item6 As T6
		Public ReadOnly Property Item6() As T6
			Get
				Return Me._item6
			End Get
		End Property
		Private ReadOnly _item7 As T7
		Public ReadOnly Property Item7() As T7
			Get
				Return Me._item7
			End Get
		End Property
		Private ReadOnly _item8 As T8
		Public ReadOnly Property Item8() As T8
			Get
				Return Me._item8
			End Get
		End Property
		Private ReadOnly _item9 As T9
		Public ReadOnly Property Item9() As T9
			Get
				Return Me._item9
			End Get
		End Property
		Private ReadOnly _item10 As T10
		Public ReadOnly Property Item10() As T10
			Get
				Return Me._item10
			End Get
		End Property
		Public Sub New(ByVal item1 As T1, ByVal item2 As T2, ByVal item3 As T3, ByVal item4 As T4, ByVal item5 As T5, ByVal item6 As T6, ByVal item7 As T7, ByVal item8 As T8, ByVal item9 As T9, ByVal item10 As T10)
			If (item1 Is Nothing) OrElse ((item2 Is Nothing) OrElse ((item3 Is Nothing) OrElse ((item4 Is Nothing) OrElse ((item5 Is Nothing) OrElse ((item6 Is Nothing) OrElse ((item7 Is Nothing) OrElse ((item8 Is Nothing) OrElse ((item9 Is Nothing) OrElse (item10 Is Nothing))))))))) Then
				Throw New System.ArgumentNullException()
			End If
			Me._item1 = item1
			Me._item2 = item2
			Me._item3 = item3
			Me._item4 = item4
			Me._item5 = item5
			Me._item6 = item6
			Me._item7 = item7
			Me._item8 = item8
			Me._item9 = item9
			Me._item10 = item10
		End Sub
		Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
			Return Me.Equals(TryCast(obj, Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)))
		End Function
		Public Overloads Function Equals(ByVal other As Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)) As Boolean Implements _
			System.IEquatable(Of Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)).Equals
			If (CObj(other) Is Nothing) OrElse (Not (Me._item1.Equals(other._item1)) OrElse (Not (Me._item2.Equals(other._item2)) OrElse (Not (Me._item3.Equals(other._item3)) OrElse (Not (Me._item4.Equals(other._item4)) OrElse (Not (Me._item5.Equals(other._item5)) OrElse (Not (Me._item6.Equals(other._item6)) OrElse (Not (Me._item7.Equals(other._item7)) OrElse (Not (Me._item8.Equals(other._item8)) OrElse (Not (Me._item9.Equals(other._item9)) OrElse Not (Me._item10.Equals(other._item10))))))))))) Then
				Return False
			End If
			Return True
		End Function
		Public Overrides Function GetHashCode() As Integer
			Return Me._item1.GetHashCode() Xor (Tuple.RotateRight(Me._item2.GetHashCode(), 1) Xor (Tuple.RotateRight(Me._item3.GetHashCode(), 2) Xor (Tuple.RotateRight(Me._item4.GetHashCode(), 3) Xor (Tuple.RotateRight(Me._item5.GetHashCode(), 4) Xor (Tuple.RotateRight(Me._item6.GetHashCode(), 5) Xor (Tuple.RotateRight(Me._item7.GetHashCode(), 6) Xor (Tuple.RotateRight(Me._item8.GetHashCode(), 7) Xor (Tuple.RotateRight(Me._item9.GetHashCode(), 8) Xor Tuple.RotateRight(Me._item10.GetHashCode(), 9)))))))))
		End Function
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Overrides Function ToString(ByVal provider As System.IFormatProvider) As String
			Return String.Format(provider, "({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10})", Me._item1, Me._item2, Me._item3, Me._item4, Me._item5, Me._item6, Me._item7, Me._item8, Me._item9, Me._item10)
		End Function
		Public Shared Operator =(ByVal tuple1 As Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10), ByVal tuple2 As Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)) As Boolean
			If CObj(tuple1) Is Nothing Then
				Return CObj(tuple2) Is Nothing
			Else
				Return tuple1.Equals(tuple2)
			End If
		End Operator
		Public Shared Operator <>(ByVal tuple1 As Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10), ByVal tuple2 As Tuple(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)) As Boolean
			Return Not (tuple1 = tuple2)
		End Operator
	End Class
	#End Region
	#Region "Property Change Event Support"
	<SuppressMessageAttribute("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")> _
	Public Interface IPropertyChangeEventArgs(Of TClass, TProperty)
		ReadOnly Property Instance() As TClass
		ReadOnly Property OldValue() As TProperty
		ReadOnly Property NewValue() As TProperty
	End Interface
	<Serializable()> _
	<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
	Public NotInheritable Class PropertyChangingEventArgs(Of TClass, TProperty)
		Inherits CancelEventArgs
		Implements IPropertyChangeEventArgs(Of TClass, TProperty)
		Private ReadOnly _instance As TClass
		Private ReadOnly _oldValue As TProperty
		Private ReadOnly _newValue As TProperty
		Public Sub New(ByVal instance As TClass, ByVal oldValue As TProperty, ByVal newValue As TProperty)
			If instance Is Nothing Then
				Throw New ArgumentNullException("instance")
			End If
			Me._instance = instance
			Me._oldValue = oldValue
			Me._newValue = newValue
		End Sub
		Public ReadOnly Property Instance() As TClass Implements _
			IPropertyChangeEventArgs(Of TClass, TProperty).Instance
			Get
				Return Me._instance
			End Get
		End Property
		Public ReadOnly Property OldValue() As TProperty Implements _
			IPropertyChangeEventArgs(Of TClass, TProperty).OldValue
			Get
				Return Me._oldValue
			End Get
		End Property
		Public ReadOnly Property NewValue() As TProperty Implements _
			IPropertyChangeEventArgs(Of TClass, TProperty).NewValue
			Get
				Return Me._newValue
			End Get
		End Property
	End Class
	<Serializable()> _
	<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
	Public NotInheritable Class PropertyChangedEventArgs(Of TClass, TProperty)
		Inherits EventArgs
		Implements IPropertyChangeEventArgs(Of TClass, TProperty)
		Private ReadOnly _instance As TClass
		Private ReadOnly _oldValue As TProperty
		Private ReadOnly _newValue As TProperty
		Public Sub New(ByVal instance As TClass, ByVal oldValue As TProperty, ByVal newValue As TProperty)
			If instance Is Nothing Then
				Throw New ArgumentNullException("instance")
			End If
			Me._instance = instance
			Me._oldValue = oldValue
			Me._newValue = newValue
		End Sub
		Public ReadOnly Property Instance() As TClass Implements _
			IPropertyChangeEventArgs(Of TClass, TProperty).Instance
			Get
				Return Me._instance
			End Get
		End Property
		Public ReadOnly Property OldValue() As TProperty Implements _
			IPropertyChangeEventArgs(Of TClass, TProperty).OldValue
			Get
				Return Me._oldValue
			End Get
		End Property
		Public ReadOnly Property NewValue() As TProperty Implements _
			IPropertyChangeEventArgs(Of TClass, TProperty).NewValue
			Get
				Return Me._newValue
			End Get
		End Property
	End Class
	#End Region
End Namespace
#End Region
Namespace PersonCountryDemo
	#Region "Person"
	<DataObjectAttribute()> _
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
	Public MustInherit Partial Class Person
		Implements INotifyPropertyChanged
		Implements IHasPersonCountryDemoContext
		Protected Sub New()
		End Sub
		Private _events As System.Delegate()
		Private ReadOnly Property Events() As System.Delegate()
			Get
				If CObj(Me._events) Is Nothing Then
					Me._events = New System.Delegate(4) {}
				End If
				Return Me._events
			End Get
		End Property
		Private _propertyChangedEventHandler As PropertyChangedEventHandler
		<SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")> _
		Private Custom Event PropertyChanged As PropertyChangedEventHandler Implements _
			INotifyPropertyChanged.PropertyChanged
			AddHandler(ByVal Value As PropertyChangedEventHandler)
				Me._propertyChangedEventHandler = TryCast(System.Delegate.Combine(Me._propertyChangedEventHandler, Value), PropertyChangedEventHandler)
			End AddHandler
			RemoveHandler(ByVal Value As PropertyChangedEventHandler)
				Me._propertyChangedEventHandler = TryCast(System.Delegate.Remove(Me._propertyChangedEventHandler, Value), PropertyChangedEventHandler)
			End RemoveHandler
		End Event
		Private Sub RaisePropertyChangedEvent(ByVal propertyName As String)
			Dim eventHandler As PropertyChangedEventHandler = Me._propertyChangedEventHandler
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(propertyName), New AsyncCallback(eventHandler.EndInvoke), Nothing)
			End If
		End Sub
		Public MustOverride ReadOnly Property Context() As PersonCountryDemoContext Implements _
			IHasPersonCountryDemoContext.Context
		Public Custom Event LastNameChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Combine(Me.Events(0), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Remove(Me.Events(0), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseLastNameChangingEvent(ByVal newValue As String) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Person, String)) = TryCast(Me.Events(0), EventHandler(Of PropertyChangingEventArgs(Of Person, String)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Person, String) = New PropertyChangingEventArgs(Of Person, String)(Me, Me.LastName, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event LastNameChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Combine(Me.Events(0), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Remove(Me.Events(0), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseLastNameChangedEvent(ByVal oldValue As String)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Person, String)) = TryCast(Me.Events(0), EventHandler(Of PropertyChangedEventArgs(Of Person, String)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Person, String)(Me, oldValue, Me.LastName), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("LastName")
			End If
		End Sub
		Public Custom Event FirstNameChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseFirstNameChangingEvent(ByVal newValue As String) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Person, String)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangingEventArgs(Of Person, String)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Person, String) = New PropertyChangingEventArgs(Of Person, String)(Me, Me.FirstName, newValue)
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
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseFirstNameChangedEvent(ByVal oldValue As String)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Person, String)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangedEventArgs(Of Person, String)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Person, String)(Me, oldValue, Me.FirstName), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("FirstName")
			End If
		End Sub
		Public Custom Event TitleChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseTitleChangingEvent(ByVal newValue As String) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Person, String)) = TryCast(Me.Events(2), EventHandler(Of PropertyChangingEventArgs(Of Person, String)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Person, String) = New PropertyChangingEventArgs(Of Person, String)(Me, Me.Title, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event TitleChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseTitleChangedEvent(ByVal oldValue As String)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Person, String)) = TryCast(Me.Events(2), EventHandler(Of PropertyChangedEventArgs(Of Person, String)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Person, String)(Me, oldValue, Me.Title), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Title")
			End If
		End Sub
		Public Custom Event CountryChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Combine(Me.Events(3), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Remove(Me.Events(3), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseCountryChangingEvent(ByVal newValue As Country) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Person, Country)) = TryCast(Me.Events(3), EventHandler(Of PropertyChangingEventArgs(Of Person, Country)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Person, Country) = New PropertyChangingEventArgs(Of Person, Country)(Me, Me.Country, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event CountryChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Combine(Me.Events(3), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Remove(Me.Events(3), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseCountryChangedEvent(ByVal oldValue As Country)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Person, Country)) = TryCast(Me.Events(3), EventHandler(Of PropertyChangedEventArgs(Of Person, Country)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Person, Country)(Me, oldValue, Me.Country), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Country")
			End If
		End Sub
		<DataObjectFieldAttribute(False, False, False)> _
		Public MustOverride Property LastName() As String
		<DataObjectFieldAttribute(False, False, False)> _
		Public MustOverride Property FirstName() As String
		<DataObjectFieldAttribute(False, False, True)> _
		Public MustOverride Property Title() As String
		<DataObjectFieldAttribute(False, False, True)> _
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
	<DataObjectAttribute()> _
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
	Public MustInherit Partial Class Country
		Implements INotifyPropertyChanged
		Implements IHasPersonCountryDemoContext
		Protected Sub New()
		End Sub
		Private _events As System.Delegate()
		Private ReadOnly Property Events() As System.Delegate()
			Get
				If CObj(Me._events) Is Nothing Then
					Me._events = New System.Delegate(2) {}
				End If
				Return Me._events
			End Get
		End Property
		Private _propertyChangedEventHandler As PropertyChangedEventHandler
		<SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")> _
		Private Custom Event PropertyChanged As PropertyChangedEventHandler Implements _
			INotifyPropertyChanged.PropertyChanged
			AddHandler(ByVal Value As PropertyChangedEventHandler)
				Me._propertyChangedEventHandler = TryCast(System.Delegate.Combine(Me._propertyChangedEventHandler, Value), PropertyChangedEventHandler)
			End AddHandler
			RemoveHandler(ByVal Value As PropertyChangedEventHandler)
				Me._propertyChangedEventHandler = TryCast(System.Delegate.Remove(Me._propertyChangedEventHandler, Value), PropertyChangedEventHandler)
			End RemoveHandler
		End Event
		Private Sub RaisePropertyChangedEvent(ByVal propertyName As String)
			Dim eventHandler As PropertyChangedEventHandler = Me._propertyChangedEventHandler
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(propertyName), New AsyncCallback(eventHandler.EndInvoke), Nothing)
			End If
		End Sub
		Public MustOverride ReadOnly Property Context() As PersonCountryDemoContext Implements _
			IHasPersonCountryDemoContext.Context
		Public Custom Event Country_nameChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Combine(Me.Events(0), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Remove(Me.Events(0), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseCountry_nameChangingEvent(ByVal newValue As String) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Country, String)) = TryCast(Me.Events(0), EventHandler(Of PropertyChangingEventArgs(Of Country, String)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Country, String) = New PropertyChangingEventArgs(Of Country, String)(Me, Me.Country_name, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event Country_nameChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Combine(Me.Events(0), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Remove(Me.Events(0), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseCountry_nameChangedEvent(ByVal oldValue As String)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Country, String)) = TryCast(Me.Events(0), EventHandler(Of PropertyChangedEventArgs(Of Country, String)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Country, String)(Me, oldValue, Me.Country_name), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Country_name")
			End If
		End Sub
		Public Custom Event Region_Region_codeChanging As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseRegion_Region_codeChangingEvent(ByVal newValue As String) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Country, String)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangingEventArgs(Of Country, String)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Country, String) = New PropertyChangingEventArgs(Of Country, String)(Me, Me.Region_Region_code, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event Region_Region_codeChanged As EventHandler
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseRegion_Region_codeChangedEvent(ByVal oldValue As String)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Country, String)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangedEventArgs(Of Country, String)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Country, String)(Me, oldValue, Me.Region_Region_code), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Region_Region_code")
			End If
		End Sub
		<DataObjectFieldAttribute(False, False, False)> _
		Public MustOverride Property Country_name() As String
		<DataObjectFieldAttribute(False, False, True)> _
		Public MustOverride Property Region_Region_code() As String
		<DataObjectFieldAttribute(False, False, True)> _
		Public MustOverride ReadOnly Property Person() As ICollection(Of Person)
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Overridable Function ToString(ByVal provider As IFormatProvider) As String
			Return String.Format(provider, "Country{0}{{{0}{1}Country_name = ""{2}"",{0}{1}Region_Region_code = ""{3}""{0}}}", Environment.NewLine, "", Me.Country_name, Me.Region_Region_code)
		End Function
	End Class
	#End Region
	#Region "IHasPersonCountryDemoContext"
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	Public Interface IHasPersonCountryDemoContext
		ReadOnly Property Context() As PersonCountryDemoContext
	End Interface
	#End Region
	#Region "IPersonCountryDemoContext"
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	Public Interface IPersonCountryDemoContext
		Function GetCountryByCountry_name(ByVal Country_name As String) As Country
		Function TryGetCountryByCountry_name(ByVal Country_name As String, <System.Runtime.InteropServices.Out> ByRef Country As Country) As Boolean
		Function CreatePerson(ByVal LastName As String, ByVal FirstName As String) As Person
		ReadOnly Property PersonCollection() As ReadOnlyCollection(Of Person)
		Function CreateCountry(ByVal Country_name As String) As Country
		ReadOnly Property CountryCollection() As ReadOnlyCollection(Of Country)
	End Interface
	#End Region
	#Region "PersonCountryDemoContext"
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
	Public NotInheritable Class PersonCountryDemoContext
		Implements IPersonCountryDemoContext
		Public Sub New()
			Dim constraintEnforcementCollectionCallbacksByTypeDictionary As Dictionary(Of Type, Object) = New Dictionary(Of Type, Object)(1)
			Dim constraintEnforcementCollectionCallbacksByTypeAndNameDictionary As Dictionary(Of ConstraintEnforcementCollectionTypeAndPropertyNameKey, Object) = New Dictionary(Of ConstraintEnforcementCollectionTypeAndPropertyNameKey, Object)(0)
			Me._ContraintEnforcementCollectionCallbacksByTypeDictionary = constraintEnforcementCollectionCallbacksByTypeDictionary
			Me._ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary = constraintEnforcementCollectionCallbacksByTypeAndNameDictionary
			constraintEnforcementCollectionCallbacksByTypeDictionary.Add(GetType(ConstraintEnforcementCollection(Of Country, Person)), New ConstraintEnforcementCollectionCallbacks(Of Country, Person)(New PotentialCollectionModificationCallback(Of Country, Person)(Me.OnCountryPersonAdding), New CommittedCollectionModificationCallback(Of Country, Person)(Me.OnCountryPersonAdded), Nothing, New CommittedCollectionModificationCallback(Of Country, Person)(Me.OnCountryPersonRemoved)))
			Dim PersonList As List(Of Person) = New List(Of Person)()
			Me._PersonList = PersonList
			Me._PersonReadOnlyCollection = New ReadOnlyCollection(Of Person)(PersonList)
			Dim CountryList As List(Of Country) = New List(Of Country)()
			Me._CountryList = CountryList
			Me._CountryReadOnlyCollection = New ReadOnlyCollection(Of Country)(CountryList)
		End Sub
		#Region "Exception Helpers"
		<SuppressMessageAttribute("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")> _
		Private Shared Function GetDifferentContextsException() As ArgumentException
			Return New ArgumentException("All objects in a relationship must be part of the same Context.", "value")
		End Function
		Private Shared Function GetConstraintEnforcementFailedException(ByVal paramName As String) As ArgumentException
			Return New ArgumentException("Argument failed constraint enforcement.", paramName)
		End Function
		#End Region
		#Region "Lookup and External Constraint Enforcement"
		Private ReadOnly _CountryCountry_nameDictionary As Dictionary(Of String, Country) = New Dictionary(Of String, Country)()
		Public Function GetCountryByCountry_name(ByVal Country_name As String) As Country Implements _
			IPersonCountryDemoContext.GetCountryByCountry_name
			Return Me._CountryCountry_nameDictionary(Country_name)
		End Function
		Public Function TryGetCountryByCountry_name(ByVal Country_name As String, <System.Runtime.InteropServices.Out> ByRef Country As Country) As Boolean Implements _
			IPersonCountryDemoContext.TryGetCountryByCountry_name
			Return Me._CountryCountry_nameDictionary.TryGetValue(Country_name, Country)
		End Function
		#End Region
		#Region "ConstraintEnforcementCollection"
		Private Delegate Function PotentialCollectionModificationCallback(Of TClass As {class, IHasPersonCountryDemoContext}, TProperty)(ByVal instance As TClass, ByVal value As TProperty) As Boolean
		Private Delegate Sub CommittedCollectionModificationCallback(Of TClass As {class, IHasPersonCountryDemoContext}, TProperty)(ByVal instance As TClass, ByVal value As TProperty)
		<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
		Private NotInheritable Class ConstraintEnforcementCollectionCallbacks(Of TClass As {class, IHasPersonCountryDemoContext}, TProperty)
			Public Sub New(ByVal adding As PotentialCollectionModificationCallback(Of TClass, TProperty), ByVal added As CommittedCollectionModificationCallback(Of TClass, TProperty), ByVal removing As PotentialCollectionModificationCallback(Of TClass, TProperty), ByVal removed As CommittedCollectionModificationCallback(Of TClass, TProperty))
				Me.Adding = adding
				Me.Added = added
				Me.Removing = removing
				Me.Removed = removed
			End Sub
			Public ReadOnly Adding As PotentialCollectionModificationCallback(Of TClass, TProperty)
			Public ReadOnly Added As CommittedCollectionModificationCallback(Of TClass, TProperty)
			Public ReadOnly Removing As PotentialCollectionModificationCallback(Of TClass, TProperty)
			Public ReadOnly Removed As CommittedCollectionModificationCallback(Of TClass, TProperty)
		End Class
		<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
		Private Structure ConstraintEnforcementCollectionTypeAndPropertyNameKey
			Implements IEquatable(Of ConstraintEnforcementCollectionTypeAndPropertyNameKey)
			Public Sub New(ByVal type As Type, ByVal name As String)
				Me.Type = type
				Me.Name = name
			End Sub
			Public ReadOnly Type As Type
			Public ReadOnly Name As String
			Public Overrides Function GetHashCode() As Integer
				Return Me.Type.GetHashCode() Xor Me.Name.GetHashCode()
			End Function
			Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
				Return (TypeOf obj Is ConstraintEnforcementCollectionTypeAndPropertyNameKey) AndAlso Me.Equals(CType(obj, ConstraintEnforcementCollectionTypeAndPropertyNameKey))
			End Function
			Public Overloads Function Equals(ByVal other As ConstraintEnforcementCollectionTypeAndPropertyNameKey) As Boolean Implements _
				IEquatable(Of ConstraintEnforcementCollectionTypeAndPropertyNameKey).Equals
				Return Me.Type.Equals(other.Type) AndAlso Me.Name.Equals(other.Name)
			End Function
			Public Shared Operator =(ByVal left As ConstraintEnforcementCollectionTypeAndPropertyNameKey, ByVal right As ConstraintEnforcementCollectionTypeAndPropertyNameKey) As Boolean
				Return left.Equals(right)
			End Operator
			Public Shared Operator <>(ByVal left As ConstraintEnforcementCollectionTypeAndPropertyNameKey, ByVal right As ConstraintEnforcementCollectionTypeAndPropertyNameKey) As Boolean
				Return Not (left.Equals(right))
			End Operator
		End Structure
		Private ReadOnly _ContraintEnforcementCollectionCallbacksByTypeDictionary As Dictionary(Of Type, Object)
		Private ReadOnly _ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary As Dictionary(Of ConstraintEnforcementCollectionTypeAndPropertyNameKey, Object)
		Private Overloads Function OnAdding(Of TClass As {class, IHasPersonCountryDemoContext}, TProperty)(ByVal instance As TClass, ByVal value As TProperty) As Boolean
			Dim adding As PotentialCollectionModificationCallback(Of TClass, TProperty) = (CType(Me._ContraintEnforcementCollectionCallbacksByTypeDictionary(GetType(ConstraintEnforcementCollection(Of TClass, TProperty))), ConstraintEnforcementCollectionCallbacks(Of TClass, TProperty))).Adding
			If adding IsNot Nothing Then
				Return adding(instance, value)
			End If
			Return True
		End Function
		Private Overloads Function OnAdding(Of TClass As {class, IHasPersonCountryDemoContext}, TProperty)(ByVal propertyName As String, ByVal instance As TClass, ByVal value As TProperty) As Boolean
			Dim adding As PotentialCollectionModificationCallback(Of TClass, TProperty) = (CType(Me._ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary(New ConstraintEnforcementCollectionTypeAndPropertyNameKey(GetType(ConstraintEnforcementCollectionWithPropertyName(Of TClass, TProperty)), propertyName)), ConstraintEnforcementCollectionCallbacks(Of TClass, TProperty))).Adding
			If adding IsNot Nothing Then
				Return adding(instance, value)
			End If
			Return True
		End Function
		Private Overloads Sub OnAdded(Of TClass As {class, IHasPersonCountryDemoContext}, TProperty)(ByVal instance As TClass, ByVal value As TProperty)
			Dim added As CommittedCollectionModificationCallback(Of TClass, TProperty) = (CType(Me._ContraintEnforcementCollectionCallbacksByTypeDictionary(GetType(ConstraintEnforcementCollection(Of TClass, TProperty))), ConstraintEnforcementCollectionCallbacks(Of TClass, TProperty))).Added
			If added IsNot Nothing Then
				added(instance, value)
			End If
		End Sub
		Private Overloads Sub OnAdded(Of TClass As {class, IHasPersonCountryDemoContext}, TProperty)(ByVal propertyName As String, ByVal instance As TClass, ByVal value As TProperty)
			Dim added As CommittedCollectionModificationCallback(Of TClass, TProperty) = (CType(Me._ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary(New ConstraintEnforcementCollectionTypeAndPropertyNameKey(GetType(ConstraintEnforcementCollectionWithPropertyName(Of TClass, TProperty)), propertyName)), ConstraintEnforcementCollectionCallbacks(Of TClass, TProperty))).Added
			If added IsNot Nothing Then
				added(instance, value)
			End If
		End Sub
		Private Overloads Function OnRemoving(Of TClass As {class, IHasPersonCountryDemoContext}, TProperty)(ByVal instance As TClass, ByVal value As TProperty) As Boolean
			Dim removing As PotentialCollectionModificationCallback(Of TClass, TProperty) = (CType(Me._ContraintEnforcementCollectionCallbacksByTypeDictionary(GetType(ConstraintEnforcementCollection(Of TClass, TProperty))), ConstraintEnforcementCollectionCallbacks(Of TClass, TProperty))).Removing
			If removing IsNot Nothing Then
				Return removing(instance, value)
			End If
			Return True
		End Function
		Private Overloads Function OnRemoving(Of TClass As {class, IHasPersonCountryDemoContext}, TProperty)(ByVal propertyName As String, ByVal instance As TClass, ByVal value As TProperty) As Boolean
			Dim removing As PotentialCollectionModificationCallback(Of TClass, TProperty) = (CType(Me._ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary(New ConstraintEnforcementCollectionTypeAndPropertyNameKey(GetType(ConstraintEnforcementCollectionWithPropertyName(Of TClass, TProperty)), propertyName)), ConstraintEnforcementCollectionCallbacks(Of TClass, TProperty))).Removing
			If removing IsNot Nothing Then
				Return removing(instance, value)
			End If
			Return True
		End Function
		Private Overloads Sub OnRemoved(Of TClass As {class, IHasPersonCountryDemoContext}, TProperty)(ByVal instance As TClass, ByVal value As TProperty)
			Dim removed As CommittedCollectionModificationCallback(Of TClass, TProperty) = (CType(Me._ContraintEnforcementCollectionCallbacksByTypeDictionary(GetType(ConstraintEnforcementCollection(Of TClass, TProperty))), ConstraintEnforcementCollectionCallbacks(Of TClass, TProperty))).Removed
			If removed IsNot Nothing Then
				removed(instance, value)
			End If
		End Sub
		Private Overloads Sub OnRemoved(Of TClass As {class, IHasPersonCountryDemoContext}, TProperty)(ByVal propertyName As String, ByVal instance As TClass, ByVal value As TProperty)
			Dim removed As CommittedCollectionModificationCallback(Of TClass, TProperty) = (CType(Me._ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary(New ConstraintEnforcementCollectionTypeAndPropertyNameKey(GetType(ConstraintEnforcementCollectionWithPropertyName(Of TClass, TProperty)), propertyName)), ConstraintEnforcementCollectionCallbacks(Of TClass, TProperty))).Removed
			If removed IsNot Nothing Then
				removed(instance, value)
			End If
		End Sub
		<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
		Private NotInheritable Class ConstraintEnforcementCollection(Of TClass As {class, IHasPersonCountryDemoContext}, TProperty)
			Implements ICollection(Of TProperty)
			Private ReadOnly _instance As TClass
			Private ReadOnly _list As List(Of TProperty) = New List(Of TProperty)()
			Public Sub New(ByVal instance As TClass)
				Me._instance = instance
			End Sub
			Private Function GetEnumerator() As System.Collections.IEnumerator Implements _
				System.Collections.IEnumerable.GetEnumerator
				Return Me.GetEnumerator()
			End Function
			Public Function GetEnumerator() As IEnumerator(Of TProperty) Implements _
				IEnumerable(Of TProperty).GetEnumerator
				Return Me._list.GetEnumerator()
			End Function
			Public Sub Add(ByVal item As TProperty) Implements _
				ICollection(Of TProperty).Add
				If item Is Nothing Then
					Throw New ArgumentNullException("item")
				End If
				If Me._instance.Context.OnAdding(Me._instance, item) Then
					Me._list.Add(item)
					Me._instance.Context.OnAdded(Me._instance, item)
				End If
			End Sub
			Public Function Remove(ByVal item As TProperty) As Boolean Implements _
				ICollection(Of TProperty).Remove
				If item Is Nothing Then
					Throw New ArgumentNullException("item")
				End If
				If Me._instance.Context.OnRemoving(Me._instance, item) Then
					If Me._list.Remove(item) Then
						Me._instance.Context.OnRemoved(Me._instance, item)
						Return True
					End If
				End If
				Return False
			End Function
			Public Sub Clear() Implements _
				ICollection(Of TProperty).Clear
				Dim i As Integer = 0
				While i < Me._list.Count
					Me.Remove(Me._list(i))
					i = i + 1
				End While
			End Sub
			Public Function Contains(ByVal item As TProperty) As Boolean Implements _
				ICollection(Of TProperty).Contains
				Return Me._list.Contains(item)
			End Function
			Public Sub CopyTo(ByVal array As TProperty(), ByVal arrayIndex As Integer) Implements _
				ICollection(Of TProperty).CopyTo
				Me._list.CopyTo(array, arrayIndex)
			End Sub
			Public ReadOnly Property Count() As Integer Implements _
				ICollection(Of TProperty).Count
				Get
					Return Me._list.Count
				End Get
			End Property
			Public ReadOnly Property IsReadOnly() As Boolean Implements _
				ICollection(Of TProperty).IsReadOnly
				Get
					Return False
				End Get
			End Property
		End Class
		<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
		Private NotInheritable Class ConstraintEnforcementCollectionWithPropertyName(Of TClass As {class, IHasPersonCountryDemoContext}, TProperty)
			Implements ICollection(Of TProperty)
			Private ReadOnly _instance As TClass
			Private ReadOnly _PropertyName As String
			Private ReadOnly _list As List(Of TProperty) = New List(Of TProperty)()
			Public Sub New(ByVal instance As TClass, ByVal propertyName As String)
				Me._instance = instance
				Me._PropertyName = propertyName
			End Sub
			Private Function GetEnumerator() As System.Collections.IEnumerator Implements _
				System.Collections.IEnumerable.GetEnumerator
				Return Me.GetEnumerator()
			End Function
			Public Function GetEnumerator() As IEnumerator(Of TProperty) Implements _
				IEnumerable(Of TProperty).GetEnumerator
				Return Me._list.GetEnumerator()
			End Function
			Public Sub Add(ByVal item As TProperty) Implements _
				ICollection(Of TProperty).Add
				If item Is Nothing Then
					Throw New ArgumentNullException("item")
				End If
				If Me._instance.Context.OnAdding(Me._PropertyName, Me._instance, item) Then
					Me._list.Add(item)
					Me._instance.Context.OnAdded(Me._PropertyName, Me._instance, item)
				End If
			End Sub
			Public Function Remove(ByVal item As TProperty) As Boolean Implements _
				ICollection(Of TProperty).Remove
				If item Is Nothing Then
					Throw New ArgumentNullException("item")
				End If
				If Me._instance.Context.OnRemoving(Me._PropertyName, Me._instance, item) Then
					If Me._list.Remove(item) Then
						Me._instance.Context.OnRemoved(Me._PropertyName, Me._instance, item)
						Return True
					End If
				End If
				Return False
			End Function
			Public Sub Clear() Implements _
				ICollection(Of TProperty).Clear
				Dim i As Integer = 0
				While i < Me._list.Count
					Me.Remove(Me._list(i))
					i = i + 1
				End While
			End Sub
			Public Function Contains(ByVal item As TProperty) As Boolean Implements _
				ICollection(Of TProperty).Contains
				Return Me._list.Contains(item)
			End Function
			Public Sub CopyTo(ByVal array As TProperty(), ByVal arrayIndex As Integer) Implements _
				ICollection(Of TProperty).CopyTo
				Me._list.CopyTo(array, arrayIndex)
			End Sub
			Public ReadOnly Property Count() As Integer Implements _
				ICollection(Of TProperty).Count
				Get
					Return Me._list.Count
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
		#Region "Person"
		Public Function CreatePerson(ByVal LastName As String, ByVal FirstName As String) As Person Implements _
			IPersonCountryDemoContext.CreatePerson
			If CObj(LastName) Is Nothing Then
				Throw New ArgumentNullException("LastName")
			End If
			If CObj(FirstName) Is Nothing Then
				Throw New ArgumentNullException("FirstName")
			End If
			If Not (Me.OnPersonLastNameChanging(Nothing, LastName)) Then
				Throw PersonCountryDemoContext.GetConstraintEnforcementFailedException("LastName")
			End If
			If Not (Me.OnPersonFirstNameChanging(Nothing, FirstName)) Then
				Throw PersonCountryDemoContext.GetConstraintEnforcementFailedException("FirstName")
			End If
			Return New PersonCore(Me, LastName, FirstName)
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
		Private Function OnPersonCountryChanging(ByVal instance As Person, ByVal newValue As Country) As Boolean
			If CObj(newValue) IsNot Nothing Then
				If CObj(Me) IsNot newValue.Context Then
					Throw PersonCountryDemoContext.GetDifferentContextsException()
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnPersonCountryChanged(ByVal instance As Person, ByVal oldValue As Country)
			If CObj(instance.Country) IsNot Nothing Then
				instance.Country.Person.Add(instance)
			End If
			If CObj(oldValue) IsNot Nothing Then
				oldValue.Person.Remove(instance)
			End If
		End Sub
		Private ReadOnly _PersonList As List(Of Person)
		Private ReadOnly _PersonReadOnlyCollection As ReadOnlyCollection(Of Person)
		Public ReadOnly Property PersonCollection() As ReadOnlyCollection(Of Person) Implements _
			IPersonCountryDemoContext.PersonCollection
			Get
				Return Me._PersonReadOnlyCollection
			End Get
		End Property
		#Region "PersonCore"
		<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
		Private NotInheritable Class PersonCore
			Inherits Person
			Public Sub New(ByVal context As PersonCountryDemoContext, ByVal LastName As String, ByVal FirstName As String)
				Me._Context = context
				Me._LastName = LastName
				Me._FirstName = FirstName
				context._PersonList.Add(Me)
			End Sub
			Private ReadOnly _Context As PersonCountryDemoContext
			Public Overrides ReadOnly Property Context() As PersonCountryDemoContext
				Get
					Return Me._Context
				End Get
			End Property
			<AccessedThroughPropertyAttribute("LastName")> _
			Private _LastName As String
			Public Overrides Property LastName() As String
				Get
					Return Me._LastName
				End Get
				Set(ByVal Value As String)
					If CObj(Value) Is Nothing Then
						Throw New ArgumentNullException("value")
					End If
					Dim oldValue As String = Me._LastName
					If Not (Object.Equals(oldValue, Value)) Then
						If Me._Context.OnPersonLastNameChanging(Me, Value) AndAlso MyBase.RaiseLastNameChangingEvent(Value) Then
							Me._LastName = Value
							MyBase.RaiseLastNameChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("FirstName")> _
			Private _FirstName As String
			Public Overrides Property FirstName() As String
				Get
					Return Me._FirstName
				End Get
				Set(ByVal Value As String)
					If CObj(Value) Is Nothing Then
						Throw New ArgumentNullException("value")
					End If
					Dim oldValue As String = Me._FirstName
					If Not (Object.Equals(oldValue, Value)) Then
						If Me._Context.OnPersonFirstNameChanging(Me, Value) AndAlso MyBase.RaiseFirstNameChangingEvent(Value) Then
							Me._FirstName = Value
							MyBase.RaiseFirstNameChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("Title")> _
			Private _Title As String
			Public Overrides Property Title() As String
				Get
					Return Me._Title
				End Get
				Set(ByVal Value As String)
					Dim oldValue As String = Me._Title
					If Not (Object.Equals(oldValue, Value)) Then
						If Me._Context.OnPersonTitleChanging(Me, Value) AndAlso MyBase.RaiseTitleChangingEvent(Value) Then
							Me._Title = Value
							MyBase.RaiseTitleChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("Country")> _
			Private _Country As Country
			Public Overrides Property Country() As Country
				Get
					Return Me._Country
				End Get
				Set(ByVal Value As Country)
					Dim oldValue As Country = Me._Country
					If CObj(oldValue) IsNot Value Then
						If Me._Context.OnPersonCountryChanging(Me, Value) AndAlso MyBase.RaiseCountryChangingEvent(Value) Then
							Me._Country = Value
							Me._Context.OnPersonCountryChanged(Me, oldValue)
							MyBase.RaiseCountryChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
		End Class
		#End Region
		#End Region
		#Region "Country"
		Public Function CreateCountry(ByVal Country_name As String) As Country Implements _
			IPersonCountryDemoContext.CreateCountry
			If CObj(Country_name) Is Nothing Then
				Throw New ArgumentNullException("Country_name")
			End If
			If Not (Me.OnCountryCountry_nameChanging(Nothing, Country_name)) Then
				Throw PersonCountryDemoContext.GetConstraintEnforcementFailedException("Country_name")
			End If
			Return New CountryCore(Me, Country_name)
		End Function
		Private Function OnCountryCountry_nameChanging(ByVal instance As Country, ByVal newValue As String) As Boolean
			Dim currentInstance As Country
			If Me._CountryCountry_nameDictionary.TryGetValue(newValue, currentInstance) Then
				If CObj(currentInstance) IsNot instance Then
					Return False
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnCountryCountry_nameChanged(ByVal instance As Country, ByVal oldValue As String)
			Me._CountryCountry_nameDictionary.Add(instance.Country_name, instance)
			If CObj(oldValue) IsNot Nothing Then
				Me._CountryCountry_nameDictionary.Remove(oldValue)
			End If
		End Sub
		Private Function OnCountryRegion_Region_codeChanging(ByVal instance As Country, ByVal newValue As String) As Boolean
			Return True
		End Function
		Private Function OnCountryPersonAdding(ByVal instance As Country, ByVal value As Person) As Boolean
			If CObj(Me) IsNot value.Context Then
				Throw PersonCountryDemoContext.GetDifferentContextsException()
			End If
			Return True
		End Function
		Private Sub OnCountryPersonAdded(ByVal instance As Country, ByVal value As Person)
			value.Country = instance
		End Sub
		Private Sub OnCountryPersonRemoved(ByVal instance As Country, ByVal value As Person)
			value.Country = Nothing
		End Sub
		Private ReadOnly _CountryList As List(Of Country)
		Private ReadOnly _CountryReadOnlyCollection As ReadOnlyCollection(Of Country)
		Public ReadOnly Property CountryCollection() As ReadOnlyCollection(Of Country) Implements _
			IPersonCountryDemoContext.CountryCollection
			Get
				Return Me._CountryReadOnlyCollection
			End Get
		End Property
		#Region "CountryCore"
		<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
		Private NotInheritable Class CountryCore
			Inherits Country
			Public Sub New(ByVal context As PersonCountryDemoContext, ByVal Country_name As String)
				Me._Context = context
				Me._Person = New ConstraintEnforcementCollection(Of Country, Person)(Me)
				Me._Country_name = Country_name
				context.OnCountryCountry_nameChanged(Me, Nothing)
				context._CountryList.Add(Me)
			End Sub
			Private ReadOnly _Context As PersonCountryDemoContext
			Public Overrides ReadOnly Property Context() As PersonCountryDemoContext
				Get
					Return Me._Context
				End Get
			End Property
			<AccessedThroughPropertyAttribute("Country_name")> _
			Private _Country_name As String
			Public Overrides Property Country_name() As String
				Get
					Return Me._Country_name
				End Get
				Set(ByVal Value As String)
					If CObj(Value) Is Nothing Then
						Throw New ArgumentNullException("value")
					End If
					Dim oldValue As String = Me._Country_name
					If Not (Object.Equals(oldValue, Value)) Then
						If Me._Context.OnCountryCountry_nameChanging(Me, Value) AndAlso MyBase.RaiseCountry_nameChangingEvent(Value) Then
							Me._Country_name = Value
							Me._Context.OnCountryCountry_nameChanged(Me, oldValue)
							MyBase.RaiseCountry_nameChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("Region_Region_code")> _
			Private _Region_Region_code As String
			Public Overrides Property Region_Region_code() As String
				Get
					Return Me._Region_Region_code
				End Get
				Set(ByVal Value As String)
					Dim oldValue As String = Me._Region_Region_code
					If Not (Object.Equals(oldValue, Value)) Then
						If Me._Context.OnCountryRegion_Region_codeChanging(Me, Value) AndAlso MyBase.RaiseRegion_Region_codeChangingEvent(Value) Then
							Me._Region_Region_code = Value
							MyBase.RaiseRegion_Region_codeChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("Person")> _
			Private ReadOnly _Person As ICollection(Of Person)
			Public Overrides ReadOnly Property Person() As ICollection(Of Person)
				Get
					Return Me._Person
				End Get
			End Property
		End Class
		#End Region
		#End Region
	End Class
	#End Region
End Namespace
