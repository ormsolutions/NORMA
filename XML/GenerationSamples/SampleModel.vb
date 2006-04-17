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
Namespace SampleModel
	#Region "PersonDrivesCar"
	<DataObjectAttribute()> _
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
	Public MustInherit Partial Class PersonDrivesCar
		Implements INotifyPropertyChanged
		Implements IHasSampleModelContext
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
		Public MustOverride ReadOnly Property Context() As SampleModelContext Implements _
			IHasSampleModelContext.Context
		Public Custom Event DrivesCar_vinChanging As EventHandler(Of PropertyChangingEventArgs(Of PersonDrivesCar, Integer))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Combine(Me.Events(0), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Remove(Me.Events(0), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseDrivesCar_vinChangingEvent(ByVal newValue As Integer) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of PersonDrivesCar, Integer)) = TryCast(Me.Events(0), EventHandler(Of PropertyChangingEventArgs(Of PersonDrivesCar, Integer)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of PersonDrivesCar, Integer) = New PropertyChangingEventArgs(Of PersonDrivesCar, Integer)(Me, Me.DrivesCar_vin, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event DrivesCar_vinChanged As EventHandler(Of PropertyChangedEventArgs(Of PersonDrivesCar, Integer))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Combine(Me.Events(0), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Remove(Me.Events(0), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseDrivesCar_vinChangedEvent(ByVal oldValue As Integer)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of PersonDrivesCar, Integer)) = TryCast(Me.Events(0), EventHandler(Of PropertyChangedEventArgs(Of PersonDrivesCar, Integer)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of PersonDrivesCar, Integer)(Me, oldValue, Me.DrivesCar_vin), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("DrivesCar_vin")
			End If
		End Sub
		Public Custom Event DrivenByPersonChanging As EventHandler(Of PropertyChangingEventArgs(Of PersonDrivesCar, Person))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseDrivenByPersonChangingEvent(ByVal newValue As Person) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of PersonDrivesCar, Person)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangingEventArgs(Of PersonDrivesCar, Person)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of PersonDrivesCar, Person) = New PropertyChangingEventArgs(Of PersonDrivesCar, Person)(Me, Me.DrivenByPerson, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event DrivenByPersonChanged As EventHandler(Of PropertyChangedEventArgs(Of PersonDrivesCar, Person))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseDrivenByPersonChangedEvent(ByVal oldValue As Person)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of PersonDrivesCar, Person)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangedEventArgs(Of PersonDrivesCar, Person)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of PersonDrivesCar, Person)(Me, oldValue, Me.DrivenByPerson), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("DrivenByPerson")
			End If
		End Sub
		<DataObjectFieldAttribute(False, False, False)> _
		Public MustOverride Property DrivesCar_vin() As Integer
		<DataObjectFieldAttribute(False, False, False)> _
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
	<DataObjectAttribute()> _
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
	Public MustInherit Partial Class PersonBoughtCarFromPersonOnDate
		Implements INotifyPropertyChanged
		Implements IHasSampleModelContext
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
		Public MustOverride ReadOnly Property Context() As SampleModelContext Implements _
			IHasSampleModelContext.Context
		Public Custom Event CarSold_vinChanging As EventHandler(Of PropertyChangingEventArgs(Of PersonBoughtCarFromPersonOnDate, Integer))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Combine(Me.Events(0), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Remove(Me.Events(0), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseCarSold_vinChangingEvent(ByVal newValue As Integer) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of PersonBoughtCarFromPersonOnDate, Integer)) = TryCast(Me.Events(0), EventHandler(Of PropertyChangingEventArgs(Of PersonBoughtCarFromPersonOnDate, Integer)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of PersonBoughtCarFromPersonOnDate, Integer) = New PropertyChangingEventArgs(Of PersonBoughtCarFromPersonOnDate, Integer)(Me, Me.CarSold_vin, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event CarSold_vinChanged As EventHandler(Of PropertyChangedEventArgs(Of PersonBoughtCarFromPersonOnDate, Integer))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Combine(Me.Events(0), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Remove(Me.Events(0), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseCarSold_vinChangedEvent(ByVal oldValue As Integer)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of PersonBoughtCarFromPersonOnDate, Integer)) = TryCast(Me.Events(0), EventHandler(Of PropertyChangedEventArgs(Of PersonBoughtCarFromPersonOnDate, Integer)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of PersonBoughtCarFromPersonOnDate, Integer)(Me, oldValue, Me.CarSold_vin), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("CarSold_vin")
			End If
		End Sub
		Public Custom Event SaleDate_YMDChanging As EventHandler(Of PropertyChangingEventArgs(Of PersonBoughtCarFromPersonOnDate, Integer))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseSaleDate_YMDChangingEvent(ByVal newValue As Integer) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of PersonBoughtCarFromPersonOnDate, Integer)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangingEventArgs(Of PersonBoughtCarFromPersonOnDate, Integer)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of PersonBoughtCarFromPersonOnDate, Integer) = New PropertyChangingEventArgs(Of PersonBoughtCarFromPersonOnDate, Integer)(Me, Me.SaleDate_YMD, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event SaleDate_YMDChanged As EventHandler(Of PropertyChangedEventArgs(Of PersonBoughtCarFromPersonOnDate, Integer))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseSaleDate_YMDChangedEvent(ByVal oldValue As Integer)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of PersonBoughtCarFromPersonOnDate, Integer)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangedEventArgs(Of PersonBoughtCarFromPersonOnDate, Integer)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of PersonBoughtCarFromPersonOnDate, Integer)(Me, oldValue, Me.SaleDate_YMD), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("SaleDate_YMD")
			End If
		End Sub
		Public Custom Event BuyerChanging As EventHandler(Of PropertyChangingEventArgs(Of PersonBoughtCarFromPersonOnDate, Person))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseBuyerChangingEvent(ByVal newValue As Person) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of PersonBoughtCarFromPersonOnDate, Person)) = TryCast(Me.Events(2), EventHandler(Of PropertyChangingEventArgs(Of PersonBoughtCarFromPersonOnDate, Person)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of PersonBoughtCarFromPersonOnDate, Person) = New PropertyChangingEventArgs(Of PersonBoughtCarFromPersonOnDate, Person)(Me, Me.Buyer, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event BuyerChanged As EventHandler(Of PropertyChangedEventArgs(Of PersonBoughtCarFromPersonOnDate, Person))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseBuyerChangedEvent(ByVal oldValue As Person)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of PersonBoughtCarFromPersonOnDate, Person)) = TryCast(Me.Events(2), EventHandler(Of PropertyChangedEventArgs(Of PersonBoughtCarFromPersonOnDate, Person)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of PersonBoughtCarFromPersonOnDate, Person)(Me, oldValue, Me.Buyer), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Buyer")
			End If
		End Sub
		Public Custom Event SellerChanging As EventHandler(Of PropertyChangingEventArgs(Of PersonBoughtCarFromPersonOnDate, Person))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Combine(Me.Events(3), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Remove(Me.Events(3), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseSellerChangingEvent(ByVal newValue As Person) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of PersonBoughtCarFromPersonOnDate, Person)) = TryCast(Me.Events(3), EventHandler(Of PropertyChangingEventArgs(Of PersonBoughtCarFromPersonOnDate, Person)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of PersonBoughtCarFromPersonOnDate, Person) = New PropertyChangingEventArgs(Of PersonBoughtCarFromPersonOnDate, Person)(Me, Me.Seller, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event SellerChanged As EventHandler(Of PropertyChangedEventArgs(Of PersonBoughtCarFromPersonOnDate, Person))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Combine(Me.Events(3), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Remove(Me.Events(3), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseSellerChangedEvent(ByVal oldValue As Person)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of PersonBoughtCarFromPersonOnDate, Person)) = TryCast(Me.Events(3), EventHandler(Of PropertyChangedEventArgs(Of PersonBoughtCarFromPersonOnDate, Person)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of PersonBoughtCarFromPersonOnDate, Person)(Me, oldValue, Me.Seller), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Seller")
			End If
		End Sub
		<DataObjectFieldAttribute(False, False, False)> _
		Public MustOverride Property CarSold_vin() As Integer
		<DataObjectFieldAttribute(False, False, False)> _
		Public MustOverride Property SaleDate_YMD() As Integer
		<DataObjectFieldAttribute(False, False, False)> _
		Public MustOverride Property Buyer() As Person
		<DataObjectFieldAttribute(False, False, False)> _
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
	<DataObjectAttribute()> _
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
	Public MustInherit Partial Class Review
		Implements INotifyPropertyChanged
		Implements IHasSampleModelContext
		Protected Sub New()
		End Sub
		Private _events As System.Delegate()
		Private ReadOnly Property Events() As System.Delegate()
			Get
				If CObj(Me._events) Is Nothing Then
					Me._events = New System.Delegate(3) {}
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
		Public MustOverride ReadOnly Property Context() As SampleModelContext Implements _
			IHasSampleModelContext.Context
		Public Custom Event Car_vinChanging As EventHandler(Of PropertyChangingEventArgs(Of Review, Integer))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Combine(Me.Events(0), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Remove(Me.Events(0), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseCar_vinChangingEvent(ByVal newValue As Integer) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Review, Integer)) = TryCast(Me.Events(0), EventHandler(Of PropertyChangingEventArgs(Of Review, Integer)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Review, Integer) = New PropertyChangingEventArgs(Of Review, Integer)(Me, Me.Car_vin, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event Car_vinChanged As EventHandler(Of PropertyChangedEventArgs(Of Review, Integer))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Combine(Me.Events(0), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Remove(Me.Events(0), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseCar_vinChangedEvent(ByVal oldValue As Integer)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Review, Integer)) = TryCast(Me.Events(0), EventHandler(Of PropertyChangedEventArgs(Of Review, Integer)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Review, Integer)(Me, oldValue, Me.Car_vin), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Car_vin")
			End If
		End Sub
		Public Custom Event Rating_Nr_IntegerChanging As EventHandler(Of PropertyChangingEventArgs(Of Review, Integer))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseRating_Nr_IntegerChangingEvent(ByVal newValue As Integer) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Review, Integer)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangingEventArgs(Of Review, Integer)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Review, Integer) = New PropertyChangingEventArgs(Of Review, Integer)(Me, Me.Rating_Nr_Integer, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event Rating_Nr_IntegerChanged As EventHandler(Of PropertyChangedEventArgs(Of Review, Integer))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseRating_Nr_IntegerChangedEvent(ByVal oldValue As Integer)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Review, Integer)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangedEventArgs(Of Review, Integer)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Review, Integer)(Me, oldValue, Me.Rating_Nr_Integer), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Rating_Nr_Integer")
			End If
		End Sub
		Public Custom Event Criterion_NameChanging As EventHandler(Of PropertyChangingEventArgs(Of Review, String))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseCriterion_NameChangingEvent(ByVal newValue As String) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Review, String)) = TryCast(Me.Events(2), EventHandler(Of PropertyChangingEventArgs(Of Review, String)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Review, String) = New PropertyChangingEventArgs(Of Review, String)(Me, Me.Criterion_Name, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event Criterion_NameChanged As EventHandler(Of PropertyChangedEventArgs(Of Review, String))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseCriterion_NameChangedEvent(ByVal oldValue As String)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Review, String)) = TryCast(Me.Events(2), EventHandler(Of PropertyChangedEventArgs(Of Review, String)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Review, String)(Me, oldValue, Me.Criterion_Name), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Criterion_Name")
			End If
		End Sub
		<DataObjectFieldAttribute(False, False, False)> _
		Public MustOverride Property Car_vin() As Integer
		<DataObjectFieldAttribute(False, False, False)> _
		Public MustOverride Property Rating_Nr_Integer() As Integer
		<DataObjectFieldAttribute(False, False, False)> _
		Public MustOverride Property Criterion_Name() As String
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Overridable Function ToString(ByVal provider As IFormatProvider) As String
			Return String.Format(provider, "Review{0}{{{0}{1}Car_vin = ""{2}"",{0}{1}Rating_Nr_Integer = ""{3}"",{0}{1}Criterion_Name = ""{4}""{0}}}", Environment.NewLine, "", Me.Car_vin, Me.Rating_Nr_Integer, Me.Criterion_Name)
		End Function
	End Class
	#End Region
	#Region "PersonHasNickName"
	<DataObjectAttribute()> _
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
	Public MustInherit Partial Class PersonHasNickName
		Implements INotifyPropertyChanged
		Implements IHasSampleModelContext
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
		Public MustOverride ReadOnly Property Context() As SampleModelContext Implements _
			IHasSampleModelContext.Context
		Public Custom Event NickNameChanging As EventHandler(Of PropertyChangingEventArgs(Of PersonHasNickName, String))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Combine(Me.Events(0), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Remove(Me.Events(0), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseNickNameChangingEvent(ByVal newValue As String) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of PersonHasNickName, String)) = TryCast(Me.Events(0), EventHandler(Of PropertyChangingEventArgs(Of PersonHasNickName, String)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of PersonHasNickName, String) = New PropertyChangingEventArgs(Of PersonHasNickName, String)(Me, Me.NickName, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event NickNameChanged As EventHandler(Of PropertyChangedEventArgs(Of PersonHasNickName, String))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Combine(Me.Events(0), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Remove(Me.Events(0), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseNickNameChangedEvent(ByVal oldValue As String)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of PersonHasNickName, String)) = TryCast(Me.Events(0), EventHandler(Of PropertyChangedEventArgs(Of PersonHasNickName, String)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of PersonHasNickName, String)(Me, oldValue, Me.NickName), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("NickName")
			End If
		End Sub
		Public Custom Event PersonChanging As EventHandler(Of PropertyChangingEventArgs(Of PersonHasNickName, Person))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaisePersonChangingEvent(ByVal newValue As Person) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of PersonHasNickName, Person)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangingEventArgs(Of PersonHasNickName, Person)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of PersonHasNickName, Person) = New PropertyChangingEventArgs(Of PersonHasNickName, Person)(Me, Me.Person, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event PersonChanged As EventHandler(Of PropertyChangedEventArgs(Of PersonHasNickName, Person))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaisePersonChangedEvent(ByVal oldValue As Person)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of PersonHasNickName, Person)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangedEventArgs(Of PersonHasNickName, Person)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of PersonHasNickName, Person)(Me, oldValue, Me.Person), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Person")
			End If
		End Sub
		<DataObjectFieldAttribute(False, False, False)> _
		Public MustOverride Property NickName() As String
		<DataObjectFieldAttribute(False, False, False)> _
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
	<DataObjectAttribute()> _
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
	Public MustInherit Partial Class Person
		Implements INotifyPropertyChanged
		Implements IHasSampleModelContext
		Protected Sub New()
		End Sub
		Private _events As System.Delegate()
		Private ReadOnly Property Events() As System.Delegate()
			Get
				If CObj(Me._events) Is Nothing Then
					Me._events = New System.Delegate(17) {}
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
		Public MustOverride ReadOnly Property Context() As SampleModelContext Implements _
			IHasSampleModelContext.Context
		Public Custom Event FirstNameChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Combine(Me.Events(0), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Remove(Me.Events(0), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseFirstNameChangingEvent(ByVal newValue As String) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Person, String)) = TryCast(Me.Events(0), EventHandler(Of PropertyChangingEventArgs(Of Person, String)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Person, String) = New PropertyChangingEventArgs(Of Person, String)(Me, Me.FirstName, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event FirstNameChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Combine(Me.Events(0), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Remove(Me.Events(0), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseFirstNameChangedEvent(ByVal oldValue As String)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Person, String)) = TryCast(Me.Events(0), EventHandler(Of PropertyChangedEventArgs(Of Person, String)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Person, String)(Me, oldValue, Me.FirstName), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("FirstName")
			End If
		End Sub
		Public Custom Event Date_YMDChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Integer))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseDate_YMDChangingEvent(ByVal newValue As Integer) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Person, Integer)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangingEventArgs(Of Person, Integer)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Person, Integer) = New PropertyChangingEventArgs(Of Person, Integer)(Me, Me.Date_YMD, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event Date_YMDChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Integer))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseDate_YMDChangedEvent(ByVal oldValue As Integer)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Person, Integer)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangedEventArgs(Of Person, Integer)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Person, Integer)(Me, oldValue, Me.Date_YMD), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Date_YMD")
			End If
		End Sub
		Public Custom Event LastNameChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseLastNameChangingEvent(ByVal newValue As String) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Person, String)) = TryCast(Me.Events(2), EventHandler(Of PropertyChangingEventArgs(Of Person, String)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Person, String) = New PropertyChangingEventArgs(Of Person, String)(Me, Me.LastName, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event LastNameChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseLastNameChangedEvent(ByVal oldValue As String)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Person, String)) = TryCast(Me.Events(2), EventHandler(Of PropertyChangedEventArgs(Of Person, String)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Person, String)(Me, oldValue, Me.LastName), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("LastName")
			End If
		End Sub
		Public Custom Event OptionalUniqueStringChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Combine(Me.Events(3), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Remove(Me.Events(3), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseOptionalUniqueStringChangingEvent(ByVal newValue As String) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Person, String)) = TryCast(Me.Events(3), EventHandler(Of PropertyChangingEventArgs(Of Person, String)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Person, String) = New PropertyChangingEventArgs(Of Person, String)(Me, Me.OptionalUniqueString, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event OptionalUniqueStringChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Combine(Me.Events(3), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Remove(Me.Events(3), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseOptionalUniqueStringChangedEvent(ByVal oldValue As String)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Person, String)) = TryCast(Me.Events(3), EventHandler(Of PropertyChangedEventArgs(Of Person, String)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Person, String)(Me, oldValue, Me.OptionalUniqueString), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("OptionalUniqueString")
			End If
		End Sub
		Public Custom Event HatType_ColorARGBChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Nullable(Of Integer)))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(4) = System.Delegate.Combine(Me.Events(4), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(4) = System.Delegate.Remove(Me.Events(4), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseHatType_ColorARGBChangingEvent(ByVal newValue As Nullable(Of Integer)) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Person, Nullable(Of Integer))) = TryCast(Me.Events(4), EventHandler(Of PropertyChangingEventArgs(Of Person, Nullable(Of Integer))))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Person, Nullable(Of Integer)) = New PropertyChangingEventArgs(Of Person, Nullable(Of Integer))(Me, Me.HatType_ColorARGB, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event HatType_ColorARGBChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Nullable(Of Integer)))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(4) = System.Delegate.Combine(Me.Events(4), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(4) = System.Delegate.Remove(Me.Events(4), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseHatType_ColorARGBChangedEvent(ByVal oldValue As Nullable(Of Integer))
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Person, Nullable(Of Integer))) = TryCast(Me.Events(4), EventHandler(Of PropertyChangedEventArgs(Of Person, Nullable(Of Integer))))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Person, Nullable(Of Integer))(Me, oldValue, Me.HatType_ColorARGB), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("HatType_ColorARGB")
			End If
		End Sub
		Public Custom Event HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(5) = System.Delegate.Combine(Me.Events(5), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(5) = System.Delegate.Remove(Me.Events(5), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseHatType_HatTypeStyle_HatTypeStyle_DescriptionChangingEvent(ByVal newValue As String) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Person, String)) = TryCast(Me.Events(5), EventHandler(Of PropertyChangingEventArgs(Of Person, String)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Person, String) = New PropertyChangingEventArgs(Of Person, String)(Me, Me.HatType_HatTypeStyle_HatTypeStyle_Description, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(5) = System.Delegate.Combine(Me.Events(5), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(5) = System.Delegate.Remove(Me.Events(5), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseHatType_HatTypeStyle_HatTypeStyle_DescriptionChangedEvent(ByVal oldValue As String)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Person, String)) = TryCast(Me.Events(5), EventHandler(Of PropertyChangedEventArgs(Of Person, String)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Person, String)(Me, oldValue, Me.HatType_HatTypeStyle_HatTypeStyle_Description), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("HatType_HatTypeStyle_HatTypeStyle_Description")
			End If
		End Sub
		Public Custom Event OwnsCar_vinChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Nullable(Of Integer)))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(6) = System.Delegate.Combine(Me.Events(6), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(6) = System.Delegate.Remove(Me.Events(6), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseOwnsCar_vinChangingEvent(ByVal newValue As Nullable(Of Integer)) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Person, Nullable(Of Integer))) = TryCast(Me.Events(6), EventHandler(Of PropertyChangingEventArgs(Of Person, Nullable(Of Integer))))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Person, Nullable(Of Integer)) = New PropertyChangingEventArgs(Of Person, Nullable(Of Integer))(Me, Me.OwnsCar_vin, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event OwnsCar_vinChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Nullable(Of Integer)))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(6) = System.Delegate.Combine(Me.Events(6), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(6) = System.Delegate.Remove(Me.Events(6), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseOwnsCar_vinChangedEvent(ByVal oldValue As Nullable(Of Integer))
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Person, Nullable(Of Integer))) = TryCast(Me.Events(6), EventHandler(Of PropertyChangedEventArgs(Of Person, Nullable(Of Integer))))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Person, Nullable(Of Integer))(Me, oldValue, Me.OwnsCar_vin), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("OwnsCar_vin")
			End If
		End Sub
		Public Custom Event Gender_Gender_CodeChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(7) = System.Delegate.Combine(Me.Events(7), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(7) = System.Delegate.Remove(Me.Events(7), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseGender_Gender_CodeChangingEvent(ByVal newValue As String) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Person, String)) = TryCast(Me.Events(7), EventHandler(Of PropertyChangingEventArgs(Of Person, String)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Person, String) = New PropertyChangingEventArgs(Of Person, String)(Me, Me.Gender_Gender_Code, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event Gender_Gender_CodeChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(7) = System.Delegate.Combine(Me.Events(7), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(7) = System.Delegate.Remove(Me.Events(7), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseGender_Gender_CodeChangedEvent(ByVal oldValue As String)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Person, String)) = TryCast(Me.Events(7), EventHandler(Of PropertyChangedEventArgs(Of Person, String)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Person, String)(Me, oldValue, Me.Gender_Gender_Code), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Gender_Gender_Code")
			End If
		End Sub
		Public Custom Event PersonHasParentsChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Nullable(Of Boolean)))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(8) = System.Delegate.Combine(Me.Events(8), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(8) = System.Delegate.Remove(Me.Events(8), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaisePersonHasParentsChangingEvent(ByVal newValue As Nullable(Of Boolean)) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Person, Nullable(Of Boolean))) = TryCast(Me.Events(8), EventHandler(Of PropertyChangingEventArgs(Of Person, Nullable(Of Boolean))))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Person, Nullable(Of Boolean)) = New PropertyChangingEventArgs(Of Person, Nullable(Of Boolean))(Me, Me.PersonHasParents, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event PersonHasParentsChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Nullable(Of Boolean)))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(8) = System.Delegate.Combine(Me.Events(8), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(8) = System.Delegate.Remove(Me.Events(8), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaisePersonHasParentsChangedEvent(ByVal oldValue As Nullable(Of Boolean))
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Person, Nullable(Of Boolean))) = TryCast(Me.Events(8), EventHandler(Of PropertyChangedEventArgs(Of Person, Nullable(Of Boolean))))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Person, Nullable(Of Boolean))(Me, oldValue, Me.PersonHasParents), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("PersonHasParents")
			End If
		End Sub
		Public Custom Event OptionalUniqueDecimalChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Nullable(Of Decimal)))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(9) = System.Delegate.Combine(Me.Events(9), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(9) = System.Delegate.Remove(Me.Events(9), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseOptionalUniqueDecimalChangingEvent(ByVal newValue As Nullable(Of Decimal)) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Person, Nullable(Of Decimal))) = TryCast(Me.Events(9), EventHandler(Of PropertyChangingEventArgs(Of Person, Nullable(Of Decimal))))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Person, Nullable(Of Decimal)) = New PropertyChangingEventArgs(Of Person, Nullable(Of Decimal))(Me, Me.OptionalUniqueDecimal, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event OptionalUniqueDecimalChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Nullable(Of Decimal)))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(9) = System.Delegate.Combine(Me.Events(9), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(9) = System.Delegate.Remove(Me.Events(9), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseOptionalUniqueDecimalChangedEvent(ByVal oldValue As Nullable(Of Decimal))
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Person, Nullable(Of Decimal))) = TryCast(Me.Events(9), EventHandler(Of PropertyChangedEventArgs(Of Person, Nullable(Of Decimal))))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Person, Nullable(Of Decimal))(Me, oldValue, Me.OptionalUniqueDecimal), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("OptionalUniqueDecimal")
			End If
		End Sub
		Public Custom Event MandatoryUniqueDecimalChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Decimal))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(10) = System.Delegate.Combine(Me.Events(10), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(10) = System.Delegate.Remove(Me.Events(10), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseMandatoryUniqueDecimalChangingEvent(ByVal newValue As Decimal) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Person, Decimal)) = TryCast(Me.Events(10), EventHandler(Of PropertyChangingEventArgs(Of Person, Decimal)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Person, Decimal) = New PropertyChangingEventArgs(Of Person, Decimal)(Me, Me.MandatoryUniqueDecimal, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event MandatoryUniqueDecimalChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Decimal))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(10) = System.Delegate.Combine(Me.Events(10), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(10) = System.Delegate.Remove(Me.Events(10), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseMandatoryUniqueDecimalChangedEvent(ByVal oldValue As Decimal)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Person, Decimal)) = TryCast(Me.Events(10), EventHandler(Of PropertyChangedEventArgs(Of Person, Decimal)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Person, Decimal)(Me, oldValue, Me.MandatoryUniqueDecimal), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("MandatoryUniqueDecimal")
			End If
		End Sub
		Public Custom Event MandatoryUniqueStringChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(11) = System.Delegate.Combine(Me.Events(11), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(11) = System.Delegate.Remove(Me.Events(11), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseMandatoryUniqueStringChangingEvent(ByVal newValue As String) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Person, String)) = TryCast(Me.Events(11), EventHandler(Of PropertyChangingEventArgs(Of Person, String)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Person, String) = New PropertyChangingEventArgs(Of Person, String)(Me, Me.MandatoryUniqueString, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event MandatoryUniqueStringChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(11) = System.Delegate.Combine(Me.Events(11), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(11) = System.Delegate.Remove(Me.Events(11), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseMandatoryUniqueStringChangedEvent(ByVal oldValue As String)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Person, String)) = TryCast(Me.Events(11), EventHandler(Of PropertyChangedEventArgs(Of Person, String)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Person, String)(Me, oldValue, Me.MandatoryUniqueString), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("MandatoryUniqueString")
			End If
		End Sub
		Public Custom Event ValueType1DoesSomethingElseWithChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, ValueType1))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(12) = System.Delegate.Combine(Me.Events(12), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(12) = System.Delegate.Remove(Me.Events(12), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseValueType1DoesSomethingElseWithChangingEvent(ByVal newValue As ValueType1) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Person, ValueType1)) = TryCast(Me.Events(12), EventHandler(Of PropertyChangingEventArgs(Of Person, ValueType1)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Person, ValueType1) = New PropertyChangingEventArgs(Of Person, ValueType1)(Me, Me.ValueType1DoesSomethingElseWith, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event ValueType1DoesSomethingElseWithChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, ValueType1))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(12) = System.Delegate.Combine(Me.Events(12), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(12) = System.Delegate.Remove(Me.Events(12), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseValueType1DoesSomethingElseWithChangedEvent(ByVal oldValue As ValueType1)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Person, ValueType1)) = TryCast(Me.Events(12), EventHandler(Of PropertyChangedEventArgs(Of Person, ValueType1)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Person, ValueType1)(Me, oldValue, Me.ValueType1DoesSomethingElseWith), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("ValueType1DoesSomethingElseWith")
			End If
		End Sub
		Public Custom Event MalePersonChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, MalePerson))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(13) = System.Delegate.Combine(Me.Events(13), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(13) = System.Delegate.Remove(Me.Events(13), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseMalePersonChangingEvent(ByVal newValue As MalePerson) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Person, MalePerson)) = TryCast(Me.Events(13), EventHandler(Of PropertyChangingEventArgs(Of Person, MalePerson)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Person, MalePerson) = New PropertyChangingEventArgs(Of Person, MalePerson)(Me, Me.MalePerson, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event MalePersonChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, MalePerson))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(13) = System.Delegate.Combine(Me.Events(13), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(13) = System.Delegate.Remove(Me.Events(13), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseMalePersonChangedEvent(ByVal oldValue As MalePerson)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Person, MalePerson)) = TryCast(Me.Events(13), EventHandler(Of PropertyChangedEventArgs(Of Person, MalePerson)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Person, MalePerson)(Me, oldValue, Me.MalePerson), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("MalePerson")
			End If
		End Sub
		Public Custom Event FemalePersonChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, FemalePerson))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(14) = System.Delegate.Combine(Me.Events(14), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(14) = System.Delegate.Remove(Me.Events(14), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseFemalePersonChangingEvent(ByVal newValue As FemalePerson) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Person, FemalePerson)) = TryCast(Me.Events(14), EventHandler(Of PropertyChangingEventArgs(Of Person, FemalePerson)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Person, FemalePerson) = New PropertyChangingEventArgs(Of Person, FemalePerson)(Me, Me.FemalePerson, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event FemalePersonChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, FemalePerson))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(14) = System.Delegate.Combine(Me.Events(14), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(14) = System.Delegate.Remove(Me.Events(14), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseFemalePersonChangedEvent(ByVal oldValue As FemalePerson)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Person, FemalePerson)) = TryCast(Me.Events(14), EventHandler(Of PropertyChangedEventArgs(Of Person, FemalePerson)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Person, FemalePerson)(Me, oldValue, Me.FemalePerson), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("FemalePerson")
			End If
		End Sub
		Public Custom Event ChildPersonChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, ChildPerson))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(15) = System.Delegate.Combine(Me.Events(15), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(15) = System.Delegate.Remove(Me.Events(15), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseChildPersonChangingEvent(ByVal newValue As ChildPerson) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Person, ChildPerson)) = TryCast(Me.Events(15), EventHandler(Of PropertyChangingEventArgs(Of Person, ChildPerson)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Person, ChildPerson) = New PropertyChangingEventArgs(Of Person, ChildPerson)(Me, Me.ChildPerson, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event ChildPersonChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, ChildPerson))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(15) = System.Delegate.Combine(Me.Events(15), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(15) = System.Delegate.Remove(Me.Events(15), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseChildPersonChangedEvent(ByVal oldValue As ChildPerson)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Person, ChildPerson)) = TryCast(Me.Events(15), EventHandler(Of PropertyChangedEventArgs(Of Person, ChildPerson)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Person, ChildPerson)(Me, oldValue, Me.ChildPerson), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("ChildPerson")
			End If
		End Sub
		Public Custom Event DeathChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Death))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(16) = System.Delegate.Combine(Me.Events(16), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(16) = System.Delegate.Remove(Me.Events(16), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseDeathChangingEvent(ByVal newValue As Death) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Person, Death)) = TryCast(Me.Events(16), EventHandler(Of PropertyChangingEventArgs(Of Person, Death)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Person, Death) = New PropertyChangingEventArgs(Of Person, Death)(Me, Me.Death, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event DeathChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Death))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(16) = System.Delegate.Combine(Me.Events(16), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(16) = System.Delegate.Remove(Me.Events(16), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseDeathChangedEvent(ByVal oldValue As Death)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Person, Death)) = TryCast(Me.Events(16), EventHandler(Of PropertyChangedEventArgs(Of Person, Death)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Person, Death)(Me, oldValue, Me.Death), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Death")
			End If
		End Sub
		<DataObjectFieldAttribute(False, False, False)> _
		Public MustOverride Property FirstName() As String
		<DataObjectFieldAttribute(False, False, False)> _
		Public MustOverride Property Date_YMD() As Integer
		<DataObjectFieldAttribute(False, False, False)> _
		Public MustOverride Property LastName() As String
		<DataObjectFieldAttribute(False, False, True)> _
		Public MustOverride Property OptionalUniqueString() As String
		<DataObjectFieldAttribute(False, False, True)> _
		Public MustOverride Property HatType_ColorARGB() As Nullable(Of Integer)
		<DataObjectFieldAttribute(False, False, True)> _
		Public MustOverride Property HatType_HatTypeStyle_HatTypeStyle_Description() As String
		<DataObjectFieldAttribute(False, False, True)> _
		Public MustOverride Property OwnsCar_vin() As Nullable(Of Integer)
		<DataObjectFieldAttribute(False, False, False)> _
		Public MustOverride Property Gender_Gender_Code() As String
		<DataObjectFieldAttribute(False, False, True)> _
		Public MustOverride Property PersonHasParents() As Nullable(Of Boolean)
		<DataObjectFieldAttribute(False, False, True)> _
		Public MustOverride Property OptionalUniqueDecimal() As Nullable(Of Decimal)
		<DataObjectFieldAttribute(False, False, False)> _
		Public MustOverride Property MandatoryUniqueDecimal() As Decimal
		<DataObjectFieldAttribute(False, False, False)> _
		Public MustOverride Property MandatoryUniqueString() As String
		<DataObjectFieldAttribute(False, False, True)> _
		Public MustOverride Property ValueType1DoesSomethingElseWith() As ValueType1
		<DataObjectFieldAttribute(False, False, True)> _
		Public MustOverride Property MalePerson() As MalePerson
		<DataObjectFieldAttribute(False, False, True)> _
		Public MustOverride Property FemalePerson() As FemalePerson
		<DataObjectFieldAttribute(False, False, True)> _
		Public MustOverride Property ChildPerson() As ChildPerson
		<DataObjectFieldAttribute(False, False, True)> _
		Public MustOverride Property Death() As Death
		<DataObjectFieldAttribute(False, False, True)> _
		Public MustOverride ReadOnly Property PersonDrivesCarAsDrivenByPerson() As ICollection(Of PersonDrivesCar)
		<DataObjectFieldAttribute(False, False, True)> _
		Public MustOverride ReadOnly Property PersonBoughtCarFromPersonOnDateAsBuyer() As ICollection(Of PersonBoughtCarFromPersonOnDate)
		<DataObjectFieldAttribute(False, False, True)> _
		Public MustOverride ReadOnly Property PersonBoughtCarFromPersonOnDateAsSeller() As ICollection(Of PersonBoughtCarFromPersonOnDate)
		<DataObjectFieldAttribute(False, False, True)> _
		Public MustOverride ReadOnly Property PersonHasNickNameAsPerson() As ICollection(Of PersonHasNickName)
		<DataObjectFieldAttribute(False, False, True)> _
		Public MustOverride ReadOnly Property Task() As ICollection(Of Task)
		<DataObjectFieldAttribute(False, False, True)> _
		Public MustOverride ReadOnly Property ValueType1DoesSomethingWith() As ICollection(Of ValueType1)
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Overridable Function ToString(ByVal provider As IFormatProvider) As String
			Return String.Format(provider, "Person{0}{{{0}{1}FirstName = ""{2}"",{0}{1}Date_YMD = ""{3}"",{0}{1}LastName = ""{4}"",{0}{1}OptionalUniqueString = ""{5}"",{0}{1}HatType_ColorARGB = ""{6}"",{0}{1}HatType_HatTypeStyle_HatTypeStyle_Description = ""{7}"",{0}{1}OwnsCar_vin = ""{8}"",{0}{1}Gender_Gender_Code = ""{9}"",{0}{1}PersonHasParents = ""{10}"",{0}{1}OptionalUniqueDecimal = ""{11}"",{0}{1}MandatoryUniqueDecimal = ""{12}"",{0}{1}MandatoryUniqueString = ""{13}"",{0}{1}ValueType1DoesSomethingElseWith = {14},{0}{1}MalePerson = {15},{0}{1}FemalePerson = {16},{0}{1}ChildPerson = {17},{0}{1}Death = {18}{0}}}", Environment.NewLine, "", Me.FirstName, Me.Date_YMD, Me.LastName, Me.OptionalUniqueString, Me.HatType_ColorARGB, Me.HatType_HatTypeStyle_HatTypeStyle_Description, Me.OwnsCar_vin, Me.Gender_Gender_Code, Me.PersonHasParents, Me.OptionalUniqueDecimal, Me.MandatoryUniqueDecimal, Me.MandatoryUniqueString, "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...")
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
	<DataObjectAttribute()> _
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
	Public MustInherit Partial Class MalePerson
		Implements INotifyPropertyChanged
		Implements IHasSampleModelContext
		Protected Sub New()
		End Sub
		Private _events As System.Delegate()
		Private ReadOnly Property Events() As System.Delegate()
			Get
				If CObj(Me._events) Is Nothing Then
					Me._events = New System.Delegate(1) {}
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
		Public MustOverride ReadOnly Property Context() As SampleModelContext Implements _
			IHasSampleModelContext.Context
		Public Custom Event PersonChanging As EventHandler(Of PropertyChangingEventArgs(Of MalePerson, Person))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Combine(Me.Events(0), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Remove(Me.Events(0), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaisePersonChangingEvent(ByVal newValue As Person) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of MalePerson, Person)) = TryCast(Me.Events(0), EventHandler(Of PropertyChangingEventArgs(Of MalePerson, Person)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of MalePerson, Person) = New PropertyChangingEventArgs(Of MalePerson, Person)(Me, Me.Person, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event PersonChanged As EventHandler(Of PropertyChangedEventArgs(Of MalePerson, Person))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Combine(Me.Events(0), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Remove(Me.Events(0), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaisePersonChangedEvent(ByVal oldValue As Person)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of MalePerson, Person)) = TryCast(Me.Events(0), EventHandler(Of PropertyChangedEventArgs(Of MalePerson, Person)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of MalePerson, Person)(Me, oldValue, Me.Person), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Person")
			End If
		End Sub
		<DataObjectFieldAttribute(False, False, False)> _
		Public MustOverride Property Person() As Person
		<DataObjectFieldAttribute(False, False, True)> _
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
		Public Custom Event FirstNameChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.FirstNameChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.FirstNameChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event FirstNameChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
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
		Public Custom Event Date_YMDChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Integer))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.Date_YMDChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.Date_YMDChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event Date_YMDChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Integer))
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
		Public Custom Event LastNameChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.LastNameChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.LastNameChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event LastNameChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.LastNameChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.LastNameChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property OptionalUniqueString() As String
			Get
				Return Me.Person.OptionalUniqueString
			End Get
			Set(ByVal Value As String)
				Me.Person.OptionalUniqueString = Value
			End Set
		End Property
		Public Custom Event OptionalUniqueStringChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.OptionalUniqueStringChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.OptionalUniqueStringChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event OptionalUniqueStringChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.OptionalUniqueStringChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.OptionalUniqueStringChanged, Value
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
		Public Custom Event HatType_ColorARGBChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Nullable(Of Integer)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.HatType_ColorARGBChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.HatType_ColorARGBChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event HatType_ColorARGBChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Nullable(Of Integer)))
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
		Public Custom Event HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
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
		Public Custom Event OwnsCar_vinChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Nullable(Of Integer)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.OwnsCar_vinChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.OwnsCar_vinChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event OwnsCar_vinChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Nullable(Of Integer)))
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
		Public Custom Event Gender_Gender_CodeChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.Gender_Gender_CodeChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.Gender_Gender_CodeChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event Gender_Gender_CodeChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
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
		Public Custom Event PersonHasParentsChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Nullable(Of Boolean)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.PersonHasParentsChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.PersonHasParentsChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event PersonHasParentsChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Nullable(Of Boolean)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.PersonHasParentsChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.PersonHasParentsChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property OptionalUniqueDecimal() As Nullable(Of Decimal)
			Get
				Return Me.Person.OptionalUniqueDecimal
			End Get
			Set(ByVal Value As Nullable(Of Decimal))
				Me.Person.OptionalUniqueDecimal = Value
			End Set
		End Property
		Public Custom Event OptionalUniqueDecimalChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Nullable(Of Decimal)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.OptionalUniqueDecimalChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.OptionalUniqueDecimalChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event OptionalUniqueDecimalChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Nullable(Of Decimal)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.OptionalUniqueDecimalChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.OptionalUniqueDecimalChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property MandatoryUniqueDecimal() As Decimal
			Get
				Return Me.Person.MandatoryUniqueDecimal
			End Get
			Set(ByVal Value As Decimal)
				Me.Person.MandatoryUniqueDecimal = Value
			End Set
		End Property
		Public Custom Event MandatoryUniqueDecimalChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Decimal))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.MandatoryUniqueDecimalChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.MandatoryUniqueDecimalChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event MandatoryUniqueDecimalChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Decimal))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.MandatoryUniqueDecimalChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.MandatoryUniqueDecimalChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property MandatoryUniqueString() As String
			Get
				Return Me.Person.MandatoryUniqueString
			End Get
			Set(ByVal Value As String)
				Me.Person.MandatoryUniqueString = Value
			End Set
		End Property
		Public Custom Event MandatoryUniqueStringChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.MandatoryUniqueStringChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.MandatoryUniqueStringChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event MandatoryUniqueStringChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.MandatoryUniqueStringChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.MandatoryUniqueStringChanged, Value
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
		Public Custom Event ValueType1DoesSomethingElseWithChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, ValueType1))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.ValueType1DoesSomethingElseWithChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.ValueType1DoesSomethingElseWithChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event ValueType1DoesSomethingElseWithChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, ValueType1))
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
		Public Custom Event FemalePersonChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, FemalePerson))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.FemalePersonChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.FemalePersonChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event FemalePersonChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, FemalePerson))
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
		Public Custom Event DeathChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Death))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.DeathChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.DeathChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event DeathChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Death))
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
	<DataObjectAttribute()> _
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
	Public MustInherit Partial Class FemalePerson
		Implements INotifyPropertyChanged
		Implements IHasSampleModelContext
		Protected Sub New()
		End Sub
		Private _events As System.Delegate()
		Private ReadOnly Property Events() As System.Delegate()
			Get
				If CObj(Me._events) Is Nothing Then
					Me._events = New System.Delegate(1) {}
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
		Public MustOverride ReadOnly Property Context() As SampleModelContext Implements _
			IHasSampleModelContext.Context
		Public Custom Event PersonChanging As EventHandler(Of PropertyChangingEventArgs(Of FemalePerson, Person))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Combine(Me.Events(0), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Remove(Me.Events(0), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaisePersonChangingEvent(ByVal newValue As Person) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of FemalePerson, Person)) = TryCast(Me.Events(0), EventHandler(Of PropertyChangingEventArgs(Of FemalePerson, Person)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of FemalePerson, Person) = New PropertyChangingEventArgs(Of FemalePerson, Person)(Me, Me.Person, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event PersonChanged As EventHandler(Of PropertyChangedEventArgs(Of FemalePerson, Person))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Combine(Me.Events(0), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Remove(Me.Events(0), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaisePersonChangedEvent(ByVal oldValue As Person)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of FemalePerson, Person)) = TryCast(Me.Events(0), EventHandler(Of PropertyChangedEventArgs(Of FemalePerson, Person)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of FemalePerson, Person)(Me, oldValue, Me.Person), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Person")
			End If
		End Sub
		<DataObjectFieldAttribute(False, False, False)> _
		Public MustOverride Property Person() As Person
		<DataObjectFieldAttribute(False, False, True)> _
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
		Public Custom Event FirstNameChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.FirstNameChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.FirstNameChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event FirstNameChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
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
		Public Custom Event Date_YMDChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Integer))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.Date_YMDChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.Date_YMDChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event Date_YMDChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Integer))
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
		Public Custom Event LastNameChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.LastNameChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.LastNameChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event LastNameChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.LastNameChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.LastNameChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property OptionalUniqueString() As String
			Get
				Return Me.Person.OptionalUniqueString
			End Get
			Set(ByVal Value As String)
				Me.Person.OptionalUniqueString = Value
			End Set
		End Property
		Public Custom Event OptionalUniqueStringChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.OptionalUniqueStringChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.OptionalUniqueStringChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event OptionalUniqueStringChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.OptionalUniqueStringChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.OptionalUniqueStringChanged, Value
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
		Public Custom Event HatType_ColorARGBChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Nullable(Of Integer)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.HatType_ColorARGBChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.HatType_ColorARGBChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event HatType_ColorARGBChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Nullable(Of Integer)))
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
		Public Custom Event HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
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
		Public Custom Event OwnsCar_vinChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Nullable(Of Integer)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.OwnsCar_vinChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.OwnsCar_vinChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event OwnsCar_vinChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Nullable(Of Integer)))
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
		Public Custom Event Gender_Gender_CodeChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.Gender_Gender_CodeChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.Gender_Gender_CodeChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event Gender_Gender_CodeChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
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
		Public Custom Event PersonHasParentsChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Nullable(Of Boolean)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.PersonHasParentsChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.PersonHasParentsChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event PersonHasParentsChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Nullable(Of Boolean)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.PersonHasParentsChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.PersonHasParentsChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property OptionalUniqueDecimal() As Nullable(Of Decimal)
			Get
				Return Me.Person.OptionalUniqueDecimal
			End Get
			Set(ByVal Value As Nullable(Of Decimal))
				Me.Person.OptionalUniqueDecimal = Value
			End Set
		End Property
		Public Custom Event OptionalUniqueDecimalChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Nullable(Of Decimal)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.OptionalUniqueDecimalChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.OptionalUniqueDecimalChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event OptionalUniqueDecimalChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Nullable(Of Decimal)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.OptionalUniqueDecimalChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.OptionalUniqueDecimalChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property MandatoryUniqueDecimal() As Decimal
			Get
				Return Me.Person.MandatoryUniqueDecimal
			End Get
			Set(ByVal Value As Decimal)
				Me.Person.MandatoryUniqueDecimal = Value
			End Set
		End Property
		Public Custom Event MandatoryUniqueDecimalChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Decimal))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.MandatoryUniqueDecimalChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.MandatoryUniqueDecimalChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event MandatoryUniqueDecimalChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Decimal))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.MandatoryUniqueDecimalChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.MandatoryUniqueDecimalChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property MandatoryUniqueString() As String
			Get
				Return Me.Person.MandatoryUniqueString
			End Get
			Set(ByVal Value As String)
				Me.Person.MandatoryUniqueString = Value
			End Set
		End Property
		Public Custom Event MandatoryUniqueStringChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.MandatoryUniqueStringChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.MandatoryUniqueStringChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event MandatoryUniqueStringChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.MandatoryUniqueStringChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.MandatoryUniqueStringChanged, Value
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
		Public Custom Event ValueType1DoesSomethingElseWithChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, ValueType1))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.ValueType1DoesSomethingElseWithChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.ValueType1DoesSomethingElseWithChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event ValueType1DoesSomethingElseWithChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, ValueType1))
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
		Public Custom Event MalePersonChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, MalePerson))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.MalePersonChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.MalePersonChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event MalePersonChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, MalePerson))
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
		Public Custom Event DeathChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Death))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.DeathChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.DeathChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event DeathChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Death))
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
	<DataObjectAttribute()> _
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
	Public MustInherit Partial Class ChildPerson
		Implements INotifyPropertyChanged
		Implements IHasSampleModelContext
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
		Public MustOverride ReadOnly Property Context() As SampleModelContext Implements _
			IHasSampleModelContext.Context
		Public Custom Event BirthOrder_BirthOrder_NrChanging As EventHandler(Of PropertyChangingEventArgs(Of ChildPerson, Integer))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Combine(Me.Events(0), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Remove(Me.Events(0), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseBirthOrder_BirthOrder_NrChangingEvent(ByVal newValue As Integer) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of ChildPerson, Integer)) = TryCast(Me.Events(0), EventHandler(Of PropertyChangingEventArgs(Of ChildPerson, Integer)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of ChildPerson, Integer) = New PropertyChangingEventArgs(Of ChildPerson, Integer)(Me, Me.BirthOrder_BirthOrder_Nr, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event BirthOrder_BirthOrder_NrChanged As EventHandler(Of PropertyChangedEventArgs(Of ChildPerson, Integer))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Combine(Me.Events(0), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Remove(Me.Events(0), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseBirthOrder_BirthOrder_NrChangedEvent(ByVal oldValue As Integer)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of ChildPerson, Integer)) = TryCast(Me.Events(0), EventHandler(Of PropertyChangedEventArgs(Of ChildPerson, Integer)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of ChildPerson, Integer)(Me, oldValue, Me.BirthOrder_BirthOrder_Nr), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("BirthOrder_BirthOrder_Nr")
			End If
		End Sub
		Public Custom Event FatherChanging As EventHandler(Of PropertyChangingEventArgs(Of ChildPerson, MalePerson))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseFatherChangingEvent(ByVal newValue As MalePerson) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of ChildPerson, MalePerson)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangingEventArgs(Of ChildPerson, MalePerson)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of ChildPerson, MalePerson) = New PropertyChangingEventArgs(Of ChildPerson, MalePerson)(Me, Me.Father, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event FatherChanged As EventHandler(Of PropertyChangedEventArgs(Of ChildPerson, MalePerson))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseFatherChangedEvent(ByVal oldValue As MalePerson)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of ChildPerson, MalePerson)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangedEventArgs(Of ChildPerson, MalePerson)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of ChildPerson, MalePerson)(Me, oldValue, Me.Father), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Father")
			End If
		End Sub
		Public Custom Event MotherChanging As EventHandler(Of PropertyChangingEventArgs(Of ChildPerson, FemalePerson))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseMotherChangingEvent(ByVal newValue As FemalePerson) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of ChildPerson, FemalePerson)) = TryCast(Me.Events(2), EventHandler(Of PropertyChangingEventArgs(Of ChildPerson, FemalePerson)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of ChildPerson, FemalePerson) = New PropertyChangingEventArgs(Of ChildPerson, FemalePerson)(Me, Me.Mother, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event MotherChanged As EventHandler(Of PropertyChangedEventArgs(Of ChildPerson, FemalePerson))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseMotherChangedEvent(ByVal oldValue As FemalePerson)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of ChildPerson, FemalePerson)) = TryCast(Me.Events(2), EventHandler(Of PropertyChangedEventArgs(Of ChildPerson, FemalePerson)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of ChildPerson, FemalePerson)(Me, oldValue, Me.Mother), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Mother")
			End If
		End Sub
		Public Custom Event PersonChanging As EventHandler(Of PropertyChangingEventArgs(Of ChildPerson, Person))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Combine(Me.Events(3), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Remove(Me.Events(3), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaisePersonChangingEvent(ByVal newValue As Person) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of ChildPerson, Person)) = TryCast(Me.Events(3), EventHandler(Of PropertyChangingEventArgs(Of ChildPerson, Person)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of ChildPerson, Person) = New PropertyChangingEventArgs(Of ChildPerson, Person)(Me, Me.Person, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event PersonChanged As EventHandler(Of PropertyChangedEventArgs(Of ChildPerson, Person))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Combine(Me.Events(3), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Remove(Me.Events(3), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaisePersonChangedEvent(ByVal oldValue As Person)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of ChildPerson, Person)) = TryCast(Me.Events(3), EventHandler(Of PropertyChangedEventArgs(Of ChildPerson, Person)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of ChildPerson, Person)(Me, oldValue, Me.Person), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Person")
			End If
		End Sub
		<DataObjectFieldAttribute(False, False, False)> _
		Public MustOverride Property BirthOrder_BirthOrder_Nr() As Integer
		<DataObjectFieldAttribute(False, False, False)> _
		Public MustOverride Property Father() As MalePerson
		<DataObjectFieldAttribute(False, False, False)> _
		Public MustOverride Property Mother() As FemalePerson
		<DataObjectFieldAttribute(False, False, False)> _
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
		Public Custom Event FirstNameChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.FirstNameChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.FirstNameChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event FirstNameChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
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
		Public Custom Event Date_YMDChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Integer))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.Date_YMDChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.Date_YMDChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event Date_YMDChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Integer))
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
		Public Custom Event LastNameChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.LastNameChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.LastNameChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event LastNameChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.LastNameChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.LastNameChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property OptionalUniqueString() As String
			Get
				Return Me.Person.OptionalUniqueString
			End Get
			Set(ByVal Value As String)
				Me.Person.OptionalUniqueString = Value
			End Set
		End Property
		Public Custom Event OptionalUniqueStringChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.OptionalUniqueStringChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.OptionalUniqueStringChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event OptionalUniqueStringChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.OptionalUniqueStringChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.OptionalUniqueStringChanged, Value
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
		Public Custom Event HatType_ColorARGBChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Nullable(Of Integer)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.HatType_ColorARGBChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.HatType_ColorARGBChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event HatType_ColorARGBChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Nullable(Of Integer)))
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
		Public Custom Event HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
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
		Public Custom Event OwnsCar_vinChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Nullable(Of Integer)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.OwnsCar_vinChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.OwnsCar_vinChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event OwnsCar_vinChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Nullable(Of Integer)))
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
		Public Custom Event Gender_Gender_CodeChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.Gender_Gender_CodeChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.Gender_Gender_CodeChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event Gender_Gender_CodeChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
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
		Public Custom Event PersonHasParentsChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Nullable(Of Boolean)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.PersonHasParentsChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.PersonHasParentsChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event PersonHasParentsChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Nullable(Of Boolean)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.PersonHasParentsChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.PersonHasParentsChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property OptionalUniqueDecimal() As Nullable(Of Decimal)
			Get
				Return Me.Person.OptionalUniqueDecimal
			End Get
			Set(ByVal Value As Nullable(Of Decimal))
				Me.Person.OptionalUniqueDecimal = Value
			End Set
		End Property
		Public Custom Event OptionalUniqueDecimalChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Nullable(Of Decimal)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.OptionalUniqueDecimalChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.OptionalUniqueDecimalChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event OptionalUniqueDecimalChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Nullable(Of Decimal)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.OptionalUniqueDecimalChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.OptionalUniqueDecimalChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property MandatoryUniqueDecimal() As Decimal
			Get
				Return Me.Person.MandatoryUniqueDecimal
			End Get
			Set(ByVal Value As Decimal)
				Me.Person.MandatoryUniqueDecimal = Value
			End Set
		End Property
		Public Custom Event MandatoryUniqueDecimalChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Decimal))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.MandatoryUniqueDecimalChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.MandatoryUniqueDecimalChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event MandatoryUniqueDecimalChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Decimal))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.MandatoryUniqueDecimalChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.MandatoryUniqueDecimalChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property MandatoryUniqueString() As String
			Get
				Return Me.Person.MandatoryUniqueString
			End Get
			Set(ByVal Value As String)
				Me.Person.MandatoryUniqueString = Value
			End Set
		End Property
		Public Custom Event MandatoryUniqueStringChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.MandatoryUniqueStringChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.MandatoryUniqueStringChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event MandatoryUniqueStringChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.MandatoryUniqueStringChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.MandatoryUniqueStringChanged, Value
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
		Public Custom Event ValueType1DoesSomethingElseWithChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, ValueType1))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.ValueType1DoesSomethingElseWithChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.ValueType1DoesSomethingElseWithChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event ValueType1DoesSomethingElseWithChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, ValueType1))
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
		Public Custom Event MalePersonChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, MalePerson))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.MalePersonChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.MalePersonChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event MalePersonChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, MalePerson))
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
		Public Custom Event FemalePersonChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, FemalePerson))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.FemalePersonChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.FemalePersonChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event FemalePersonChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, FemalePerson))
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
		Public Custom Event DeathChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Death))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.DeathChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.DeathChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event DeathChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Death))
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
	<DataObjectAttribute()> _
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
	Public MustInherit Partial Class Death
		Implements INotifyPropertyChanged
		Implements IHasSampleModelContext
		Protected Sub New()
		End Sub
		Private _events As System.Delegate()
		Private ReadOnly Property Events() As System.Delegate()
			Get
				If CObj(Me._events) Is Nothing Then
					Me._events = New System.Delegate(5) {}
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
		Public MustOverride ReadOnly Property Context() As SampleModelContext Implements _
			IHasSampleModelContext.Context
		Public Custom Event Date_YMDChanging As EventHandler(Of PropertyChangingEventArgs(Of Death, Nullable(Of Integer)))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Combine(Me.Events(0), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Remove(Me.Events(0), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseDate_YMDChangingEvent(ByVal newValue As Nullable(Of Integer)) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Death, Nullable(Of Integer))) = TryCast(Me.Events(0), EventHandler(Of PropertyChangingEventArgs(Of Death, Nullable(Of Integer))))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Death, Nullable(Of Integer)) = New PropertyChangingEventArgs(Of Death, Nullable(Of Integer))(Me, Me.Date_YMD, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event Date_YMDChanged As EventHandler(Of PropertyChangedEventArgs(Of Death, Nullable(Of Integer)))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Combine(Me.Events(0), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Remove(Me.Events(0), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseDate_YMDChangedEvent(ByVal oldValue As Nullable(Of Integer))
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Death, Nullable(Of Integer))) = TryCast(Me.Events(0), EventHandler(Of PropertyChangedEventArgs(Of Death, Nullable(Of Integer))))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Death, Nullable(Of Integer))(Me, oldValue, Me.Date_YMD), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Date_YMD")
			End If
		End Sub
		Public Custom Event DeathCause_DeathCause_TypeChanging As EventHandler(Of PropertyChangingEventArgs(Of Death, String))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseDeathCause_DeathCause_TypeChangingEvent(ByVal newValue As String) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Death, String)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangingEventArgs(Of Death, String)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Death, String) = New PropertyChangingEventArgs(Of Death, String)(Me, Me.DeathCause_DeathCause_Type, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event DeathCause_DeathCause_TypeChanged As EventHandler(Of PropertyChangedEventArgs(Of Death, String))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseDeathCause_DeathCause_TypeChangedEvent(ByVal oldValue As String)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Death, String)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangedEventArgs(Of Death, String)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Death, String)(Me, oldValue, Me.DeathCause_DeathCause_Type), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("DeathCause_DeathCause_Type")
			End If
		End Sub
		Public Custom Event NaturalDeathChanging As EventHandler(Of PropertyChangingEventArgs(Of Death, NaturalDeath))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseNaturalDeathChangingEvent(ByVal newValue As NaturalDeath) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Death, NaturalDeath)) = TryCast(Me.Events(2), EventHandler(Of PropertyChangingEventArgs(Of Death, NaturalDeath)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Death, NaturalDeath) = New PropertyChangingEventArgs(Of Death, NaturalDeath)(Me, Me.NaturalDeath, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event NaturalDeathChanged As EventHandler(Of PropertyChangedEventArgs(Of Death, NaturalDeath))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseNaturalDeathChangedEvent(ByVal oldValue As NaturalDeath)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Death, NaturalDeath)) = TryCast(Me.Events(2), EventHandler(Of PropertyChangedEventArgs(Of Death, NaturalDeath)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Death, NaturalDeath)(Me, oldValue, Me.NaturalDeath), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("NaturalDeath")
			End If
		End Sub
		Public Custom Event UnnaturalDeathChanging As EventHandler(Of PropertyChangingEventArgs(Of Death, UnnaturalDeath))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Combine(Me.Events(3), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Remove(Me.Events(3), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseUnnaturalDeathChangingEvent(ByVal newValue As UnnaturalDeath) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Death, UnnaturalDeath)) = TryCast(Me.Events(3), EventHandler(Of PropertyChangingEventArgs(Of Death, UnnaturalDeath)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Death, UnnaturalDeath) = New PropertyChangingEventArgs(Of Death, UnnaturalDeath)(Me, Me.UnnaturalDeath, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event UnnaturalDeathChanged As EventHandler(Of PropertyChangedEventArgs(Of Death, UnnaturalDeath))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Combine(Me.Events(3), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(3) = System.Delegate.Remove(Me.Events(3), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseUnnaturalDeathChangedEvent(ByVal oldValue As UnnaturalDeath)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Death, UnnaturalDeath)) = TryCast(Me.Events(3), EventHandler(Of PropertyChangedEventArgs(Of Death, UnnaturalDeath)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Death, UnnaturalDeath)(Me, oldValue, Me.UnnaturalDeath), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("UnnaturalDeath")
			End If
		End Sub
		Public Custom Event PersonChanging As EventHandler(Of PropertyChangingEventArgs(Of Death, Person))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(4) = System.Delegate.Combine(Me.Events(4), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(4) = System.Delegate.Remove(Me.Events(4), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaisePersonChangingEvent(ByVal newValue As Person) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Death, Person)) = TryCast(Me.Events(4), EventHandler(Of PropertyChangingEventArgs(Of Death, Person)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Death, Person) = New PropertyChangingEventArgs(Of Death, Person)(Me, Me.Person, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event PersonChanged As EventHandler(Of PropertyChangedEventArgs(Of Death, Person))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(4) = System.Delegate.Combine(Me.Events(4), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(4) = System.Delegate.Remove(Me.Events(4), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaisePersonChangedEvent(ByVal oldValue As Person)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Death, Person)) = TryCast(Me.Events(4), EventHandler(Of PropertyChangedEventArgs(Of Death, Person)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Death, Person)(Me, oldValue, Me.Person), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Person")
			End If
		End Sub
		<DataObjectFieldAttribute(False, False, True)> _
		Public MustOverride Property Date_YMD() As Nullable(Of Integer)
		<DataObjectFieldAttribute(False, False, False)> _
		Public MustOverride Property DeathCause_DeathCause_Type() As String
		<DataObjectFieldAttribute(False, False, True)> _
		Public MustOverride Property NaturalDeath() As NaturalDeath
		<DataObjectFieldAttribute(False, False, True)> _
		Public MustOverride Property UnnaturalDeath() As UnnaturalDeath
		<DataObjectFieldAttribute(False, False, False)> _
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
		Public Custom Event FirstNameChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.FirstNameChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.FirstNameChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event FirstNameChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
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
		Public Custom Event LastNameChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.LastNameChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.LastNameChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event LastNameChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.LastNameChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.LastNameChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property OptionalUniqueString() As String
			Get
				Return Me.Person.OptionalUniqueString
			End Get
			Set(ByVal Value As String)
				Me.Person.OptionalUniqueString = Value
			End Set
		End Property
		Public Custom Event OptionalUniqueStringChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.OptionalUniqueStringChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.OptionalUniqueStringChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event OptionalUniqueStringChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.OptionalUniqueStringChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.OptionalUniqueStringChanged, Value
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
		Public Custom Event HatType_ColorARGBChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Nullable(Of Integer)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.HatType_ColorARGBChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.HatType_ColorARGBChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event HatType_ColorARGBChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Nullable(Of Integer)))
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
		Public Custom Event HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
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
		Public Custom Event OwnsCar_vinChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Nullable(Of Integer)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.OwnsCar_vinChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.OwnsCar_vinChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event OwnsCar_vinChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Nullable(Of Integer)))
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
		Public Custom Event Gender_Gender_CodeChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.Gender_Gender_CodeChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.Gender_Gender_CodeChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event Gender_Gender_CodeChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
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
		Public Custom Event PersonHasParentsChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Nullable(Of Boolean)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.PersonHasParentsChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.PersonHasParentsChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event PersonHasParentsChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Nullable(Of Boolean)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.PersonHasParentsChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.PersonHasParentsChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property OptionalUniqueDecimal() As Nullable(Of Decimal)
			Get
				Return Me.Person.OptionalUniqueDecimal
			End Get
			Set(ByVal Value As Nullable(Of Decimal))
				Me.Person.OptionalUniqueDecimal = Value
			End Set
		End Property
		Public Custom Event OptionalUniqueDecimalChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Nullable(Of Decimal)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.OptionalUniqueDecimalChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.OptionalUniqueDecimalChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event OptionalUniqueDecimalChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Nullable(Of Decimal)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.OptionalUniqueDecimalChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.OptionalUniqueDecimalChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property MandatoryUniqueDecimal() As Decimal
			Get
				Return Me.Person.MandatoryUniqueDecimal
			End Get
			Set(ByVal Value As Decimal)
				Me.Person.MandatoryUniqueDecimal = Value
			End Set
		End Property
		Public Custom Event MandatoryUniqueDecimalChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Decimal))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.MandatoryUniqueDecimalChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.MandatoryUniqueDecimalChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event MandatoryUniqueDecimalChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Decimal))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.MandatoryUniqueDecimalChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.MandatoryUniqueDecimalChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property MandatoryUniqueString() As String
			Get
				Return Me.Person.MandatoryUniqueString
			End Get
			Set(ByVal Value As String)
				Me.Person.MandatoryUniqueString = Value
			End Set
		End Property
		Public Custom Event MandatoryUniqueStringChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.MandatoryUniqueStringChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.MandatoryUniqueStringChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event MandatoryUniqueStringChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.MandatoryUniqueStringChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.MandatoryUniqueStringChanged, Value
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
		Public Custom Event ValueType1DoesSomethingElseWithChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, ValueType1))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.ValueType1DoesSomethingElseWithChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.ValueType1DoesSomethingElseWithChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event ValueType1DoesSomethingElseWithChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, ValueType1))
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
		Public Custom Event MalePersonChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, MalePerson))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.MalePersonChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.MalePersonChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event MalePersonChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, MalePerson))
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
		Public Custom Event FemalePersonChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, FemalePerson))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.FemalePersonChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.FemalePersonChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event FemalePersonChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, FemalePerson))
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
		Public Custom Event ChildPersonChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, ChildPerson))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Person.ChildPersonChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Person.ChildPersonChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event ChildPersonChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, ChildPerson))
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
	<DataObjectAttribute()> _
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
	Public MustInherit Partial Class NaturalDeath
		Implements INotifyPropertyChanged
		Implements IHasSampleModelContext
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
		Public MustOverride ReadOnly Property Context() As SampleModelContext Implements _
			IHasSampleModelContext.Context
		Public Custom Event NaturalDeathIsFromProstateCancerChanging As EventHandler(Of PropertyChangingEventArgs(Of NaturalDeath, Nullable(Of Boolean)))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Combine(Me.Events(0), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Remove(Me.Events(0), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseNaturalDeathIsFromProstateCancerChangingEvent(ByVal newValue As Nullable(Of Boolean)) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of NaturalDeath, Nullable(Of Boolean))) = TryCast(Me.Events(0), EventHandler(Of PropertyChangingEventArgs(Of NaturalDeath, Nullable(Of Boolean))))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of NaturalDeath, Nullable(Of Boolean)) = New PropertyChangingEventArgs(Of NaturalDeath, Nullable(Of Boolean))(Me, Me.NaturalDeathIsFromProstateCancer, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event NaturalDeathIsFromProstateCancerChanged As EventHandler(Of PropertyChangedEventArgs(Of NaturalDeath, Nullable(Of Boolean)))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Combine(Me.Events(0), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Remove(Me.Events(0), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseNaturalDeathIsFromProstateCancerChangedEvent(ByVal oldValue As Nullable(Of Boolean))
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of NaturalDeath, Nullable(Of Boolean))) = TryCast(Me.Events(0), EventHandler(Of PropertyChangedEventArgs(Of NaturalDeath, Nullable(Of Boolean))))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of NaturalDeath, Nullable(Of Boolean))(Me, oldValue, Me.NaturalDeathIsFromProstateCancer), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("NaturalDeathIsFromProstateCancer")
			End If
		End Sub
		Public Custom Event DeathChanging As EventHandler(Of PropertyChangingEventArgs(Of NaturalDeath, Death))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseDeathChangingEvent(ByVal newValue As Death) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of NaturalDeath, Death)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangingEventArgs(Of NaturalDeath, Death)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of NaturalDeath, Death) = New PropertyChangingEventArgs(Of NaturalDeath, Death)(Me, Me.Death, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event DeathChanged As EventHandler(Of PropertyChangedEventArgs(Of NaturalDeath, Death))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseDeathChangedEvent(ByVal oldValue As Death)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of NaturalDeath, Death)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangedEventArgs(Of NaturalDeath, Death)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of NaturalDeath, Death)(Me, oldValue, Me.Death), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Death")
			End If
		End Sub
		<DataObjectFieldAttribute(False, False, True)> _
		Public MustOverride Property NaturalDeathIsFromProstateCancer() As Nullable(Of Boolean)
		<DataObjectFieldAttribute(False, False, False)> _
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
		Public Custom Event Date_YMDChanging As EventHandler(Of PropertyChangingEventArgs(Of Death, Nullable(Of Integer)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Date_YMDChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Date_YMDChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event Date_YMDChanged As EventHandler(Of PropertyChangedEventArgs(Of Death, Nullable(Of Integer)))
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
		Public Custom Event DeathCause_DeathCause_TypeChanging As EventHandler(Of PropertyChangingEventArgs(Of Death, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.DeathCause_DeathCause_TypeChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.DeathCause_DeathCause_TypeChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event DeathCause_DeathCause_TypeChanged As EventHandler(Of PropertyChangedEventArgs(Of Death, String))
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
		Public Custom Event UnnaturalDeathChanging As EventHandler(Of PropertyChangingEventArgs(Of Death, UnnaturalDeath))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.UnnaturalDeathChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.UnnaturalDeathChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event UnnaturalDeathChanged As EventHandler(Of PropertyChangedEventArgs(Of Death, UnnaturalDeath))
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
		Public Custom Event PersonChanging As EventHandler(Of PropertyChangingEventArgs(Of Death, Person))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.PersonChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.PersonChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event PersonChanged As EventHandler(Of PropertyChangedEventArgs(Of Death, Person))
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
		Public Custom Event FirstNameChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.FirstNameChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.FirstNameChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event FirstNameChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
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
		Public Custom Event LastNameChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.LastNameChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.LastNameChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event LastNameChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.LastNameChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.LastNameChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property OptionalUniqueString() As String
			Get
				Return Me.Death.Person.OptionalUniqueString
			End Get
			Set(ByVal Value As String)
				Me.Death.Person.OptionalUniqueString = Value
			End Set
		End Property
		Public Custom Event OptionalUniqueStringChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.OptionalUniqueStringChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.OptionalUniqueStringChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event OptionalUniqueStringChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.OptionalUniqueStringChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.OptionalUniqueStringChanged, Value
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
		Public Custom Event HatType_ColorARGBChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Nullable(Of Integer)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.HatType_ColorARGBChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.HatType_ColorARGBChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event HatType_ColorARGBChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Nullable(Of Integer)))
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
		Public Custom Event HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
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
		Public Custom Event OwnsCar_vinChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Nullable(Of Integer)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.OwnsCar_vinChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.OwnsCar_vinChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event OwnsCar_vinChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Nullable(Of Integer)))
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
		Public Custom Event Gender_Gender_CodeChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.Gender_Gender_CodeChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.Gender_Gender_CodeChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event Gender_Gender_CodeChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
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
		Public Custom Event PersonHasParentsChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Nullable(Of Boolean)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.PersonHasParentsChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.PersonHasParentsChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event PersonHasParentsChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Nullable(Of Boolean)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.PersonHasParentsChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.PersonHasParentsChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property OptionalUniqueDecimal() As Nullable(Of Decimal)
			Get
				Return Me.Death.Person.OptionalUniqueDecimal
			End Get
			Set(ByVal Value As Nullable(Of Decimal))
				Me.Death.Person.OptionalUniqueDecimal = Value
			End Set
		End Property
		Public Custom Event OptionalUniqueDecimalChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Nullable(Of Decimal)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.OptionalUniqueDecimalChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.OptionalUniqueDecimalChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event OptionalUniqueDecimalChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Nullable(Of Decimal)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.OptionalUniqueDecimalChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.OptionalUniqueDecimalChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property MandatoryUniqueDecimal() As Decimal
			Get
				Return Me.Death.Person.MandatoryUniqueDecimal
			End Get
			Set(ByVal Value As Decimal)
				Me.Death.Person.MandatoryUniqueDecimal = Value
			End Set
		End Property
		Public Custom Event MandatoryUniqueDecimalChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Decimal))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.MandatoryUniqueDecimalChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.MandatoryUniqueDecimalChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event MandatoryUniqueDecimalChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Decimal))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.MandatoryUniqueDecimalChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.MandatoryUniqueDecimalChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property MandatoryUniqueString() As String
			Get
				Return Me.Death.Person.MandatoryUniqueString
			End Get
			Set(ByVal Value As String)
				Me.Death.Person.MandatoryUniqueString = Value
			End Set
		End Property
		Public Custom Event MandatoryUniqueStringChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.MandatoryUniqueStringChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.MandatoryUniqueStringChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event MandatoryUniqueStringChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.MandatoryUniqueStringChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.MandatoryUniqueStringChanged, Value
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
		Public Custom Event ValueType1DoesSomethingElseWithChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, ValueType1))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.ValueType1DoesSomethingElseWithChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.ValueType1DoesSomethingElseWithChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event ValueType1DoesSomethingElseWithChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, ValueType1))
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
		Public Custom Event MalePersonChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, MalePerson))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.MalePersonChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.MalePersonChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event MalePersonChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, MalePerson))
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
		Public Custom Event FemalePersonChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, FemalePerson))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.FemalePersonChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.FemalePersonChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event FemalePersonChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, FemalePerson))
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
		Public Custom Event ChildPersonChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, ChildPerson))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.ChildPersonChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.ChildPersonChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event ChildPersonChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, ChildPerson))
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
	<DataObjectAttribute()> _
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
	Public MustInherit Partial Class UnnaturalDeath
		Implements INotifyPropertyChanged
		Implements IHasSampleModelContext
		Protected Sub New()
		End Sub
		Private _events As System.Delegate()
		Private ReadOnly Property Events() As System.Delegate()
			Get
				If CObj(Me._events) Is Nothing Then
					Me._events = New System.Delegate(3) {}
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
		Public MustOverride ReadOnly Property Context() As SampleModelContext Implements _
			IHasSampleModelContext.Context
		Public Custom Event UnnaturalDeathIsViolentChanging As EventHandler(Of PropertyChangingEventArgs(Of UnnaturalDeath, Nullable(Of Boolean)))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Combine(Me.Events(0), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Remove(Me.Events(0), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseUnnaturalDeathIsViolentChangingEvent(ByVal newValue As Nullable(Of Boolean)) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of UnnaturalDeath, Nullable(Of Boolean))) = TryCast(Me.Events(0), EventHandler(Of PropertyChangingEventArgs(Of UnnaturalDeath, Nullable(Of Boolean))))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of UnnaturalDeath, Nullable(Of Boolean)) = New PropertyChangingEventArgs(Of UnnaturalDeath, Nullable(Of Boolean))(Me, Me.UnnaturalDeathIsViolent, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event UnnaturalDeathIsViolentChanged As EventHandler(Of PropertyChangedEventArgs(Of UnnaturalDeath, Nullable(Of Boolean)))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Combine(Me.Events(0), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Remove(Me.Events(0), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseUnnaturalDeathIsViolentChangedEvent(ByVal oldValue As Nullable(Of Boolean))
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of UnnaturalDeath, Nullable(Of Boolean))) = TryCast(Me.Events(0), EventHandler(Of PropertyChangedEventArgs(Of UnnaturalDeath, Nullable(Of Boolean))))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of UnnaturalDeath, Nullable(Of Boolean))(Me, oldValue, Me.UnnaturalDeathIsViolent), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("UnnaturalDeathIsViolent")
			End If
		End Sub
		Public Custom Event UnnaturalDeathIsBloodyChanging As EventHandler(Of PropertyChangingEventArgs(Of UnnaturalDeath, Nullable(Of Boolean)))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseUnnaturalDeathIsBloodyChangingEvent(ByVal newValue As Nullable(Of Boolean)) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of UnnaturalDeath, Nullable(Of Boolean))) = TryCast(Me.Events(1), EventHandler(Of PropertyChangingEventArgs(Of UnnaturalDeath, Nullable(Of Boolean))))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of UnnaturalDeath, Nullable(Of Boolean)) = New PropertyChangingEventArgs(Of UnnaturalDeath, Nullable(Of Boolean))(Me, Me.UnnaturalDeathIsBloody, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event UnnaturalDeathIsBloodyChanged As EventHandler(Of PropertyChangedEventArgs(Of UnnaturalDeath, Nullable(Of Boolean)))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseUnnaturalDeathIsBloodyChangedEvent(ByVal oldValue As Nullable(Of Boolean))
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of UnnaturalDeath, Nullable(Of Boolean))) = TryCast(Me.Events(1), EventHandler(Of PropertyChangedEventArgs(Of UnnaturalDeath, Nullable(Of Boolean))))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of UnnaturalDeath, Nullable(Of Boolean))(Me, oldValue, Me.UnnaturalDeathIsBloody), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("UnnaturalDeathIsBloody")
			End If
		End Sub
		Public Custom Event DeathChanging As EventHandler(Of PropertyChangingEventArgs(Of UnnaturalDeath, Death))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseDeathChangingEvent(ByVal newValue As Death) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of UnnaturalDeath, Death)) = TryCast(Me.Events(2), EventHandler(Of PropertyChangingEventArgs(Of UnnaturalDeath, Death)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of UnnaturalDeath, Death) = New PropertyChangingEventArgs(Of UnnaturalDeath, Death)(Me, Me.Death, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event DeathChanged As EventHandler(Of PropertyChangedEventArgs(Of UnnaturalDeath, Death))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Combine(Me.Events(2), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(2) = System.Delegate.Remove(Me.Events(2), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseDeathChangedEvent(ByVal oldValue As Death)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of UnnaturalDeath, Death)) = TryCast(Me.Events(2), EventHandler(Of PropertyChangedEventArgs(Of UnnaturalDeath, Death)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of UnnaturalDeath, Death)(Me, oldValue, Me.Death), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Death")
			End If
		End Sub
		<DataObjectFieldAttribute(False, False, True)> _
		Public MustOverride Property UnnaturalDeathIsViolent() As Nullable(Of Boolean)
		<DataObjectFieldAttribute(False, False, True)> _
		Public MustOverride Property UnnaturalDeathIsBloody() As Nullable(Of Boolean)
		<DataObjectFieldAttribute(False, False, False)> _
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
		Public Custom Event Date_YMDChanging As EventHandler(Of PropertyChangingEventArgs(Of Death, Nullable(Of Integer)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Date_YMDChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Date_YMDChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event Date_YMDChanged As EventHandler(Of PropertyChangedEventArgs(Of Death, Nullable(Of Integer)))
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
		Public Custom Event DeathCause_DeathCause_TypeChanging As EventHandler(Of PropertyChangingEventArgs(Of Death, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.DeathCause_DeathCause_TypeChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.DeathCause_DeathCause_TypeChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event DeathCause_DeathCause_TypeChanged As EventHandler(Of PropertyChangedEventArgs(Of Death, String))
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
		Public Custom Event NaturalDeathChanging As EventHandler(Of PropertyChangingEventArgs(Of Death, NaturalDeath))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.NaturalDeathChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.NaturalDeathChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event NaturalDeathChanged As EventHandler(Of PropertyChangedEventArgs(Of Death, NaturalDeath))
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
		Public Custom Event PersonChanging As EventHandler(Of PropertyChangingEventArgs(Of Death, Person))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.PersonChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.PersonChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event PersonChanged As EventHandler(Of PropertyChangedEventArgs(Of Death, Person))
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
		Public Custom Event FirstNameChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.FirstNameChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.FirstNameChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event FirstNameChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
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
		Public Custom Event LastNameChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.LastNameChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.LastNameChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event LastNameChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.LastNameChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.LastNameChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property OptionalUniqueString() As String
			Get
				Return Me.Death.Person.OptionalUniqueString
			End Get
			Set(ByVal Value As String)
				Me.Death.Person.OptionalUniqueString = Value
			End Set
		End Property
		Public Custom Event OptionalUniqueStringChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.OptionalUniqueStringChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.OptionalUniqueStringChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event OptionalUniqueStringChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.OptionalUniqueStringChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.OptionalUniqueStringChanged, Value
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
		Public Custom Event HatType_ColorARGBChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Nullable(Of Integer)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.HatType_ColorARGBChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.HatType_ColorARGBChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event HatType_ColorARGBChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Nullable(Of Integer)))
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
		Public Custom Event HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
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
		Public Custom Event OwnsCar_vinChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Nullable(Of Integer)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.OwnsCar_vinChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.OwnsCar_vinChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event OwnsCar_vinChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Nullable(Of Integer)))
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
		Public Custom Event Gender_Gender_CodeChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.Gender_Gender_CodeChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.Gender_Gender_CodeChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event Gender_Gender_CodeChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
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
		Public Custom Event PersonHasParentsChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Nullable(Of Boolean)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.PersonHasParentsChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.PersonHasParentsChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event PersonHasParentsChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Nullable(Of Boolean)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.PersonHasParentsChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.PersonHasParentsChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property OptionalUniqueDecimal() As Nullable(Of Decimal)
			Get
				Return Me.Death.Person.OptionalUniqueDecimal
			End Get
			Set(ByVal Value As Nullable(Of Decimal))
				Me.Death.Person.OptionalUniqueDecimal = Value
			End Set
		End Property
		Public Custom Event OptionalUniqueDecimalChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Nullable(Of Decimal)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.OptionalUniqueDecimalChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.OptionalUniqueDecimalChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event OptionalUniqueDecimalChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Nullable(Of Decimal)))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.OptionalUniqueDecimalChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.OptionalUniqueDecimalChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property MandatoryUniqueDecimal() As Decimal
			Get
				Return Me.Death.Person.MandatoryUniqueDecimal
			End Get
			Set(ByVal Value As Decimal)
				Me.Death.Person.MandatoryUniqueDecimal = Value
			End Set
		End Property
		Public Custom Event MandatoryUniqueDecimalChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, Decimal))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.MandatoryUniqueDecimalChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.MandatoryUniqueDecimalChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event MandatoryUniqueDecimalChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, Decimal))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.MandatoryUniqueDecimalChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.MandatoryUniqueDecimalChanged, Value
			End RemoveHandler
		End Event
		Public Overridable Property MandatoryUniqueString() As String
			Get
				Return Me.Death.Person.MandatoryUniqueString
			End Get
			Set(ByVal Value As String)
				Me.Death.Person.MandatoryUniqueString = Value
			End Set
		End Property
		Public Custom Event MandatoryUniqueStringChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.MandatoryUniqueStringChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.MandatoryUniqueStringChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event MandatoryUniqueStringChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, String))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.MandatoryUniqueStringChanged, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.MandatoryUniqueStringChanged, Value
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
		Public Custom Event ValueType1DoesSomethingElseWithChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, ValueType1))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.ValueType1DoesSomethingElseWithChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.ValueType1DoesSomethingElseWithChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event ValueType1DoesSomethingElseWithChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, ValueType1))
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
		Public Custom Event MalePersonChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, MalePerson))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.MalePersonChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.MalePersonChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event MalePersonChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, MalePerson))
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
		Public Custom Event FemalePersonChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, FemalePerson))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.FemalePersonChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.FemalePersonChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event FemalePersonChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, FemalePerson))
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
		Public Custom Event ChildPersonChanging As EventHandler(Of PropertyChangingEventArgs(Of Person, ChildPerson))
			AddHandler(ByVal Value As EventHandler)
				AddHandler Me.Death.Person.ChildPersonChanging, Value
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				RemoveHandler Me.Death.Person.ChildPersonChanging, Value
			End RemoveHandler
		End Event
		Public Custom Event ChildPersonChanged As EventHandler(Of PropertyChangedEventArgs(Of Person, ChildPerson))
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
	<DataObjectAttribute()> _
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
	Public MustInherit Partial Class Task
		Implements INotifyPropertyChanged
		Implements IHasSampleModelContext
		Protected Sub New()
		End Sub
		Private _events As System.Delegate()
		Private ReadOnly Property Events() As System.Delegate()
			Get
				If CObj(Me._events) Is Nothing Then
					Me._events = New System.Delegate(1) {}
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
		Public MustOverride ReadOnly Property Context() As SampleModelContext Implements _
			IHasSampleModelContext.Context
		Public Custom Event PersonChanging As EventHandler(Of PropertyChangingEventArgs(Of Task, Person))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Combine(Me.Events(0), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Remove(Me.Events(0), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaisePersonChangingEvent(ByVal newValue As Person) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of Task, Person)) = TryCast(Me.Events(0), EventHandler(Of PropertyChangingEventArgs(Of Task, Person)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of Task, Person) = New PropertyChangingEventArgs(Of Task, Person)(Me, Me.Person, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event PersonChanged As EventHandler(Of PropertyChangedEventArgs(Of Task, Person))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Combine(Me.Events(0), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Remove(Me.Events(0), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaisePersonChangedEvent(ByVal oldValue As Person)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of Task, Person)) = TryCast(Me.Events(0), EventHandler(Of PropertyChangedEventArgs(Of Task, Person)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of Task, Person)(Me, oldValue, Me.Person), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("Person")
			End If
		End Sub
		<DataObjectFieldAttribute(False, False, True)> _
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
	<DataObjectAttribute()> _
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
	Public MustInherit Partial Class ValueType1
		Implements INotifyPropertyChanged
		Implements IHasSampleModelContext
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
		Public MustOverride ReadOnly Property Context() As SampleModelContext Implements _
			IHasSampleModelContext.Context
		Public Custom Event ValueType1ValueChanging As EventHandler(Of PropertyChangingEventArgs(Of ValueType1, Integer))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Combine(Me.Events(0), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Remove(Me.Events(0), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseValueType1ValueChangingEvent(ByVal newValue As Integer) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of ValueType1, Integer)) = TryCast(Me.Events(0), EventHandler(Of PropertyChangingEventArgs(Of ValueType1, Integer)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of ValueType1, Integer) = New PropertyChangingEventArgs(Of ValueType1, Integer)(Me, Me.ValueType1Value, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event ValueType1ValueChanged As EventHandler(Of PropertyChangedEventArgs(Of ValueType1, Integer))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Combine(Me.Events(0), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(0) = System.Delegate.Remove(Me.Events(0), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseValueType1ValueChangedEvent(ByVal oldValue As Integer)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of ValueType1, Integer)) = TryCast(Me.Events(0), EventHandler(Of PropertyChangedEventArgs(Of ValueType1, Integer)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of ValueType1, Integer)(Me, oldValue, Me.ValueType1Value), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("ValueType1Value")
			End If
		End Sub
		Public Custom Event DoesSomethingWithPersonChanging As EventHandler(Of PropertyChangingEventArgs(Of ValueType1, Person))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Function RaiseDoesSomethingWithPersonChangingEvent(ByVal newValue As Person) As Boolean
			Dim eventHandler As EventHandler(Of PropertyChangingEventArgs(Of ValueType1, Person)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangingEventArgs(Of ValueType1, Person)))
			If CObj(eventHandler) IsNot Nothing Then
				Dim eventArgs As PropertyChangingEventArgs(Of ValueType1, Person) = New PropertyChangingEventArgs(Of ValueType1, Person)(Me, Me.DoesSomethingWithPerson, newValue)
				eventHandler(Me, eventArgs)
				Return Not (eventArgs.Cancel)
			End If
			Return True
		End Function
		Public Custom Event DoesSomethingWithPersonChanged As EventHandler(Of PropertyChangedEventArgs(Of ValueType1, Person))
			AddHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Combine(Me.Events(1), Value)
			End AddHandler
			RemoveHandler(ByVal Value As EventHandler)
				Me.Events(1) = System.Delegate.Remove(Me.Events(1), Value)
			End RemoveHandler
		End Event
		<SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")> _
		Protected Sub RaiseDoesSomethingWithPersonChangedEvent(ByVal oldValue As Person)
			Dim eventHandler As EventHandler(Of PropertyChangedEventArgs(Of ValueType1, Person)) = TryCast(Me.Events(1), EventHandler(Of PropertyChangedEventArgs(Of ValueType1, Person)))
			If CObj(eventHandler) IsNot Nothing Then
				eventHandler.BeginInvoke(Me, New PropertyChangedEventArgs(Of ValueType1, Person)(Me, oldValue, Me.DoesSomethingWithPerson), New AsyncCallback(eventHandler.EndInvoke), Nothing)
				Me.RaisePropertyChangedEvent("DoesSomethingWithPerson")
			End If
		End Sub
		<DataObjectFieldAttribute(False, False, False)> _
		Public MustOverride Property ValueType1Value() As Integer
		<DataObjectFieldAttribute(False, False, True)> _
		Public MustOverride Property DoesSomethingWithPerson() As Person
		<DataObjectFieldAttribute(False, False, True)> _
		Public MustOverride ReadOnly Property DoesSomethingElseWithPerson() As ICollection(Of Person)
		Public Overloads Overrides Function ToString() As String
			Return Me.ToString(Nothing)
		End Function
		Public Overloads Overridable Function ToString(ByVal provider As IFormatProvider) As String
			Return String.Format(provider, "ValueType1{0}{{{0}{1}ValueType1Value = ""{2}"",{0}{1}DoesSomethingWithPerson = {3}{0}}}", Environment.NewLine, "", Me.ValueType1Value, "TODO: Recursively call ToString for customTypes...")
		End Function
	End Class
	#End Region
	#Region "IHasSampleModelContext"
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	Public Interface IHasSampleModelContext
		ReadOnly Property Context() As SampleModelContext
	End Interface
	#End Region
	#Region "ISampleModelContext"
	<GeneratedCodeAttribute("OIALtoPLiX", "1.0")> _
	Public Interface ISampleModelContext
		Function GetPersonDrivesCarByInternalUniquenessConstraint18(ByVal DrivesCar_vin As Integer, ByVal DrivenByPerson As Person) As PersonDrivesCar
		Function TryGetPersonDrivesCarByInternalUniquenessConstraint18(ByVal DrivesCar_vin As Integer, ByVal DrivenByPerson As Person, <System.Runtime.InteropServices.Out> ByRef PersonDrivesCar As PersonDrivesCar) As Boolean
		Function GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint23(ByVal Buyer As Person, ByVal CarSold_vin As Integer, ByVal Seller As Person) As PersonBoughtCarFromPersonOnDate
		Function TryGetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint23(ByVal Buyer As Person, ByVal CarSold_vin As Integer, ByVal Seller As Person, <System.Runtime.InteropServices.Out> ByRef PersonBoughtCarFromPersonOnDate As PersonBoughtCarFromPersonOnDate) As Boolean
		Function GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint24(ByVal SaleDate_YMD As Integer, ByVal Seller As Person, ByVal CarSold_vin As Integer) As PersonBoughtCarFromPersonOnDate
		Function TryGetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint24(ByVal SaleDate_YMD As Integer, ByVal Seller As Person, ByVal CarSold_vin As Integer, <System.Runtime.InteropServices.Out> ByRef PersonBoughtCarFromPersonOnDate As PersonBoughtCarFromPersonOnDate) As Boolean
		Function GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint25(ByVal CarSold_vin As Integer, ByVal SaleDate_YMD As Integer, ByVal Buyer As Person) As PersonBoughtCarFromPersonOnDate
		Function TryGetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint25(ByVal CarSold_vin As Integer, ByVal SaleDate_YMD As Integer, ByVal Buyer As Person, <System.Runtime.InteropServices.Out> ByRef PersonBoughtCarFromPersonOnDate As PersonBoughtCarFromPersonOnDate) As Boolean
		Function GetReviewByInternalUniquenessConstraint26(ByVal Car_vin As Integer, ByVal Criterion_Name As String) As Review
		Function TryGetReviewByInternalUniquenessConstraint26(ByVal Car_vin As Integer, ByVal Criterion_Name As String, <System.Runtime.InteropServices.Out> ByRef Review As Review) As Boolean
		Function GetPersonHasNickNameByInternalUniquenessConstraint33(ByVal NickName As String, ByVal Person As Person) As PersonHasNickName
		Function TryGetPersonHasNickNameByInternalUniquenessConstraint33(ByVal NickName As String, ByVal Person As Person, <System.Runtime.InteropServices.Out> ByRef PersonHasNickName As PersonHasNickName) As Boolean
		Function GetChildPersonByExternalUniquenessConstraint3(ByVal Father As MalePerson, ByVal BirthOrder_BirthOrder_Nr As Integer, ByVal Mother As FemalePerson) As ChildPerson
		Function TryGetChildPersonByExternalUniquenessConstraint3(ByVal Father As MalePerson, ByVal BirthOrder_BirthOrder_Nr As Integer, ByVal Mother As FemalePerson, <System.Runtime.InteropServices.Out> ByRef ChildPerson As ChildPerson) As Boolean
		Function GetPersonByExternalUniquenessConstraint1(ByVal FirstName As String, ByVal Date_YMD As Integer) As Person
		Function TryGetPersonByExternalUniquenessConstraint1(ByVal FirstName As String, ByVal Date_YMD As Integer, <System.Runtime.InteropServices.Out> ByRef Person As Person) As Boolean
		Function GetPersonByExternalUniquenessConstraint2(ByVal LastName As String, ByVal Date_YMD As Integer) As Person
		Function TryGetPersonByExternalUniquenessConstraint2(ByVal LastName As String, ByVal Date_YMD As Integer, <System.Runtime.InteropServices.Out> ByRef Person As Person) As Boolean
		Function GetPersonByOptionalUniqueString(ByVal OptionalUniqueString As String) As Person
		Function TryGetPersonByOptionalUniqueString(ByVal OptionalUniqueString As String, <System.Runtime.InteropServices.Out> ByRef Person As Person) As Boolean
		Function GetPersonByOwnsCar_vin(ByVal OwnsCar_vin As Integer) As Person
		Function TryGetPersonByOwnsCar_vin(ByVal OwnsCar_vin As Integer, <System.Runtime.InteropServices.Out> ByRef Person As Person) As Boolean
		Function GetPersonByOptionalUniqueDecimal(ByVal OptionalUniqueDecimal As Decimal) As Person
		Function TryGetPersonByOptionalUniqueDecimal(ByVal OptionalUniqueDecimal As Decimal, <System.Runtime.InteropServices.Out> ByRef Person As Person) As Boolean
		Function GetPersonByMandatoryUniqueDecimal(ByVal MandatoryUniqueDecimal As Decimal) As Person
		Function TryGetPersonByMandatoryUniqueDecimal(ByVal MandatoryUniqueDecimal As Decimal, <System.Runtime.InteropServices.Out> ByRef Person As Person) As Boolean
		Function GetPersonByMandatoryUniqueString(ByVal MandatoryUniqueString As String) As Person
		Function TryGetPersonByMandatoryUniqueString(ByVal MandatoryUniqueString As String, <System.Runtime.InteropServices.Out> ByRef Person As Person) As Boolean
		Function GetValueType1ByValueType1Value(ByVal ValueType1Value As Integer) As ValueType1
		Function TryGetValueType1ByValueType1Value(ByVal ValueType1Value As Integer, <System.Runtime.InteropServices.Out> ByRef ValueType1 As ValueType1) As Boolean
		Function CreatePersonDrivesCar(ByVal DrivesCar_vin As Integer, ByVal DrivenByPerson As Person) As PersonDrivesCar
		ReadOnly Property PersonDrivesCarCollection() As ReadOnlyCollection(Of PersonDrivesCar)
		Function CreatePersonBoughtCarFromPersonOnDate(ByVal CarSold_vin As Integer, ByVal SaleDate_YMD As Integer, ByVal Buyer As Person, ByVal Seller As Person) As PersonBoughtCarFromPersonOnDate
		ReadOnly Property PersonBoughtCarFromPersonOnDateCollection() As ReadOnlyCollection(Of PersonBoughtCarFromPersonOnDate)
		Function CreateReview(ByVal Car_vin As Integer, ByVal Rating_Nr_Integer As Integer, ByVal Criterion_Name As String) As Review
		ReadOnly Property ReviewCollection() As ReadOnlyCollection(Of Review)
		Function CreatePersonHasNickName(ByVal NickName As String, ByVal Person As Person) As PersonHasNickName
		ReadOnly Property PersonHasNickNameCollection() As ReadOnlyCollection(Of PersonHasNickName)
		Function CreatePerson(ByVal FirstName As String, ByVal Date_YMD As Integer, ByVal LastName As String, ByVal Gender_Gender_Code As String, ByVal MandatoryUniqueDecimal As Decimal, ByVal MandatoryUniqueString As String) As Person
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
	<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
	Public NotInheritable Class SampleModelContext
		Implements ISampleModelContext
		Public Sub New()
			Dim constraintEnforcementCollectionCallbacksByTypeDictionary As Dictionary(Of Type, Object) = New Dictionary(Of Type, Object)(7)
			Dim constraintEnforcementCollectionCallbacksByTypeAndNameDictionary As Dictionary(Of ConstraintEnforcementCollectionTypeAndPropertyNameKey, Object) = New Dictionary(Of ConstraintEnforcementCollectionTypeAndPropertyNameKey, Object)(2)
			Me._ContraintEnforcementCollectionCallbacksByTypeDictionary = constraintEnforcementCollectionCallbacksByTypeDictionary
			Me._ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary = constraintEnforcementCollectionCallbacksByTypeAndNameDictionary
			constraintEnforcementCollectionCallbacksByTypeDictionary.Add(GetType(ConstraintEnforcementCollection(Of Person, PersonDrivesCar)), New ConstraintEnforcementCollectionCallbacks(Of Person, PersonDrivesCar)(New PotentialCollectionModificationCallback(Of Person, PersonDrivesCar)(Me.OnPersonPersonDrivesCarAsDrivenByPersonAdding), New CommittedCollectionModificationCallback(Of Person, PersonDrivesCar)(Me.OnPersonPersonDrivesCarAsDrivenByPersonAdded), Nothing, New CommittedCollectionModificationCallback(Of Person, PersonDrivesCar)(Me.OnPersonPersonDrivesCarAsDrivenByPersonRemoved)))
			constraintEnforcementCollectionCallbacksByTypeAndNameDictionary.Add(New ConstraintEnforcementCollectionTypeAndPropertyNameKey(GetType(ConstraintEnforcementCollectionWithPropertyName(Of Person, PersonBoughtCarFromPersonOnDate)), "PersonBoughtCarFromPersonOnDateAsBuyer"), New ConstraintEnforcementCollectionCallbacks(Of Person, PersonBoughtCarFromPersonOnDate)(New PotentialCollectionModificationCallback(Of Person, PersonBoughtCarFromPersonOnDate)(Me.OnPersonPersonBoughtCarFromPersonOnDateAsBuyerAdding), New CommittedCollectionModificationCallback(Of Person, PersonBoughtCarFromPersonOnDate)(Me.OnPersonPersonBoughtCarFromPersonOnDateAsBuyerAdded), Nothing, New CommittedCollectionModificationCallback(Of Person, PersonBoughtCarFromPersonOnDate)(Me.OnPersonPersonBoughtCarFromPersonOnDateAsBuyerRemoved)))
			constraintEnforcementCollectionCallbacksByTypeAndNameDictionary.Add(New ConstraintEnforcementCollectionTypeAndPropertyNameKey(GetType(ConstraintEnforcementCollectionWithPropertyName(Of Person, PersonBoughtCarFromPersonOnDate)), "PersonBoughtCarFromPersonOnDateAsSeller"), New ConstraintEnforcementCollectionCallbacks(Of Person, PersonBoughtCarFromPersonOnDate)(New PotentialCollectionModificationCallback(Of Person, PersonBoughtCarFromPersonOnDate)(Me.OnPersonPersonBoughtCarFromPersonOnDateAsSellerAdding), New CommittedCollectionModificationCallback(Of Person, PersonBoughtCarFromPersonOnDate)(Me.OnPersonPersonBoughtCarFromPersonOnDateAsSellerAdded), Nothing, New CommittedCollectionModificationCallback(Of Person, PersonBoughtCarFromPersonOnDate)(Me.OnPersonPersonBoughtCarFromPersonOnDateAsSellerRemoved)))
			constraintEnforcementCollectionCallbacksByTypeDictionary.Add(GetType(ConstraintEnforcementCollection(Of Person, PersonHasNickName)), New ConstraintEnforcementCollectionCallbacks(Of Person, PersonHasNickName)(New PotentialCollectionModificationCallback(Of Person, PersonHasNickName)(Me.OnPersonPersonHasNickNameAsPersonAdding), New CommittedCollectionModificationCallback(Of Person, PersonHasNickName)(Me.OnPersonPersonHasNickNameAsPersonAdded), Nothing, New CommittedCollectionModificationCallback(Of Person, PersonHasNickName)(Me.OnPersonPersonHasNickNameAsPersonRemoved)))
			constraintEnforcementCollectionCallbacksByTypeDictionary.Add(GetType(ConstraintEnforcementCollection(Of Person, Task)), New ConstraintEnforcementCollectionCallbacks(Of Person, Task)(New PotentialCollectionModificationCallback(Of Person, Task)(Me.OnPersonTaskAdding), New CommittedCollectionModificationCallback(Of Person, Task)(Me.OnPersonTaskAdded), Nothing, New CommittedCollectionModificationCallback(Of Person, Task)(Me.OnPersonTaskRemoved)))
			constraintEnforcementCollectionCallbacksByTypeDictionary.Add(GetType(ConstraintEnforcementCollection(Of Person, ValueType1)), New ConstraintEnforcementCollectionCallbacks(Of Person, ValueType1)(New PotentialCollectionModificationCallback(Of Person, ValueType1)(Me.OnPersonValueType1DoesSomethingWithAdding), New CommittedCollectionModificationCallback(Of Person, ValueType1)(Me.OnPersonValueType1DoesSomethingWithAdded), Nothing, New CommittedCollectionModificationCallback(Of Person, ValueType1)(Me.OnPersonValueType1DoesSomethingWithRemoved)))
			constraintEnforcementCollectionCallbacksByTypeDictionary.Add(GetType(ConstraintEnforcementCollection(Of MalePerson, ChildPerson)), New ConstraintEnforcementCollectionCallbacks(Of MalePerson, ChildPerson)(New PotentialCollectionModificationCallback(Of MalePerson, ChildPerson)(Me.OnMalePersonChildPersonAdding), New CommittedCollectionModificationCallback(Of MalePerson, ChildPerson)(Me.OnMalePersonChildPersonAdded), Nothing, New CommittedCollectionModificationCallback(Of MalePerson, ChildPerson)(Me.OnMalePersonChildPersonRemoved)))
			constraintEnforcementCollectionCallbacksByTypeDictionary.Add(GetType(ConstraintEnforcementCollection(Of FemalePerson, ChildPerson)), New ConstraintEnforcementCollectionCallbacks(Of FemalePerson, ChildPerson)(New PotentialCollectionModificationCallback(Of FemalePerson, ChildPerson)(Me.OnFemalePersonChildPersonAdding), New CommittedCollectionModificationCallback(Of FemalePerson, ChildPerson)(Me.OnFemalePersonChildPersonAdded), Nothing, New CommittedCollectionModificationCallback(Of FemalePerson, ChildPerson)(Me.OnFemalePersonChildPersonRemoved)))
			constraintEnforcementCollectionCallbacksByTypeDictionary.Add(GetType(ConstraintEnforcementCollection(Of ValueType1, Person)), New ConstraintEnforcementCollectionCallbacks(Of ValueType1, Person)(New PotentialCollectionModificationCallback(Of ValueType1, Person)(Me.OnValueType1DoesSomethingElseWithPersonAdding), New CommittedCollectionModificationCallback(Of ValueType1, Person)(Me.OnValueType1DoesSomethingElseWithPersonAdded), Nothing, New CommittedCollectionModificationCallback(Of ValueType1, Person)(Me.OnValueType1DoesSomethingElseWithPersonRemoved)))
			Dim PersonDrivesCarList As List(Of PersonDrivesCar) = New List(Of PersonDrivesCar)()
			Me._PersonDrivesCarList = PersonDrivesCarList
			Me._PersonDrivesCarReadOnlyCollection = New ReadOnlyCollection(Of PersonDrivesCar)(PersonDrivesCarList)
			Dim PersonBoughtCarFromPersonOnDateList As List(Of PersonBoughtCarFromPersonOnDate) = New List(Of PersonBoughtCarFromPersonOnDate)()
			Me._PersonBoughtCarFromPersonOnDateList = PersonBoughtCarFromPersonOnDateList
			Me._PersonBoughtCarFromPersonOnDateReadOnlyCollection = New ReadOnlyCollection(Of PersonBoughtCarFromPersonOnDate)(PersonBoughtCarFromPersonOnDateList)
			Dim ReviewList As List(Of Review) = New List(Of Review)()
			Me._ReviewList = ReviewList
			Me._ReviewReadOnlyCollection = New ReadOnlyCollection(Of Review)(ReviewList)
			Dim PersonHasNickNameList As List(Of PersonHasNickName) = New List(Of PersonHasNickName)()
			Me._PersonHasNickNameList = PersonHasNickNameList
			Me._PersonHasNickNameReadOnlyCollection = New ReadOnlyCollection(Of PersonHasNickName)(PersonHasNickNameList)
			Dim PersonList As List(Of Person) = New List(Of Person)()
			Me._PersonList = PersonList
			Me._PersonReadOnlyCollection = New ReadOnlyCollection(Of Person)(PersonList)
			Dim MalePersonList As List(Of MalePerson) = New List(Of MalePerson)()
			Me._MalePersonList = MalePersonList
			Me._MalePersonReadOnlyCollection = New ReadOnlyCollection(Of MalePerson)(MalePersonList)
			Dim FemalePersonList As List(Of FemalePerson) = New List(Of FemalePerson)()
			Me._FemalePersonList = FemalePersonList
			Me._FemalePersonReadOnlyCollection = New ReadOnlyCollection(Of FemalePerson)(FemalePersonList)
			Dim ChildPersonList As List(Of ChildPerson) = New List(Of ChildPerson)()
			Me._ChildPersonList = ChildPersonList
			Me._ChildPersonReadOnlyCollection = New ReadOnlyCollection(Of ChildPerson)(ChildPersonList)
			Dim DeathList As List(Of Death) = New List(Of Death)()
			Me._DeathList = DeathList
			Me._DeathReadOnlyCollection = New ReadOnlyCollection(Of Death)(DeathList)
			Dim NaturalDeathList As List(Of NaturalDeath) = New List(Of NaturalDeath)()
			Me._NaturalDeathList = NaturalDeathList
			Me._NaturalDeathReadOnlyCollection = New ReadOnlyCollection(Of NaturalDeath)(NaturalDeathList)
			Dim UnnaturalDeathList As List(Of UnnaturalDeath) = New List(Of UnnaturalDeath)()
			Me._UnnaturalDeathList = UnnaturalDeathList
			Me._UnnaturalDeathReadOnlyCollection = New ReadOnlyCollection(Of UnnaturalDeath)(UnnaturalDeathList)
			Dim TaskList As List(Of Task) = New List(Of Task)()
			Me._TaskList = TaskList
			Me._TaskReadOnlyCollection = New ReadOnlyCollection(Of Task)(TaskList)
			Dim ValueType1List As List(Of ValueType1) = New List(Of ValueType1)()
			Me._ValueType1List = ValueType1List
			Me._ValueType1ReadOnlyCollection = New ReadOnlyCollection(Of ValueType1)(ValueType1List)
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
		Private ReadOnly _InternalUniquenessConstraint18Dictionary As Dictionary(Of Tuple(Of Integer, Person), PersonDrivesCar) = New Dictionary(Of Tuple(Of Integer, Person), PersonDrivesCar)()
		Public Function GetPersonDrivesCarByInternalUniquenessConstraint18(ByVal DrivesCar_vin As Integer, ByVal DrivenByPerson As Person) As PersonDrivesCar Implements _
			ISampleModelContext.GetPersonDrivesCarByInternalUniquenessConstraint18
			Return Me._InternalUniquenessConstraint18Dictionary(Tuple.CreateTuple(Of Integer, Person)(DrivesCar_vin, DrivenByPerson))
		End Function
		Public Function TryGetPersonDrivesCarByInternalUniquenessConstraint18(ByVal DrivesCar_vin As Integer, ByVal DrivenByPerson As Person, <System.Runtime.InteropServices.Out> ByRef PersonDrivesCar As PersonDrivesCar) As Boolean Implements _
			ISampleModelContext.TryGetPersonDrivesCarByInternalUniquenessConstraint18
			Return Me._InternalUniquenessConstraint18Dictionary.TryGetValue(Tuple.CreateTuple(Of Integer, Person)(DrivesCar_vin, DrivenByPerson), PersonDrivesCar)
		End Function
		Private Function OnInternalUniquenessConstraint18Changing(ByVal instance As PersonDrivesCar, ByVal newValue As Tuple(Of Integer, Person)) As Boolean
			If CObj(newValue) IsNot Nothing Then
				Dim currentInstance As PersonDrivesCar
				If Me._InternalUniquenessConstraint18Dictionary.TryGetValue(newValue, currentInstance) Then
					Return CObj(currentInstance) Is instance
				End If
			End If
			Return True
		End Function
		Private Sub OnInternalUniquenessConstraint18Changed(ByVal instance As PersonDrivesCar, ByVal oldValue As Tuple(Of Integer, Person), ByVal newValue As Tuple(Of Integer, Person))
			If CObj(oldValue) IsNot Nothing Then
				Me._InternalUniquenessConstraint18Dictionary.Remove(oldValue)
			End If
			If CObj(newValue) IsNot Nothing Then
				Me._InternalUniquenessConstraint18Dictionary.Add(newValue, instance)
			End If
		End Sub
		Private ReadOnly _InternalUniquenessConstraint23Dictionary As Dictionary(Of Tuple(Of Person, Integer, Person), PersonBoughtCarFromPersonOnDate) = New Dictionary(Of Tuple(Of Person, Integer, Person), PersonBoughtCarFromPersonOnDate)()
		Public Function GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint23(ByVal Buyer As Person, ByVal CarSold_vin As Integer, ByVal Seller As Person) As PersonBoughtCarFromPersonOnDate Implements _
			ISampleModelContext.GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint23
			Return Me._InternalUniquenessConstraint23Dictionary(Tuple.CreateTuple(Of Person, Integer, Person)(Buyer, CarSold_vin, Seller))
		End Function
		Public Function TryGetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint23(ByVal Buyer As Person, ByVal CarSold_vin As Integer, ByVal Seller As Person, <System.Runtime.InteropServices.Out> ByRef PersonBoughtCarFromPersonOnDate As PersonBoughtCarFromPersonOnDate) As Boolean Implements _
			ISampleModelContext.TryGetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint23
			Return Me._InternalUniquenessConstraint23Dictionary.TryGetValue(Tuple.CreateTuple(Of Person, Integer, Person)(Buyer, CarSold_vin, Seller), PersonBoughtCarFromPersonOnDate)
		End Function
		Private Function OnInternalUniquenessConstraint23Changing(ByVal instance As PersonBoughtCarFromPersonOnDate, ByVal newValue As Tuple(Of Person, Integer, Person)) As Boolean
			If CObj(newValue) IsNot Nothing Then
				Dim currentInstance As PersonBoughtCarFromPersonOnDate
				If Me._InternalUniquenessConstraint23Dictionary.TryGetValue(newValue, currentInstance) Then
					Return CObj(currentInstance) Is instance
				End If
			End If
			Return True
		End Function
		Private Sub OnInternalUniquenessConstraint23Changed(ByVal instance As PersonBoughtCarFromPersonOnDate, ByVal oldValue As Tuple(Of Person, Integer, Person), ByVal newValue As Tuple(Of Person, Integer, Person))
			If CObj(oldValue) IsNot Nothing Then
				Me._InternalUniquenessConstraint23Dictionary.Remove(oldValue)
			End If
			If CObj(newValue) IsNot Nothing Then
				Me._InternalUniquenessConstraint23Dictionary.Add(newValue, instance)
			End If
		End Sub
		Private ReadOnly _InternalUniquenessConstraint24Dictionary As Dictionary(Of Tuple(Of Integer, Person, Integer), PersonBoughtCarFromPersonOnDate) = New Dictionary(Of Tuple(Of Integer, Person, Integer), PersonBoughtCarFromPersonOnDate)()
		Public Function GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint24(ByVal SaleDate_YMD As Integer, ByVal Seller As Person, ByVal CarSold_vin As Integer) As PersonBoughtCarFromPersonOnDate Implements _
			ISampleModelContext.GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint24
			Return Me._InternalUniquenessConstraint24Dictionary(Tuple.CreateTuple(Of Integer, Person, Integer)(SaleDate_YMD, Seller, CarSold_vin))
		End Function
		Public Function TryGetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint24(ByVal SaleDate_YMD As Integer, ByVal Seller As Person, ByVal CarSold_vin As Integer, <System.Runtime.InteropServices.Out> ByRef PersonBoughtCarFromPersonOnDate As PersonBoughtCarFromPersonOnDate) As Boolean Implements _
			ISampleModelContext.TryGetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint24
			Return Me._InternalUniquenessConstraint24Dictionary.TryGetValue(Tuple.CreateTuple(Of Integer, Person, Integer)(SaleDate_YMD, Seller, CarSold_vin), PersonBoughtCarFromPersonOnDate)
		End Function
		Private Function OnInternalUniquenessConstraint24Changing(ByVal instance As PersonBoughtCarFromPersonOnDate, ByVal newValue As Tuple(Of Integer, Person, Integer)) As Boolean
			If CObj(newValue) IsNot Nothing Then
				Dim currentInstance As PersonBoughtCarFromPersonOnDate
				If Me._InternalUniquenessConstraint24Dictionary.TryGetValue(newValue, currentInstance) Then
					Return CObj(currentInstance) Is instance
				End If
			End If
			Return True
		End Function
		Private Sub OnInternalUniquenessConstraint24Changed(ByVal instance As PersonBoughtCarFromPersonOnDate, ByVal oldValue As Tuple(Of Integer, Person, Integer), ByVal newValue As Tuple(Of Integer, Person, Integer))
			If CObj(oldValue) IsNot Nothing Then
				Me._InternalUniquenessConstraint24Dictionary.Remove(oldValue)
			End If
			If CObj(newValue) IsNot Nothing Then
				Me._InternalUniquenessConstraint24Dictionary.Add(newValue, instance)
			End If
		End Sub
		Private ReadOnly _InternalUniquenessConstraint25Dictionary As Dictionary(Of Tuple(Of Integer, Integer, Person), PersonBoughtCarFromPersonOnDate) = New Dictionary(Of Tuple(Of Integer, Integer, Person), PersonBoughtCarFromPersonOnDate)()
		Public Function GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint25(ByVal CarSold_vin As Integer, ByVal SaleDate_YMD As Integer, ByVal Buyer As Person) As PersonBoughtCarFromPersonOnDate Implements _
			ISampleModelContext.GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint25
			Return Me._InternalUniquenessConstraint25Dictionary(Tuple.CreateTuple(Of Integer, Integer, Person)(CarSold_vin, SaleDate_YMD, Buyer))
		End Function
		Public Function TryGetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint25(ByVal CarSold_vin As Integer, ByVal SaleDate_YMD As Integer, ByVal Buyer As Person, <System.Runtime.InteropServices.Out> ByRef PersonBoughtCarFromPersonOnDate As PersonBoughtCarFromPersonOnDate) As Boolean Implements _
			ISampleModelContext.TryGetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint25
			Return Me._InternalUniquenessConstraint25Dictionary.TryGetValue(Tuple.CreateTuple(Of Integer, Integer, Person)(CarSold_vin, SaleDate_YMD, Buyer), PersonBoughtCarFromPersonOnDate)
		End Function
		Private Function OnInternalUniquenessConstraint25Changing(ByVal instance As PersonBoughtCarFromPersonOnDate, ByVal newValue As Tuple(Of Integer, Integer, Person)) As Boolean
			If CObj(newValue) IsNot Nothing Then
				Dim currentInstance As PersonBoughtCarFromPersonOnDate
				If Me._InternalUniquenessConstraint25Dictionary.TryGetValue(newValue, currentInstance) Then
					Return CObj(currentInstance) Is instance
				End If
			End If
			Return True
		End Function
		Private Sub OnInternalUniquenessConstraint25Changed(ByVal instance As PersonBoughtCarFromPersonOnDate, ByVal oldValue As Tuple(Of Integer, Integer, Person), ByVal newValue As Tuple(Of Integer, Integer, Person))
			If CObj(oldValue) IsNot Nothing Then
				Me._InternalUniquenessConstraint25Dictionary.Remove(oldValue)
			End If
			If CObj(newValue) IsNot Nothing Then
				Me._InternalUniquenessConstraint25Dictionary.Add(newValue, instance)
			End If
		End Sub
		Private ReadOnly _InternalUniquenessConstraint26Dictionary As Dictionary(Of Tuple(Of Integer, String), Review) = New Dictionary(Of Tuple(Of Integer, String), Review)()
		Public Function GetReviewByInternalUniquenessConstraint26(ByVal Car_vin As Integer, ByVal Criterion_Name As String) As Review Implements _
			ISampleModelContext.GetReviewByInternalUniquenessConstraint26
			Return Me._InternalUniquenessConstraint26Dictionary(Tuple.CreateTuple(Of Integer, String)(Car_vin, Criterion_Name))
		End Function
		Public Function TryGetReviewByInternalUniquenessConstraint26(ByVal Car_vin As Integer, ByVal Criterion_Name As String, <System.Runtime.InteropServices.Out> ByRef Review As Review) As Boolean Implements _
			ISampleModelContext.TryGetReviewByInternalUniquenessConstraint26
			Return Me._InternalUniquenessConstraint26Dictionary.TryGetValue(Tuple.CreateTuple(Of Integer, String)(Car_vin, Criterion_Name), Review)
		End Function
		Private Function OnInternalUniquenessConstraint26Changing(ByVal instance As Review, ByVal newValue As Tuple(Of Integer, String)) As Boolean
			If CObj(newValue) IsNot Nothing Then
				Dim currentInstance As Review
				If Me._InternalUniquenessConstraint26Dictionary.TryGetValue(newValue, currentInstance) Then
					Return CObj(currentInstance) Is instance
				End If
			End If
			Return True
		End Function
		Private Sub OnInternalUniquenessConstraint26Changed(ByVal instance As Review, ByVal oldValue As Tuple(Of Integer, String), ByVal newValue As Tuple(Of Integer, String))
			If CObj(oldValue) IsNot Nothing Then
				Me._InternalUniquenessConstraint26Dictionary.Remove(oldValue)
			End If
			If CObj(newValue) IsNot Nothing Then
				Me._InternalUniquenessConstraint26Dictionary.Add(newValue, instance)
			End If
		End Sub
		Private ReadOnly _InternalUniquenessConstraint33Dictionary As Dictionary(Of Tuple(Of String, Person), PersonHasNickName) = New Dictionary(Of Tuple(Of String, Person), PersonHasNickName)()
		Public Function GetPersonHasNickNameByInternalUniquenessConstraint33(ByVal NickName As String, ByVal Person As Person) As PersonHasNickName Implements _
			ISampleModelContext.GetPersonHasNickNameByInternalUniquenessConstraint33
			Return Me._InternalUniquenessConstraint33Dictionary(Tuple.CreateTuple(Of String, Person)(NickName, Person))
		End Function
		Public Function TryGetPersonHasNickNameByInternalUniquenessConstraint33(ByVal NickName As String, ByVal Person As Person, <System.Runtime.InteropServices.Out> ByRef PersonHasNickName As PersonHasNickName) As Boolean Implements _
			ISampleModelContext.TryGetPersonHasNickNameByInternalUniquenessConstraint33
			Return Me._InternalUniquenessConstraint33Dictionary.TryGetValue(Tuple.CreateTuple(Of String, Person)(NickName, Person), PersonHasNickName)
		End Function
		Private Function OnInternalUniquenessConstraint33Changing(ByVal instance As PersonHasNickName, ByVal newValue As Tuple(Of String, Person)) As Boolean
			If CObj(newValue) IsNot Nothing Then
				Dim currentInstance As PersonHasNickName
				If Me._InternalUniquenessConstraint33Dictionary.TryGetValue(newValue, currentInstance) Then
					Return CObj(currentInstance) Is instance
				End If
			End If
			Return True
		End Function
		Private Sub OnInternalUniquenessConstraint33Changed(ByVal instance As PersonHasNickName, ByVal oldValue As Tuple(Of String, Person), ByVal newValue As Tuple(Of String, Person))
			If CObj(oldValue) IsNot Nothing Then
				Me._InternalUniquenessConstraint33Dictionary.Remove(oldValue)
			End If
			If CObj(newValue) IsNot Nothing Then
				Me._InternalUniquenessConstraint33Dictionary.Add(newValue, instance)
			End If
		End Sub
		Private ReadOnly _ExternalUniquenessConstraint3Dictionary As Dictionary(Of Tuple(Of MalePerson, Integer, FemalePerson), ChildPerson) = New Dictionary(Of Tuple(Of MalePerson, Integer, FemalePerson), ChildPerson)()
		Public Function GetChildPersonByExternalUniquenessConstraint3(ByVal Father As MalePerson, ByVal BirthOrder_BirthOrder_Nr As Integer, ByVal Mother As FemalePerson) As ChildPerson Implements _
			ISampleModelContext.GetChildPersonByExternalUniquenessConstraint3
			Return Me._ExternalUniquenessConstraint3Dictionary(Tuple.CreateTuple(Of MalePerson, Integer, FemalePerson)(Father, BirthOrder_BirthOrder_Nr, Mother))
		End Function
		Public Function TryGetChildPersonByExternalUniquenessConstraint3(ByVal Father As MalePerson, ByVal BirthOrder_BirthOrder_Nr As Integer, ByVal Mother As FemalePerson, <System.Runtime.InteropServices.Out> ByRef ChildPerson As ChildPerson) As Boolean Implements _
			ISampleModelContext.TryGetChildPersonByExternalUniquenessConstraint3
			Return Me._ExternalUniquenessConstraint3Dictionary.TryGetValue(Tuple.CreateTuple(Of MalePerson, Integer, FemalePerson)(Father, BirthOrder_BirthOrder_Nr, Mother), ChildPerson)
		End Function
		Private Function OnExternalUniquenessConstraint3Changing(ByVal instance As ChildPerson, ByVal newValue As Tuple(Of MalePerson, Integer, FemalePerson)) As Boolean
			If CObj(newValue) IsNot Nothing Then
				Dim currentInstance As ChildPerson
				If Me._ExternalUniquenessConstraint3Dictionary.TryGetValue(newValue, currentInstance) Then
					Return CObj(currentInstance) Is instance
				End If
			End If
			Return True
		End Function
		Private Sub OnExternalUniquenessConstraint3Changed(ByVal instance As ChildPerson, ByVal oldValue As Tuple(Of MalePerson, Integer, FemalePerson), ByVal newValue As Tuple(Of MalePerson, Integer, FemalePerson))
			If CObj(oldValue) IsNot Nothing Then
				Me._ExternalUniquenessConstraint3Dictionary.Remove(oldValue)
			End If
			If CObj(newValue) IsNot Nothing Then
				Me._ExternalUniquenessConstraint3Dictionary.Add(newValue, instance)
			End If
		End Sub
		Private ReadOnly _ExternalUniquenessConstraint1Dictionary As Dictionary(Of Tuple(Of String, Integer), Person) = New Dictionary(Of Tuple(Of String, Integer), Person)()
		Public Function GetPersonByExternalUniquenessConstraint1(ByVal FirstName As String, ByVal Date_YMD As Integer) As Person Implements _
			ISampleModelContext.GetPersonByExternalUniquenessConstraint1
			Return Me._ExternalUniquenessConstraint1Dictionary(Tuple.CreateTuple(Of String, Integer)(FirstName, Date_YMD))
		End Function
		Public Function TryGetPersonByExternalUniquenessConstraint1(ByVal FirstName As String, ByVal Date_YMD As Integer, <System.Runtime.InteropServices.Out> ByRef Person As Person) As Boolean Implements _
			ISampleModelContext.TryGetPersonByExternalUniquenessConstraint1
			Return Me._ExternalUniquenessConstraint1Dictionary.TryGetValue(Tuple.CreateTuple(Of String, Integer)(FirstName, Date_YMD), Person)
		End Function
		Private Function OnExternalUniquenessConstraint1Changing(ByVal instance As Person, ByVal newValue As Tuple(Of String, Integer)) As Boolean
			If CObj(newValue) IsNot Nothing Then
				Dim currentInstance As Person
				If Me._ExternalUniquenessConstraint1Dictionary.TryGetValue(newValue, currentInstance) Then
					Return CObj(currentInstance) Is instance
				End If
			End If
			Return True
		End Function
		Private Sub OnExternalUniquenessConstraint1Changed(ByVal instance As Person, ByVal oldValue As Tuple(Of String, Integer), ByVal newValue As Tuple(Of String, Integer))
			If CObj(oldValue) IsNot Nothing Then
				Me._ExternalUniquenessConstraint1Dictionary.Remove(oldValue)
			End If
			If CObj(newValue) IsNot Nothing Then
				Me._ExternalUniquenessConstraint1Dictionary.Add(newValue, instance)
			End If
		End Sub
		Private ReadOnly _ExternalUniquenessConstraint2Dictionary As Dictionary(Of Tuple(Of String, Integer), Person) = New Dictionary(Of Tuple(Of String, Integer), Person)()
		Public Function GetPersonByExternalUniquenessConstraint2(ByVal LastName As String, ByVal Date_YMD As Integer) As Person Implements _
			ISampleModelContext.GetPersonByExternalUniquenessConstraint2
			Return Me._ExternalUniquenessConstraint2Dictionary(Tuple.CreateTuple(Of String, Integer)(LastName, Date_YMD))
		End Function
		Public Function TryGetPersonByExternalUniquenessConstraint2(ByVal LastName As String, ByVal Date_YMD As Integer, <System.Runtime.InteropServices.Out> ByRef Person As Person) As Boolean Implements _
			ISampleModelContext.TryGetPersonByExternalUniquenessConstraint2
			Return Me._ExternalUniquenessConstraint2Dictionary.TryGetValue(Tuple.CreateTuple(Of String, Integer)(LastName, Date_YMD), Person)
		End Function
		Private Function OnExternalUniquenessConstraint2Changing(ByVal instance As Person, ByVal newValue As Tuple(Of String, Integer)) As Boolean
			If CObj(newValue) IsNot Nothing Then
				Dim currentInstance As Person
				If Me._ExternalUniquenessConstraint2Dictionary.TryGetValue(newValue, currentInstance) Then
					Return CObj(currentInstance) Is instance
				End If
			End If
			Return True
		End Function
		Private Sub OnExternalUniquenessConstraint2Changed(ByVal instance As Person, ByVal oldValue As Tuple(Of String, Integer), ByVal newValue As Tuple(Of String, Integer))
			If CObj(oldValue) IsNot Nothing Then
				Me._ExternalUniquenessConstraint2Dictionary.Remove(oldValue)
			End If
			If CObj(newValue) IsNot Nothing Then
				Me._ExternalUniquenessConstraint2Dictionary.Add(newValue, instance)
			End If
		End Sub
		Private ReadOnly _PersonOptionalUniqueStringDictionary As Dictionary(Of String, Person) = New Dictionary(Of String, Person)()
		Public Function GetPersonByOptionalUniqueString(ByVal OptionalUniqueString As String) As Person Implements _
			ISampleModelContext.GetPersonByOptionalUniqueString
			Return Me._PersonOptionalUniqueStringDictionary(OptionalUniqueString)
		End Function
		Public Function TryGetPersonByOptionalUniqueString(ByVal OptionalUniqueString As String, <System.Runtime.InteropServices.Out> ByRef Person As Person) As Boolean Implements _
			ISampleModelContext.TryGetPersonByOptionalUniqueString
			Return Me._PersonOptionalUniqueStringDictionary.TryGetValue(OptionalUniqueString, Person)
		End Function
		Private ReadOnly _PersonOwnsCar_vinDictionary As Dictionary(Of Integer, Person) = New Dictionary(Of Integer, Person)()
		Public Function GetPersonByOwnsCar_vin(ByVal OwnsCar_vin As Integer) As Person Implements _
			ISampleModelContext.GetPersonByOwnsCar_vin
			Return Me._PersonOwnsCar_vinDictionary(OwnsCar_vin)
		End Function
		Public Function TryGetPersonByOwnsCar_vin(ByVal OwnsCar_vin As Integer, <System.Runtime.InteropServices.Out> ByRef Person As Person) As Boolean Implements _
			ISampleModelContext.TryGetPersonByOwnsCar_vin
			Return Me._PersonOwnsCar_vinDictionary.TryGetValue(OwnsCar_vin, Person)
		End Function
		Private ReadOnly _PersonOptionalUniqueDecimalDictionary As Dictionary(Of Decimal, Person) = New Dictionary(Of Decimal, Person)()
		Public Function GetPersonByOptionalUniqueDecimal(ByVal OptionalUniqueDecimal As Decimal) As Person Implements _
			ISampleModelContext.GetPersonByOptionalUniqueDecimal
			Return Me._PersonOptionalUniqueDecimalDictionary(OptionalUniqueDecimal)
		End Function
		Public Function TryGetPersonByOptionalUniqueDecimal(ByVal OptionalUniqueDecimal As Decimal, <System.Runtime.InteropServices.Out> ByRef Person As Person) As Boolean Implements _
			ISampleModelContext.TryGetPersonByOptionalUniqueDecimal
			Return Me._PersonOptionalUniqueDecimalDictionary.TryGetValue(OptionalUniqueDecimal, Person)
		End Function
		Private ReadOnly _PersonMandatoryUniqueDecimalDictionary As Dictionary(Of Decimal, Person) = New Dictionary(Of Decimal, Person)()
		Public Function GetPersonByMandatoryUniqueDecimal(ByVal MandatoryUniqueDecimal As Decimal) As Person Implements _
			ISampleModelContext.GetPersonByMandatoryUniqueDecimal
			Return Me._PersonMandatoryUniqueDecimalDictionary(MandatoryUniqueDecimal)
		End Function
		Public Function TryGetPersonByMandatoryUniqueDecimal(ByVal MandatoryUniqueDecimal As Decimal, <System.Runtime.InteropServices.Out> ByRef Person As Person) As Boolean Implements _
			ISampleModelContext.TryGetPersonByMandatoryUniqueDecimal
			Return Me._PersonMandatoryUniqueDecimalDictionary.TryGetValue(MandatoryUniqueDecimal, Person)
		End Function
		Private ReadOnly _PersonMandatoryUniqueStringDictionary As Dictionary(Of String, Person) = New Dictionary(Of String, Person)()
		Public Function GetPersonByMandatoryUniqueString(ByVal MandatoryUniqueString As String) As Person Implements _
			ISampleModelContext.GetPersonByMandatoryUniqueString
			Return Me._PersonMandatoryUniqueStringDictionary(MandatoryUniqueString)
		End Function
		Public Function TryGetPersonByMandatoryUniqueString(ByVal MandatoryUniqueString As String, <System.Runtime.InteropServices.Out> ByRef Person As Person) As Boolean Implements _
			ISampleModelContext.TryGetPersonByMandatoryUniqueString
			Return Me._PersonMandatoryUniqueStringDictionary.TryGetValue(MandatoryUniqueString, Person)
		End Function
		Private ReadOnly _ValueType1ValueType1ValueDictionary As Dictionary(Of Integer, ValueType1) = New Dictionary(Of Integer, ValueType1)()
		Public Function GetValueType1ByValueType1Value(ByVal ValueType1Value As Integer) As ValueType1 Implements _
			ISampleModelContext.GetValueType1ByValueType1Value
			Return Me._ValueType1ValueType1ValueDictionary(ValueType1Value)
		End Function
		Public Function TryGetValueType1ByValueType1Value(ByVal ValueType1Value As Integer, <System.Runtime.InteropServices.Out> ByRef ValueType1 As ValueType1) As Boolean Implements _
			ISampleModelContext.TryGetValueType1ByValueType1Value
			Return Me._ValueType1ValueType1ValueDictionary.TryGetValue(ValueType1Value, ValueType1)
		End Function
		#End Region
		#Region "ConstraintEnforcementCollection"
		Private Delegate Function PotentialCollectionModificationCallback(Of TClass As {IHasSampleModelContext, Class}, TProperty)(ByVal instance As TClass, ByVal value As TProperty) As Boolean
		Private Delegate Sub CommittedCollectionModificationCallback(Of TClass As {IHasSampleModelContext, Class}, TProperty)(ByVal instance As TClass, ByVal value As TProperty)
		<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
		Private NotInheritable Class ConstraintEnforcementCollectionCallbacks(Of TClass As {IHasSampleModelContext, Class}, TProperty)
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
		Private Overloads Function OnAdding(Of TClass As {IHasSampleModelContext, Class}, TProperty)(ByVal instance As TClass, ByVal value As TProperty) As Boolean
			Dim adding As PotentialCollectionModificationCallback(Of TClass, TProperty) = (CType(Me._ContraintEnforcementCollectionCallbacksByTypeDictionary(GetType(ConstraintEnforcementCollection(Of TClass, TProperty))), ConstraintEnforcementCollectionCallbacks(Of TClass, TProperty))).Adding
			If adding IsNot Nothing Then
				Return adding(instance, value)
			End If
			Return True
		End Function
		Private Overloads Function OnAdding(Of TClass As {IHasSampleModelContext, Class}, TProperty)(ByVal propertyName As String, ByVal instance As TClass, ByVal value As TProperty) As Boolean
			Dim adding As PotentialCollectionModificationCallback(Of TClass, TProperty) = (CType(Me._ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary(New ConstraintEnforcementCollectionTypeAndPropertyNameKey(GetType(ConstraintEnforcementCollectionWithPropertyName(Of TClass, TProperty)), propertyName)), ConstraintEnforcementCollectionCallbacks(Of TClass, TProperty))).Adding
			If adding IsNot Nothing Then
				Return adding(instance, value)
			End If
			Return True
		End Function
		Private Overloads Sub OnAdded(Of TClass As {IHasSampleModelContext, Class}, TProperty)(ByVal instance As TClass, ByVal value As TProperty)
			Dim added As CommittedCollectionModificationCallback(Of TClass, TProperty) = (CType(Me._ContraintEnforcementCollectionCallbacksByTypeDictionary(GetType(ConstraintEnforcementCollection(Of TClass, TProperty))), ConstraintEnforcementCollectionCallbacks(Of TClass, TProperty))).Added
			If added IsNot Nothing Then
				added(instance, value)
			End If
		End Sub
		Private Overloads Sub OnAdded(Of TClass As {IHasSampleModelContext, Class}, TProperty)(ByVal propertyName As String, ByVal instance As TClass, ByVal value As TProperty)
			Dim added As CommittedCollectionModificationCallback(Of TClass, TProperty) = (CType(Me._ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary(New ConstraintEnforcementCollectionTypeAndPropertyNameKey(GetType(ConstraintEnforcementCollectionWithPropertyName(Of TClass, TProperty)), propertyName)), ConstraintEnforcementCollectionCallbacks(Of TClass, TProperty))).Added
			If added IsNot Nothing Then
				added(instance, value)
			End If
		End Sub
		Private Overloads Function OnRemoving(Of TClass As {IHasSampleModelContext, Class}, TProperty)(ByVal instance As TClass, ByVal value As TProperty) As Boolean
			Dim removing As PotentialCollectionModificationCallback(Of TClass, TProperty) = (CType(Me._ContraintEnforcementCollectionCallbacksByTypeDictionary(GetType(ConstraintEnforcementCollection(Of TClass, TProperty))), ConstraintEnforcementCollectionCallbacks(Of TClass, TProperty))).Removing
			If removing IsNot Nothing Then
				Return removing(instance, value)
			End If
			Return True
		End Function
		Private Overloads Function OnRemoving(Of TClass As {IHasSampleModelContext, Class}, TProperty)(ByVal propertyName As String, ByVal instance As TClass, ByVal value As TProperty) As Boolean
			Dim removing As PotentialCollectionModificationCallback(Of TClass, TProperty) = (CType(Me._ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary(New ConstraintEnforcementCollectionTypeAndPropertyNameKey(GetType(ConstraintEnforcementCollectionWithPropertyName(Of TClass, TProperty)), propertyName)), ConstraintEnforcementCollectionCallbacks(Of TClass, TProperty))).Removing
			If removing IsNot Nothing Then
				Return removing(instance, value)
			End If
			Return True
		End Function
		Private Overloads Sub OnRemoved(Of TClass As {IHasSampleModelContext, Class}, TProperty)(ByVal instance As TClass, ByVal value As TProperty)
			Dim removed As CommittedCollectionModificationCallback(Of TClass, TProperty) = (CType(Me._ContraintEnforcementCollectionCallbacksByTypeDictionary(GetType(ConstraintEnforcementCollection(Of TClass, TProperty))), ConstraintEnforcementCollectionCallbacks(Of TClass, TProperty))).Removed
			If removed IsNot Nothing Then
				removed(instance, value)
			End If
		End Sub
		Private Overloads Sub OnRemoved(Of TClass As {IHasSampleModelContext, Class}, TProperty)(ByVal propertyName As String, ByVal instance As TClass, ByVal value As TProperty)
			Dim removed As CommittedCollectionModificationCallback(Of TClass, TProperty) = (CType(Me._ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary(New ConstraintEnforcementCollectionTypeAndPropertyNameKey(GetType(ConstraintEnforcementCollectionWithPropertyName(Of TClass, TProperty)), propertyName)), ConstraintEnforcementCollectionCallbacks(Of TClass, TProperty))).Removed
			If removed IsNot Nothing Then
				removed(instance, value)
			End If
		End Sub
		<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
		Private NotInheritable Class ConstraintEnforcementCollection(Of TClass As {IHasSampleModelContext, Class}, TProperty)
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
		Private NotInheritable Class ConstraintEnforcementCollectionWithPropertyName(Of TClass As {IHasSampleModelContext, Class}, TProperty)
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
		#Region "PersonDrivesCar"
		Public Function CreatePersonDrivesCar(ByVal DrivesCar_vin As Integer, ByVal DrivenByPerson As Person) As PersonDrivesCar Implements _
			ISampleModelContext.CreatePersonDrivesCar
			If CObj(DrivenByPerson) Is Nothing Then
				Throw New ArgumentNullException("DrivenByPerson")
			End If
			If Not (Me.OnPersonDrivesCarDrivesCar_vinChanging(Nothing, DrivesCar_vin)) Then
				Throw SampleModelContext.GetConstraintEnforcementFailedException("DrivesCar_vin")
			End If
			If Not (Me.OnPersonDrivesCarDrivenByPersonChanging(Nothing, DrivenByPerson)) Then
				Throw SampleModelContext.GetConstraintEnforcementFailedException("DrivenByPerson")
			End If
			Return New PersonDrivesCarCore(Me, DrivesCar_vin, DrivenByPerson)
		End Function
		Private Function OnPersonDrivesCarDrivesCar_vinChanging(ByVal instance As PersonDrivesCar, ByVal newValue As Integer) As Boolean
			If CObj(instance) IsNot Nothing Then
				If Not (Me.OnInternalUniquenessConstraint18Changing(instance, Tuple.CreateTuple(Of Integer, Person)(newValue, instance.DrivenByPerson))) Then
					Return False
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnPersonDrivesCarDrivesCar_vinChanged(ByVal instance As PersonDrivesCar, ByVal oldValue As Nullable(Of Integer))
			Dim InternalUniquenessConstraint18OldValueTuple As Tuple(Of Integer, Person)
			If oldValue.HasValue Then
				InternalUniquenessConstraint18OldValueTuple = Tuple.CreateTuple(Of Integer, Person)(oldValue.Value, instance.DrivenByPerson)
			Else
				InternalUniquenessConstraint18OldValueTuple = Nothing
			End If
			Me.OnInternalUniquenessConstraint18Changed(instance, InternalUniquenessConstraint18OldValueTuple, Tuple.CreateTuple(Of Integer, Person)(instance.DrivesCar_vin, instance.DrivenByPerson))
		End Sub
		Private Function OnPersonDrivesCarDrivenByPersonChanging(ByVal instance As PersonDrivesCar, ByVal newValue As Person) As Boolean
			If CObj(Me) IsNot newValue.Context Then
				Throw SampleModelContext.GetDifferentContextsException()
			End If
			If CObj(instance) IsNot Nothing Then
				If Not (Me.OnInternalUniquenessConstraint18Changing(instance, Tuple.CreateTuple(Of Integer, Person)(instance.DrivesCar_vin, newValue))) Then
					Return False
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnPersonDrivesCarDrivenByPersonChanged(ByVal instance As PersonDrivesCar, ByVal oldValue As Person)
			instance.DrivenByPerson.PersonDrivesCarAsDrivenByPerson.Add(instance)
			Dim InternalUniquenessConstraint18OldValueTuple As Tuple(Of Integer, Person)
			If CObj(oldValue) IsNot Nothing Then
				oldValue.PersonDrivesCarAsDrivenByPerson.Remove(instance)
				InternalUniquenessConstraint18OldValueTuple = Tuple.CreateTuple(Of Integer, Person)(instance.DrivesCar_vin, oldValue)
			Else
				InternalUniquenessConstraint18OldValueTuple = Nothing
			End If
			Me.OnInternalUniquenessConstraint18Changed(instance, InternalUniquenessConstraint18OldValueTuple, Tuple.CreateTuple(Of Integer, Person)(instance.DrivesCar_vin, instance.DrivenByPerson))
		End Sub
		Private ReadOnly _PersonDrivesCarList As List(Of PersonDrivesCar)
		Private ReadOnly _PersonDrivesCarReadOnlyCollection As ReadOnlyCollection(Of PersonDrivesCar)
		Public ReadOnly Property PersonDrivesCarCollection() As ReadOnlyCollection(Of PersonDrivesCar) Implements _
			ISampleModelContext.PersonDrivesCarCollection
			Get
				Return Me._PersonDrivesCarReadOnlyCollection
			End Get
		End Property
		#Region "PersonDrivesCarCore"
		<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
		Private NotInheritable Class PersonDrivesCarCore
			Inherits PersonDrivesCar
			Public Sub New(ByVal context As SampleModelContext, ByVal DrivesCar_vin As Integer, ByVal DrivenByPerson As Person)
				Me._Context = context
				Me._DrivesCar_vin = DrivesCar_vin
				context.OnPersonDrivesCarDrivesCar_vinChanged(Me, Nothing)
				Me._DrivenByPerson = DrivenByPerson
				context.OnPersonDrivesCarDrivenByPersonChanged(Me, Nothing)
				context._PersonDrivesCarList.Add(Me)
			End Sub
			Private ReadOnly _Context As SampleModelContext
			Public Overrides ReadOnly Property Context() As SampleModelContext
				Get
					Return Me._Context
				End Get
			End Property
			<AccessedThroughPropertyAttribute("DrivesCar_vin")> _
			Private _DrivesCar_vin As Integer
			Public Overrides Property DrivesCar_vin() As Integer
				Get
					Return Me._DrivesCar_vin
				End Get
				Set(ByVal Value As Integer)
					Dim oldValue As Integer = Me._DrivesCar_vin
					If oldValue <> Value Then
						If Me._Context.OnPersonDrivesCarDrivesCar_vinChanging(Me, Value) AndAlso MyBase.RaiseDrivesCar_vinChangingEvent(Value) Then
							Me._DrivesCar_vin = Value
							Me._Context.OnPersonDrivesCarDrivesCar_vinChanged(Me, oldValue)
							MyBase.RaiseDrivesCar_vinChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("DrivenByPerson")> _
			Private _DrivenByPerson As Person
			Public Overrides Property DrivenByPerson() As Person
				Get
					Return Me._DrivenByPerson
				End Get
				Set(ByVal Value As Person)
					If CObj(Value) Is Nothing Then
						Throw New ArgumentNullException("value")
					End If
					Dim oldValue As Person = Me._DrivenByPerson
					If CObj(oldValue) IsNot Value Then
						If Me._Context.OnPersonDrivesCarDrivenByPersonChanging(Me, Value) AndAlso MyBase.RaiseDrivenByPersonChangingEvent(Value) Then
							Me._DrivenByPerson = Value
							Me._Context.OnPersonDrivesCarDrivenByPersonChanged(Me, oldValue)
							MyBase.RaiseDrivenByPersonChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
		End Class
		#End Region
		#End Region
		#Region "PersonBoughtCarFromPersonOnDate"
		Public Function CreatePersonBoughtCarFromPersonOnDate(ByVal CarSold_vin As Integer, ByVal SaleDate_YMD As Integer, ByVal Buyer As Person, ByVal Seller As Person) As PersonBoughtCarFromPersonOnDate Implements _
			ISampleModelContext.CreatePersonBoughtCarFromPersonOnDate
			If CObj(Buyer) Is Nothing Then
				Throw New ArgumentNullException("Buyer")
			End If
			If CObj(Seller) Is Nothing Then
				Throw New ArgumentNullException("Seller")
			End If
			If Not (Me.OnPersonBoughtCarFromPersonOnDateCarSold_vinChanging(Nothing, CarSold_vin)) Then
				Throw SampleModelContext.GetConstraintEnforcementFailedException("CarSold_vin")
			End If
			If Not (Me.OnPersonBoughtCarFromPersonOnDateSaleDate_YMDChanging(Nothing, SaleDate_YMD)) Then
				Throw SampleModelContext.GetConstraintEnforcementFailedException("SaleDate_YMD")
			End If
			If Not (Me.OnPersonBoughtCarFromPersonOnDateBuyerChanging(Nothing, Buyer)) Then
				Throw SampleModelContext.GetConstraintEnforcementFailedException("Buyer")
			End If
			If Not (Me.OnPersonBoughtCarFromPersonOnDateSellerChanging(Nothing, Seller)) Then
				Throw SampleModelContext.GetConstraintEnforcementFailedException("Seller")
			End If
			Return New PersonBoughtCarFromPersonOnDateCore(Me, CarSold_vin, SaleDate_YMD, Buyer, Seller)
		End Function
		Private Function OnPersonBoughtCarFromPersonOnDateCarSold_vinChanging(ByVal instance As PersonBoughtCarFromPersonOnDate, ByVal newValue As Integer) As Boolean
			If CObj(instance) IsNot Nothing Then
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
			If oldValue.HasValue Then
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
			If CObj(instance) IsNot Nothing Then
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
			If oldValue.HasValue Then
				InternalUniquenessConstraint24OldValueTuple = Tuple.CreateTuple(Of Integer, Person, Integer)(oldValue.Value, instance.Seller, instance.CarSold_vin)
				InternalUniquenessConstraint25OldValueTuple = Tuple.CreateTuple(Of Integer, Integer, Person)(instance.CarSold_vin, oldValue.Value, instance.Buyer)
			Else
				InternalUniquenessConstraint24OldValueTuple = Nothing
				InternalUniquenessConstraint25OldValueTuple = Nothing
			End If
			Me.OnInternalUniquenessConstraint24Changed(instance, InternalUniquenessConstraint24OldValueTuple, Tuple.CreateTuple(Of Integer, Person, Integer)(instance.SaleDate_YMD, instance.Seller, instance.CarSold_vin))
			Me.OnInternalUniquenessConstraint25Changed(instance, InternalUniquenessConstraint25OldValueTuple, Tuple.CreateTuple(Of Integer, Integer, Person)(instance.CarSold_vin, instance.SaleDate_YMD, instance.Buyer))
		End Sub
		Private Function OnPersonBoughtCarFromPersonOnDateBuyerChanging(ByVal instance As PersonBoughtCarFromPersonOnDate, ByVal newValue As Person) As Boolean
			If CObj(Me) IsNot newValue.Context Then
				Throw SampleModelContext.GetDifferentContextsException()
			End If
			If CObj(instance) IsNot Nothing Then
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
			If CObj(oldValue) IsNot Nothing Then
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
		Private Function OnPersonBoughtCarFromPersonOnDateSellerChanging(ByVal instance As PersonBoughtCarFromPersonOnDate, ByVal newValue As Person) As Boolean
			If CObj(Me) IsNot newValue.Context Then
				Throw SampleModelContext.GetDifferentContextsException()
			End If
			If CObj(instance) IsNot Nothing Then
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
			If CObj(oldValue) IsNot Nothing Then
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
		Private ReadOnly _PersonBoughtCarFromPersonOnDateList As List(Of PersonBoughtCarFromPersonOnDate)
		Private ReadOnly _PersonBoughtCarFromPersonOnDateReadOnlyCollection As ReadOnlyCollection(Of PersonBoughtCarFromPersonOnDate)
		Public ReadOnly Property PersonBoughtCarFromPersonOnDateCollection() As ReadOnlyCollection(Of PersonBoughtCarFromPersonOnDate) Implements _
			ISampleModelContext.PersonBoughtCarFromPersonOnDateCollection
			Get
				Return Me._PersonBoughtCarFromPersonOnDateReadOnlyCollection
			End Get
		End Property
		#Region "PersonBoughtCarFromPersonOnDateCore"
		<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
		Private NotInheritable Class PersonBoughtCarFromPersonOnDateCore
			Inherits PersonBoughtCarFromPersonOnDate
			Public Sub New(ByVal context As SampleModelContext, ByVal CarSold_vin As Integer, ByVal SaleDate_YMD As Integer, ByVal Buyer As Person, ByVal Seller As Person)
				Me._Context = context
				Me._CarSold_vin = CarSold_vin
				context.OnPersonBoughtCarFromPersonOnDateCarSold_vinChanged(Me, Nothing)
				Me._SaleDate_YMD = SaleDate_YMD
				context.OnPersonBoughtCarFromPersonOnDateSaleDate_YMDChanged(Me, Nothing)
				Me._Buyer = Buyer
				context.OnPersonBoughtCarFromPersonOnDateBuyerChanged(Me, Nothing)
				Me._Seller = Seller
				context.OnPersonBoughtCarFromPersonOnDateSellerChanged(Me, Nothing)
				context._PersonBoughtCarFromPersonOnDateList.Add(Me)
			End Sub
			Private ReadOnly _Context As SampleModelContext
			Public Overrides ReadOnly Property Context() As SampleModelContext
				Get
					Return Me._Context
				End Get
			End Property
			<AccessedThroughPropertyAttribute("CarSold_vin")> _
			Private _CarSold_vin As Integer
			Public Overrides Property CarSold_vin() As Integer
				Get
					Return Me._CarSold_vin
				End Get
				Set(ByVal Value As Integer)
					Dim oldValue As Integer = Me._CarSold_vin
					If oldValue <> Value Then
						If Me._Context.OnPersonBoughtCarFromPersonOnDateCarSold_vinChanging(Me, Value) AndAlso MyBase.RaiseCarSold_vinChangingEvent(Value) Then
							Me._CarSold_vin = Value
							Me._Context.OnPersonBoughtCarFromPersonOnDateCarSold_vinChanged(Me, oldValue)
							MyBase.RaiseCarSold_vinChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("SaleDate_YMD")> _
			Private _SaleDate_YMD As Integer
			Public Overrides Property SaleDate_YMD() As Integer
				Get
					Return Me._SaleDate_YMD
				End Get
				Set(ByVal Value As Integer)
					Dim oldValue As Integer = Me._SaleDate_YMD
					If oldValue <> Value Then
						If Me._Context.OnPersonBoughtCarFromPersonOnDateSaleDate_YMDChanging(Me, Value) AndAlso MyBase.RaiseSaleDate_YMDChangingEvent(Value) Then
							Me._SaleDate_YMD = Value
							Me._Context.OnPersonBoughtCarFromPersonOnDateSaleDate_YMDChanged(Me, oldValue)
							MyBase.RaiseSaleDate_YMDChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("Buyer")> _
			Private _Buyer As Person
			Public Overrides Property Buyer() As Person
				Get
					Return Me._Buyer
				End Get
				Set(ByVal Value As Person)
					If CObj(Value) Is Nothing Then
						Throw New ArgumentNullException("value")
					End If
					Dim oldValue As Person = Me._Buyer
					If CObj(oldValue) IsNot Value Then
						If Me._Context.OnPersonBoughtCarFromPersonOnDateBuyerChanging(Me, Value) AndAlso MyBase.RaiseBuyerChangingEvent(Value) Then
							Me._Buyer = Value
							Me._Context.OnPersonBoughtCarFromPersonOnDateBuyerChanged(Me, oldValue)
							MyBase.RaiseBuyerChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("Seller")> _
			Private _Seller As Person
			Public Overrides Property Seller() As Person
				Get
					Return Me._Seller
				End Get
				Set(ByVal Value As Person)
					If CObj(Value) Is Nothing Then
						Throw New ArgumentNullException("value")
					End If
					Dim oldValue As Person = Me._Seller
					If CObj(oldValue) IsNot Value Then
						If Me._Context.OnPersonBoughtCarFromPersonOnDateSellerChanging(Me, Value) AndAlso MyBase.RaiseSellerChangingEvent(Value) Then
							Me._Seller = Value
							Me._Context.OnPersonBoughtCarFromPersonOnDateSellerChanged(Me, oldValue)
							MyBase.RaiseSellerChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
		End Class
		#End Region
		#End Region
		#Region "Review"
		Public Function CreateReview(ByVal Car_vin As Integer, ByVal Rating_Nr_Integer As Integer, ByVal Criterion_Name As String) As Review Implements _
			ISampleModelContext.CreateReview
			If CObj(Criterion_Name) Is Nothing Then
				Throw New ArgumentNullException("Criterion_Name")
			End If
			If Not (Me.OnReviewCar_vinChanging(Nothing, Car_vin)) Then
				Throw SampleModelContext.GetConstraintEnforcementFailedException("Car_vin")
			End If
			If Not (Me.OnReviewRating_Nr_IntegerChanging(Nothing, Rating_Nr_Integer)) Then
				Throw SampleModelContext.GetConstraintEnforcementFailedException("Rating_Nr_Integer")
			End If
			If Not (Me.OnReviewCriterion_NameChanging(Nothing, Criterion_Name)) Then
				Throw SampleModelContext.GetConstraintEnforcementFailedException("Criterion_Name")
			End If
			Return New ReviewCore(Me, Car_vin, Rating_Nr_Integer, Criterion_Name)
		End Function
		Private Function OnReviewCar_vinChanging(ByVal instance As Review, ByVal newValue As Integer) As Boolean
			If CObj(instance) IsNot Nothing Then
				If Not (Me.OnInternalUniquenessConstraint26Changing(instance, Tuple.CreateTuple(Of Integer, String)(newValue, instance.Criterion_Name))) Then
					Return False
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnReviewCar_vinChanged(ByVal instance As Review, ByVal oldValue As Nullable(Of Integer))
			Dim InternalUniquenessConstraint26OldValueTuple As Tuple(Of Integer, String)
			If oldValue.HasValue Then
				InternalUniquenessConstraint26OldValueTuple = Tuple.CreateTuple(Of Integer, String)(oldValue.Value, instance.Criterion_Name)
			Else
				InternalUniquenessConstraint26OldValueTuple = Nothing
			End If
			Me.OnInternalUniquenessConstraint26Changed(instance, InternalUniquenessConstraint26OldValueTuple, Tuple.CreateTuple(Of Integer, String)(instance.Car_vin, instance.Criterion_Name))
		End Sub
		Private Function OnReviewRating_Nr_IntegerChanging(ByVal instance As Review, ByVal newValue As Integer) As Boolean
			Return True
		End Function
		Private Function OnReviewCriterion_NameChanging(ByVal instance As Review, ByVal newValue As String) As Boolean
			If CObj(instance) IsNot Nothing Then
				If Not (Me.OnInternalUniquenessConstraint26Changing(instance, Tuple.CreateTuple(Of Integer, String)(instance.Car_vin, newValue))) Then
					Return False
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnReviewCriterion_NameChanged(ByVal instance As Review, ByVal oldValue As String)
			Dim InternalUniquenessConstraint26OldValueTuple As Tuple(Of Integer, String)
			If CObj(oldValue) IsNot Nothing Then
				InternalUniquenessConstraint26OldValueTuple = Tuple.CreateTuple(Of Integer, String)(instance.Car_vin, oldValue)
			Else
				InternalUniquenessConstraint26OldValueTuple = Nothing
			End If
			Me.OnInternalUniquenessConstraint26Changed(instance, InternalUniquenessConstraint26OldValueTuple, Tuple.CreateTuple(Of Integer, String)(instance.Car_vin, instance.Criterion_Name))
		End Sub
		Private ReadOnly _ReviewList As List(Of Review)
		Private ReadOnly _ReviewReadOnlyCollection As ReadOnlyCollection(Of Review)
		Public ReadOnly Property ReviewCollection() As ReadOnlyCollection(Of Review) Implements _
			ISampleModelContext.ReviewCollection
			Get
				Return Me._ReviewReadOnlyCollection
			End Get
		End Property
		#Region "ReviewCore"
		<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
		Private NotInheritable Class ReviewCore
			Inherits Review
			Public Sub New(ByVal context As SampleModelContext, ByVal Car_vin As Integer, ByVal Rating_Nr_Integer As Integer, ByVal Criterion_Name As String)
				Me._Context = context
				Me._Car_vin = Car_vin
				context.OnReviewCar_vinChanged(Me, Nothing)
				Me._Rating_Nr_Integer = Rating_Nr_Integer
				Me._Criterion_Name = Criterion_Name
				context.OnReviewCriterion_NameChanged(Me, Nothing)
				context._ReviewList.Add(Me)
			End Sub
			Private ReadOnly _Context As SampleModelContext
			Public Overrides ReadOnly Property Context() As SampleModelContext
				Get
					Return Me._Context
				End Get
			End Property
			<AccessedThroughPropertyAttribute("Car_vin")> _
			Private _Car_vin As Integer
			Public Overrides Property Car_vin() As Integer
				Get
					Return Me._Car_vin
				End Get
				Set(ByVal Value As Integer)
					Dim oldValue As Integer = Me._Car_vin
					If oldValue <> Value Then
						If Me._Context.OnReviewCar_vinChanging(Me, Value) AndAlso MyBase.RaiseCar_vinChangingEvent(Value) Then
							Me._Car_vin = Value
							Me._Context.OnReviewCar_vinChanged(Me, oldValue)
							MyBase.RaiseCar_vinChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("Rating_Nr_Integer")> _
			Private _Rating_Nr_Integer As Integer
			Public Overrides Property Rating_Nr_Integer() As Integer
				Get
					Return Me._Rating_Nr_Integer
				End Get
				Set(ByVal Value As Integer)
					Dim oldValue As Integer = Me._Rating_Nr_Integer
					If oldValue <> Value Then
						If Me._Context.OnReviewRating_Nr_IntegerChanging(Me, Value) AndAlso MyBase.RaiseRating_Nr_IntegerChangingEvent(Value) Then
							Me._Rating_Nr_Integer = Value
							MyBase.RaiseRating_Nr_IntegerChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("Criterion_Name")> _
			Private _Criterion_Name As String
			Public Overrides Property Criterion_Name() As String
				Get
					Return Me._Criterion_Name
				End Get
				Set(ByVal Value As String)
					If CObj(Value) Is Nothing Then
						Throw New ArgumentNullException("value")
					End If
					Dim oldValue As String = Me._Criterion_Name
					If Not (Object.Equals(oldValue, Value)) Then
						If Me._Context.OnReviewCriterion_NameChanging(Me, Value) AndAlso MyBase.RaiseCriterion_NameChangingEvent(Value) Then
							Me._Criterion_Name = Value
							Me._Context.OnReviewCriterion_NameChanged(Me, oldValue)
							MyBase.RaiseCriterion_NameChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
		End Class
		#End Region
		#End Region
		#Region "PersonHasNickName"
		Public Function CreatePersonHasNickName(ByVal NickName As String, ByVal Person As Person) As PersonHasNickName Implements _
			ISampleModelContext.CreatePersonHasNickName
			If CObj(NickName) Is Nothing Then
				Throw New ArgumentNullException("NickName")
			End If
			If CObj(Person) Is Nothing Then
				Throw New ArgumentNullException("Person")
			End If
			If Not (Me.OnPersonHasNickNameNickNameChanging(Nothing, NickName)) Then
				Throw SampleModelContext.GetConstraintEnforcementFailedException("NickName")
			End If
			If Not (Me.OnPersonHasNickNamePersonChanging(Nothing, Person)) Then
				Throw SampleModelContext.GetConstraintEnforcementFailedException("Person")
			End If
			Return New PersonHasNickNameCore(Me, NickName, Person)
		End Function
		Private Function OnPersonHasNickNameNickNameChanging(ByVal instance As PersonHasNickName, ByVal newValue As String) As Boolean
			If CObj(instance) IsNot Nothing Then
				If Not (Me.OnInternalUniquenessConstraint33Changing(instance, Tuple.CreateTuple(Of String, Person)(newValue, instance.Person))) Then
					Return False
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnPersonHasNickNameNickNameChanged(ByVal instance As PersonHasNickName, ByVal oldValue As String)
			Dim InternalUniquenessConstraint33OldValueTuple As Tuple(Of String, Person)
			If CObj(oldValue) IsNot Nothing Then
				InternalUniquenessConstraint33OldValueTuple = Tuple.CreateTuple(Of String, Person)(oldValue, instance.Person)
			Else
				InternalUniquenessConstraint33OldValueTuple = Nothing
			End If
			Me.OnInternalUniquenessConstraint33Changed(instance, InternalUniquenessConstraint33OldValueTuple, Tuple.CreateTuple(Of String, Person)(instance.NickName, instance.Person))
		End Sub
		Private Function OnPersonHasNickNamePersonChanging(ByVal instance As PersonHasNickName, ByVal newValue As Person) As Boolean
			If CObj(Me) IsNot newValue.Context Then
				Throw SampleModelContext.GetDifferentContextsException()
			End If
			If CObj(instance) IsNot Nothing Then
				If Not (Me.OnInternalUniquenessConstraint33Changing(instance, Tuple.CreateTuple(Of String, Person)(instance.NickName, newValue))) Then
					Return False
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnPersonHasNickNamePersonChanged(ByVal instance As PersonHasNickName, ByVal oldValue As Person)
			instance.Person.PersonHasNickNameAsPerson.Add(instance)
			Dim InternalUniquenessConstraint33OldValueTuple As Tuple(Of String, Person)
			If CObj(oldValue) IsNot Nothing Then
				oldValue.PersonHasNickNameAsPerson.Remove(instance)
				InternalUniquenessConstraint33OldValueTuple = Tuple.CreateTuple(Of String, Person)(instance.NickName, oldValue)
			Else
				InternalUniquenessConstraint33OldValueTuple = Nothing
			End If
			Me.OnInternalUniquenessConstraint33Changed(instance, InternalUniquenessConstraint33OldValueTuple, Tuple.CreateTuple(Of String, Person)(instance.NickName, instance.Person))
		End Sub
		Private ReadOnly _PersonHasNickNameList As List(Of PersonHasNickName)
		Private ReadOnly _PersonHasNickNameReadOnlyCollection As ReadOnlyCollection(Of PersonHasNickName)
		Public ReadOnly Property PersonHasNickNameCollection() As ReadOnlyCollection(Of PersonHasNickName) Implements _
			ISampleModelContext.PersonHasNickNameCollection
			Get
				Return Me._PersonHasNickNameReadOnlyCollection
			End Get
		End Property
		#Region "PersonHasNickNameCore"
		<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
		Private NotInheritable Class PersonHasNickNameCore
			Inherits PersonHasNickName
			Public Sub New(ByVal context As SampleModelContext, ByVal NickName As String, ByVal Person As Person)
				Me._Context = context
				Me._NickName = NickName
				context.OnPersonHasNickNameNickNameChanged(Me, Nothing)
				Me._Person = Person
				context.OnPersonHasNickNamePersonChanged(Me, Nothing)
				context._PersonHasNickNameList.Add(Me)
			End Sub
			Private ReadOnly _Context As SampleModelContext
			Public Overrides ReadOnly Property Context() As SampleModelContext
				Get
					Return Me._Context
				End Get
			End Property
			<AccessedThroughPropertyAttribute("NickName")> _
			Private _NickName As String
			Public Overrides Property NickName() As String
				Get
					Return Me._NickName
				End Get
				Set(ByVal Value As String)
					If CObj(Value) Is Nothing Then
						Throw New ArgumentNullException("value")
					End If
					Dim oldValue As String = Me._NickName
					If Not (Object.Equals(oldValue, Value)) Then
						If Me._Context.OnPersonHasNickNameNickNameChanging(Me, Value) AndAlso MyBase.RaiseNickNameChangingEvent(Value) Then
							Me._NickName = Value
							Me._Context.OnPersonHasNickNameNickNameChanged(Me, oldValue)
							MyBase.RaiseNickNameChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("Person")> _
			Private _Person As Person
			Public Overrides Property Person() As Person
				Get
					Return Me._Person
				End Get
				Set(ByVal Value As Person)
					If CObj(Value) Is Nothing Then
						Throw New ArgumentNullException("value")
					End If
					Dim oldValue As Person = Me._Person
					If CObj(oldValue) IsNot Value Then
						If Me._Context.OnPersonHasNickNamePersonChanging(Me, Value) AndAlso MyBase.RaisePersonChangingEvent(Value) Then
							Me._Person = Value
							Me._Context.OnPersonHasNickNamePersonChanged(Me, oldValue)
							MyBase.RaisePersonChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
		End Class
		#End Region
		#End Region
		#Region "Person"
		Public Function CreatePerson(ByVal FirstName As String, ByVal Date_YMD As Integer, ByVal LastName As String, ByVal Gender_Gender_Code As String, ByVal MandatoryUniqueDecimal As Decimal, ByVal MandatoryUniqueString As String) As Person Implements _
			ISampleModelContext.CreatePerson
			If CObj(FirstName) Is Nothing Then
				Throw New ArgumentNullException("FirstName")
			End If
			If CObj(LastName) Is Nothing Then
				Throw New ArgumentNullException("LastName")
			End If
			If CObj(Gender_Gender_Code) Is Nothing Then
				Throw New ArgumentNullException("Gender_Gender_Code")
			End If
			If CObj(MandatoryUniqueString) Is Nothing Then
				Throw New ArgumentNullException("MandatoryUniqueString")
			End If
			If Not (Me.OnPersonFirstNameChanging(Nothing, FirstName)) Then
				Throw SampleModelContext.GetConstraintEnforcementFailedException("FirstName")
			End If
			If Not (Me.OnPersonDate_YMDChanging(Nothing, Date_YMD)) Then
				Throw SampleModelContext.GetConstraintEnforcementFailedException("Date_YMD")
			End If
			If Not (Me.OnPersonLastNameChanging(Nothing, LastName)) Then
				Throw SampleModelContext.GetConstraintEnforcementFailedException("LastName")
			End If
			If Not (Me.OnPersonGender_Gender_CodeChanging(Nothing, Gender_Gender_Code)) Then
				Throw SampleModelContext.GetConstraintEnforcementFailedException("Gender_Gender_Code")
			End If
			If Not (Me.OnPersonMandatoryUniqueDecimalChanging(Nothing, MandatoryUniqueDecimal)) Then
				Throw SampleModelContext.GetConstraintEnforcementFailedException("MandatoryUniqueDecimal")
			End If
			If Not (Me.OnPersonMandatoryUniqueStringChanging(Nothing, MandatoryUniqueString)) Then
				Throw SampleModelContext.GetConstraintEnforcementFailedException("MandatoryUniqueString")
			End If
			Return New PersonCore(Me, FirstName, Date_YMD, LastName, Gender_Gender_Code, MandatoryUniqueDecimal, MandatoryUniqueString)
		End Function
		Private Function OnPersonFirstNameChanging(ByVal instance As Person, ByVal newValue As String) As Boolean
			If CObj(instance) IsNot Nothing Then
				If Not (Me.OnExternalUniquenessConstraint1Changing(instance, Tuple.CreateTuple(Of String, Integer)(newValue, instance.Date_YMD))) Then
					Return False
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnPersonFirstNameChanged(ByVal instance As Person, ByVal oldValue As String)
			Dim ExternalUniquenessConstraint1OldValueTuple As Tuple(Of String, Integer)
			If CObj(oldValue) IsNot Nothing Then
				ExternalUniquenessConstraint1OldValueTuple = Tuple.CreateTuple(Of String, Integer)(oldValue, instance.Date_YMD)
			Else
				ExternalUniquenessConstraint1OldValueTuple = Nothing
			End If
			Me.OnExternalUniquenessConstraint1Changed(instance, ExternalUniquenessConstraint1OldValueTuple, Tuple.CreateTuple(Of String, Integer)(instance.FirstName, instance.Date_YMD))
		End Sub
		Private Function OnPersonDate_YMDChanging(ByVal instance As Person, ByVal newValue As Integer) As Boolean
			If CObj(instance) IsNot Nothing Then
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
			If oldValue.HasValue Then
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
			If CObj(instance) IsNot Nothing Then
				If Not (Me.OnExternalUniquenessConstraint2Changing(instance, Tuple.CreateTuple(Of String, Integer)(newValue, instance.Date_YMD))) Then
					Return False
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnPersonLastNameChanged(ByVal instance As Person, ByVal oldValue As String)
			Dim ExternalUniquenessConstraint2OldValueTuple As Tuple(Of String, Integer)
			If CObj(oldValue) IsNot Nothing Then
				ExternalUniquenessConstraint2OldValueTuple = Tuple.CreateTuple(Of String, Integer)(oldValue, instance.Date_YMD)
			Else
				ExternalUniquenessConstraint2OldValueTuple = Nothing
			End If
			Me.OnExternalUniquenessConstraint2Changed(instance, ExternalUniquenessConstraint2OldValueTuple, Tuple.CreateTuple(Of String, Integer)(instance.LastName, instance.Date_YMD))
		End Sub
		Private Function OnPersonOptionalUniqueStringChanging(ByVal instance As Person, ByVal newValue As String) As Boolean
			If CObj(newValue) IsNot Nothing Then
				Dim currentInstance As Person
				If Me._PersonOptionalUniqueStringDictionary.TryGetValue(newValue, currentInstance) Then
					If CObj(currentInstance) IsNot instance Then
						Return False
					End If
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnPersonOptionalUniqueStringChanged(ByVal instance As Person, ByVal oldValue As String)
			If CObj(instance.OptionalUniqueString) IsNot Nothing Then
				Me._PersonOptionalUniqueStringDictionary.Add(instance.OptionalUniqueString, instance)
			End If
			If CObj(oldValue) IsNot Nothing Then
				Me._PersonOptionalUniqueStringDictionary.Remove(oldValue)
			End If
		End Sub
		Private Function OnPersonHatType_ColorARGBChanging(ByVal instance As Person, ByVal newValue As Nullable(Of Integer)) As Boolean
			Return True
		End Function
		Private Function OnPersonHatType_HatTypeStyle_HatTypeStyle_DescriptionChanging(ByVal instance As Person, ByVal newValue As String) As Boolean
			Return True
		End Function
		Private Function OnPersonOwnsCar_vinChanging(ByVal instance As Person, ByVal newValue As Nullable(Of Integer)) As Boolean
			If newValue.HasValue Then
				Dim currentInstance As Person
				If Me._PersonOwnsCar_vinDictionary.TryGetValue(newValue.Value, currentInstance) Then
					If CObj(currentInstance) IsNot instance Then
						Return False
					End If
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnPersonOwnsCar_vinChanged(ByVal instance As Person, ByVal oldValue As Nullable(Of Integer))
			If instance.OwnsCar_vin.HasValue Then
				Me._PersonOwnsCar_vinDictionary.Add(instance.OwnsCar_vin.Value, instance)
			End If
			If oldValue.HasValue Then
				Me._PersonOwnsCar_vinDictionary.Remove(oldValue.Value)
			End If
		End Sub
		Private Function OnPersonGender_Gender_CodeChanging(ByVal instance As Person, ByVal newValue As String) As Boolean
			Return True
		End Function
		Private Function OnPersonPersonHasParentsChanging(ByVal instance As Person, ByVal newValue As Nullable(Of Boolean)) As Boolean
			Return True
		End Function
		Private Function OnPersonOptionalUniqueDecimalChanging(ByVal instance As Person, ByVal newValue As Nullable(Of Decimal)) As Boolean
			If newValue.HasValue Then
				Dim currentInstance As Person
				If Me._PersonOptionalUniqueDecimalDictionary.TryGetValue(newValue.Value, currentInstance) Then
					If CObj(currentInstance) IsNot instance Then
						Return False
					End If
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnPersonOptionalUniqueDecimalChanged(ByVal instance As Person, ByVal oldValue As Nullable(Of Decimal))
			If instance.OptionalUniqueDecimal.HasValue Then
				Me._PersonOptionalUniqueDecimalDictionary.Add(instance.OptionalUniqueDecimal.Value, instance)
			End If
			If oldValue.HasValue Then
				Me._PersonOptionalUniqueDecimalDictionary.Remove(oldValue.Value)
			End If
		End Sub
		Private Function OnPersonMandatoryUniqueDecimalChanging(ByVal instance As Person, ByVal newValue As Decimal) As Boolean
			Dim currentInstance As Person
			If Me._PersonMandatoryUniqueDecimalDictionary.TryGetValue(newValue, currentInstance) Then
				If CObj(currentInstance) IsNot instance Then
					Return False
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnPersonMandatoryUniqueDecimalChanged(ByVal instance As Person, ByVal oldValue As Nullable(Of Decimal))
			Me._PersonMandatoryUniqueDecimalDictionary.Add(instance.MandatoryUniqueDecimal, instance)
			If oldValue.HasValue Then
				Me._PersonMandatoryUniqueDecimalDictionary.Remove(oldValue.Value)
			End If
		End Sub
		Private Function OnPersonMandatoryUniqueStringChanging(ByVal instance As Person, ByVal newValue As String) As Boolean
			Dim currentInstance As Person
			If Me._PersonMandatoryUniqueStringDictionary.TryGetValue(newValue, currentInstance) Then
				If CObj(currentInstance) IsNot instance Then
					Return False
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnPersonMandatoryUniqueStringChanged(ByVal instance As Person, ByVal oldValue As String)
			Me._PersonMandatoryUniqueStringDictionary.Add(instance.MandatoryUniqueString, instance)
			If CObj(oldValue) IsNot Nothing Then
				Me._PersonMandatoryUniqueStringDictionary.Remove(oldValue)
			End If
		End Sub
		Private Function OnPersonValueType1DoesSomethingElseWithChanging(ByVal instance As Person, ByVal newValue As ValueType1) As Boolean
			If CObj(newValue) IsNot Nothing Then
				If CObj(Me) IsNot newValue.Context Then
					Throw SampleModelContext.GetDifferentContextsException()
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnPersonValueType1DoesSomethingElseWithChanged(ByVal instance As Person, ByVal oldValue As ValueType1)
			If CObj(instance.ValueType1DoesSomethingElseWith) IsNot Nothing Then
				instance.ValueType1DoesSomethingElseWith.DoesSomethingElseWithPerson.Add(instance)
			End If
			If CObj(oldValue) IsNot Nothing Then
				oldValue.DoesSomethingElseWithPerson.Remove(instance)
			End If
		End Sub
		Private Function OnPersonMalePersonChanging(ByVal instance As Person, ByVal newValue As MalePerson) As Boolean
			If CObj(newValue) IsNot Nothing Then
				If CObj(Me) IsNot newValue.Context Then
					Throw SampleModelContext.GetDifferentContextsException()
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnPersonMalePersonChanged(ByVal instance As Person, ByVal oldValue As MalePerson)
			If CObj(instance.MalePerson) IsNot Nothing Then
				instance.MalePerson.Person = instance
			End If
			If CObj(oldValue) IsNot Nothing Then
				oldValue.Person = Nothing
			End If
		End Sub
		Private Function OnPersonFemalePersonChanging(ByVal instance As Person, ByVal newValue As FemalePerson) As Boolean
			If CObj(newValue) IsNot Nothing Then
				If CObj(Me) IsNot newValue.Context Then
					Throw SampleModelContext.GetDifferentContextsException()
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnPersonFemalePersonChanged(ByVal instance As Person, ByVal oldValue As FemalePerson)
			If CObj(instance.FemalePerson) IsNot Nothing Then
				instance.FemalePerson.Person = instance
			End If
			If CObj(oldValue) IsNot Nothing Then
				oldValue.Person = Nothing
			End If
		End Sub
		Private Function OnPersonChildPersonChanging(ByVal instance As Person, ByVal newValue As ChildPerson) As Boolean
			If CObj(newValue) IsNot Nothing Then
				If CObj(Me) IsNot newValue.Context Then
					Throw SampleModelContext.GetDifferentContextsException()
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnPersonChildPersonChanged(ByVal instance As Person, ByVal oldValue As ChildPerson)
			If CObj(instance.ChildPerson) IsNot Nothing Then
				instance.ChildPerson.Person = instance
			End If
			If CObj(oldValue) IsNot Nothing Then
				oldValue.Person = Nothing
			End If
		End Sub
		Private Function OnPersonDeathChanging(ByVal instance As Person, ByVal newValue As Death) As Boolean
			If CObj(newValue) IsNot Nothing Then
				If CObj(Me) IsNot newValue.Context Then
					Throw SampleModelContext.GetDifferentContextsException()
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnPersonDeathChanged(ByVal instance As Person, ByVal oldValue As Death)
			If CObj(instance.Death) IsNot Nothing Then
				instance.Death.Person = instance
			End If
			If CObj(oldValue) IsNot Nothing Then
				oldValue.Person = Nothing
			End If
		End Sub
		Private Function OnPersonPersonDrivesCarAsDrivenByPersonAdding(ByVal instance As Person, ByVal value As PersonDrivesCar) As Boolean
			If CObj(Me) IsNot value.Context Then
				Throw SampleModelContext.GetDifferentContextsException()
			End If
			Return True
		End Function
		Private Sub OnPersonPersonDrivesCarAsDrivenByPersonAdded(ByVal instance As Person, ByVal value As PersonDrivesCar)
			value.DrivenByPerson = instance
		End Sub
		Private Sub OnPersonPersonDrivesCarAsDrivenByPersonRemoved(ByVal instance As Person, ByVal value As PersonDrivesCar)
			value.DrivenByPerson = Nothing
		End Sub
		Private Function OnPersonPersonBoughtCarFromPersonOnDateAsBuyerAdding(ByVal instance As Person, ByVal value As PersonBoughtCarFromPersonOnDate) As Boolean
			If CObj(Me) IsNot value.Context Then
				Throw SampleModelContext.GetDifferentContextsException()
			End If
			Return True
		End Function
		Private Sub OnPersonPersonBoughtCarFromPersonOnDateAsBuyerAdded(ByVal instance As Person, ByVal value As PersonBoughtCarFromPersonOnDate)
			value.Buyer = instance
		End Sub
		Private Sub OnPersonPersonBoughtCarFromPersonOnDateAsBuyerRemoved(ByVal instance As Person, ByVal value As PersonBoughtCarFromPersonOnDate)
			value.Buyer = Nothing
		End Sub
		Private Function OnPersonPersonBoughtCarFromPersonOnDateAsSellerAdding(ByVal instance As Person, ByVal value As PersonBoughtCarFromPersonOnDate) As Boolean
			If CObj(Me) IsNot value.Context Then
				Throw SampleModelContext.GetDifferentContextsException()
			End If
			Return True
		End Function
		Private Sub OnPersonPersonBoughtCarFromPersonOnDateAsSellerAdded(ByVal instance As Person, ByVal value As PersonBoughtCarFromPersonOnDate)
			value.Seller = instance
		End Sub
		Private Sub OnPersonPersonBoughtCarFromPersonOnDateAsSellerRemoved(ByVal instance As Person, ByVal value As PersonBoughtCarFromPersonOnDate)
			value.Seller = Nothing
		End Sub
		Private Function OnPersonPersonHasNickNameAsPersonAdding(ByVal instance As Person, ByVal value As PersonHasNickName) As Boolean
			If CObj(Me) IsNot value.Context Then
				Throw SampleModelContext.GetDifferentContextsException()
			End If
			Return True
		End Function
		Private Sub OnPersonPersonHasNickNameAsPersonAdded(ByVal instance As Person, ByVal value As PersonHasNickName)
			value.Person = instance
		End Sub
		Private Sub OnPersonPersonHasNickNameAsPersonRemoved(ByVal instance As Person, ByVal value As PersonHasNickName)
			value.Person = Nothing
		End Sub
		Private Function OnPersonTaskAdding(ByVal instance As Person, ByVal value As Task) As Boolean
			If CObj(Me) IsNot value.Context Then
				Throw SampleModelContext.GetDifferentContextsException()
			End If
			Return True
		End Function
		Private Sub OnPersonTaskAdded(ByVal instance As Person, ByVal value As Task)
			value.Person = instance
		End Sub
		Private Sub OnPersonTaskRemoved(ByVal instance As Person, ByVal value As Task)
			value.Person = Nothing
		End Sub
		Private Function OnPersonValueType1DoesSomethingWithAdding(ByVal instance As Person, ByVal value As ValueType1) As Boolean
			If CObj(Me) IsNot value.Context Then
				Throw SampleModelContext.GetDifferentContextsException()
			End If
			Return True
		End Function
		Private Sub OnPersonValueType1DoesSomethingWithAdded(ByVal instance As Person, ByVal value As ValueType1)
			value.DoesSomethingWithPerson = instance
		End Sub
		Private Sub OnPersonValueType1DoesSomethingWithRemoved(ByVal instance As Person, ByVal value As ValueType1)
			value.DoesSomethingWithPerson = Nothing
		End Sub
		Private ReadOnly _PersonList As List(Of Person)
		Private ReadOnly _PersonReadOnlyCollection As ReadOnlyCollection(Of Person)
		Public ReadOnly Property PersonCollection() As ReadOnlyCollection(Of Person) Implements _
			ISampleModelContext.PersonCollection
			Get
				Return Me._PersonReadOnlyCollection
			End Get
		End Property
		#Region "PersonCore"
		<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
		Private NotInheritable Class PersonCore
			Inherits Person
			Public Sub New(ByVal context As SampleModelContext, ByVal FirstName As String, ByVal Date_YMD As Integer, ByVal LastName As String, ByVal Gender_Gender_Code As String, ByVal MandatoryUniqueDecimal As Decimal, ByVal MandatoryUniqueString As String)
				Me._Context = context
				Me._PersonDrivesCarAsDrivenByPerson = New ConstraintEnforcementCollection(Of Person, PersonDrivesCar)(Me)
				Me._PersonBoughtCarFromPersonOnDateAsBuyer = New ConstraintEnforcementCollectionWithPropertyName(Of Person, PersonBoughtCarFromPersonOnDate)(Me, "PersonBoughtCarFromPersonOnDateAsBuyer")
				Me._PersonBoughtCarFromPersonOnDateAsSeller = New ConstraintEnforcementCollectionWithPropertyName(Of Person, PersonBoughtCarFromPersonOnDate)(Me, "PersonBoughtCarFromPersonOnDateAsSeller")
				Me._PersonHasNickNameAsPerson = New ConstraintEnforcementCollection(Of Person, PersonHasNickName)(Me)
				Me._Task = New ConstraintEnforcementCollection(Of Person, Task)(Me)
				Me._ValueType1DoesSomethingWith = New ConstraintEnforcementCollection(Of Person, ValueType1)(Me)
				Me._FirstName = FirstName
				context.OnPersonFirstNameChanged(Me, Nothing)
				Me._Date_YMD = Date_YMD
				context.OnPersonDate_YMDChanged(Me, Nothing)
				Me._LastName = LastName
				context.OnPersonLastNameChanged(Me, Nothing)
				Me._Gender_Gender_Code = Gender_Gender_Code
				Me._MandatoryUniqueDecimal = MandatoryUniqueDecimal
				context.OnPersonMandatoryUniqueDecimalChanged(Me, Nothing)
				Me._MandatoryUniqueString = MandatoryUniqueString
				context.OnPersonMandatoryUniqueStringChanged(Me, Nothing)
				context._PersonList.Add(Me)
			End Sub
			Private ReadOnly _Context As SampleModelContext
			Public Overrides ReadOnly Property Context() As SampleModelContext
				Get
					Return Me._Context
				End Get
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
							Me._Context.OnPersonFirstNameChanged(Me, oldValue)
							MyBase.RaiseFirstNameChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("Date_YMD")> _
			Private _Date_YMD As Integer
			Public Overrides Property Date_YMD() As Integer
				Get
					Return Me._Date_YMD
				End Get
				Set(ByVal Value As Integer)
					Dim oldValue As Integer = Me._Date_YMD
					If oldValue <> Value Then
						If Me._Context.OnPersonDate_YMDChanging(Me, Value) AndAlso MyBase.RaiseDate_YMDChangingEvent(Value) Then
							Me._Date_YMD = Value
							Me._Context.OnPersonDate_YMDChanged(Me, oldValue)
							MyBase.RaiseDate_YMDChangedEvent(oldValue)
						End If
					End If
				End Set
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
							Me._Context.OnPersonLastNameChanged(Me, oldValue)
							MyBase.RaiseLastNameChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("OptionalUniqueString")> _
			Private _OptionalUniqueString As String
			Public Overrides Property OptionalUniqueString() As String
				Get
					Return Me._OptionalUniqueString
				End Get
				Set(ByVal Value As String)
					Dim oldValue As String = Me._OptionalUniqueString
					If Not (Object.Equals(oldValue, Value)) Then
						If Me._Context.OnPersonOptionalUniqueStringChanging(Me, Value) AndAlso MyBase.RaiseOptionalUniqueStringChangingEvent(Value) Then
							Me._OptionalUniqueString = Value
							Me._Context.OnPersonOptionalUniqueStringChanged(Me, oldValue)
							MyBase.RaiseOptionalUniqueStringChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("HatType_ColorARGB")> _
			Private _HatType_ColorARGB As Nullable(Of Integer)
			Public Overrides Property HatType_ColorARGB() As Nullable(Of Integer)
				Get
					Return Me._HatType_ColorARGB
				End Get
				Set(ByVal Value As Nullable(Of Integer))
					Dim oldValue As Nullable(Of Integer) = Me._HatType_ColorARGB
					If (oldValue.GetValueOrDefault() <> Value.GetValueOrDefault()) OrElse (oldValue.HasValue <> Value.HasValue) Then
						If Me._Context.OnPersonHatType_ColorARGBChanging(Me, Value) AndAlso MyBase.RaiseHatType_ColorARGBChangingEvent(Value) Then
							Me._HatType_ColorARGB = Value
							MyBase.RaiseHatType_ColorARGBChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("HatType_HatTypeStyle_HatTypeStyle_Description")> _
			Private _HatType_HatTypeStyle_HatTypeStyle_Description As String
			Public Overrides Property HatType_HatTypeStyle_HatTypeStyle_Description() As String
				Get
					Return Me._HatType_HatTypeStyle_HatTypeStyle_Description
				End Get
				Set(ByVal Value As String)
					Dim oldValue As String = Me._HatType_HatTypeStyle_HatTypeStyle_Description
					If Not (Object.Equals(oldValue, Value)) Then
						If Me._Context.OnPersonHatType_HatTypeStyle_HatTypeStyle_DescriptionChanging(Me, Value) AndAlso MyBase.RaiseHatType_HatTypeStyle_HatTypeStyle_DescriptionChangingEvent(Value) Then
							Me._HatType_HatTypeStyle_HatTypeStyle_Description = Value
							MyBase.RaiseHatType_HatTypeStyle_HatTypeStyle_DescriptionChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("OwnsCar_vin")> _
			Private _OwnsCar_vin As Nullable(Of Integer)
			Public Overrides Property OwnsCar_vin() As Nullable(Of Integer)
				Get
					Return Me._OwnsCar_vin
				End Get
				Set(ByVal Value As Nullable(Of Integer))
					Dim oldValue As Nullable(Of Integer) = Me._OwnsCar_vin
					If (oldValue.GetValueOrDefault() <> Value.GetValueOrDefault()) OrElse (oldValue.HasValue <> Value.HasValue) Then
						If Me._Context.OnPersonOwnsCar_vinChanging(Me, Value) AndAlso MyBase.RaiseOwnsCar_vinChangingEvent(Value) Then
							Me._OwnsCar_vin = Value
							Me._Context.OnPersonOwnsCar_vinChanged(Me, oldValue)
							MyBase.RaiseOwnsCar_vinChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("Gender_Gender_Code")> _
			Private _Gender_Gender_Code As String
			Public Overrides Property Gender_Gender_Code() As String
				Get
					Return Me._Gender_Gender_Code
				End Get
				Set(ByVal Value As String)
					If CObj(Value) Is Nothing Then
						Throw New ArgumentNullException("value")
					End If
					Dim oldValue As String = Me._Gender_Gender_Code
					If Not (Object.Equals(oldValue, Value)) Then
						If Me._Context.OnPersonGender_Gender_CodeChanging(Me, Value) AndAlso MyBase.RaiseGender_Gender_CodeChangingEvent(Value) Then
							Me._Gender_Gender_Code = Value
							MyBase.RaiseGender_Gender_CodeChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("PersonHasParents")> _
			Private _PersonHasParents As Nullable(Of Boolean)
			Public Overrides Property PersonHasParents() As Nullable(Of Boolean)
				Get
					Return Me._PersonHasParents
				End Get
				Set(ByVal Value As Nullable(Of Boolean))
					Dim oldValue As Nullable(Of Boolean) = Me._PersonHasParents
					If (oldValue.GetValueOrDefault() <> Value.GetValueOrDefault()) OrElse (oldValue.HasValue <> Value.HasValue) Then
						If Me._Context.OnPersonPersonHasParentsChanging(Me, Value) AndAlso MyBase.RaisePersonHasParentsChangingEvent(Value) Then
							Me._PersonHasParents = Value
							MyBase.RaisePersonHasParentsChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("OptionalUniqueDecimal")> _
			Private _OptionalUniqueDecimal As Nullable(Of Decimal)
			Public Overrides Property OptionalUniqueDecimal() As Nullable(Of Decimal)
				Get
					Return Me._OptionalUniqueDecimal
				End Get
				Set(ByVal Value As Nullable(Of Decimal))
					Dim oldValue As Nullable(Of Decimal) = Me._OptionalUniqueDecimal
					If (oldValue.GetValueOrDefault() <> Value.GetValueOrDefault()) OrElse (oldValue.HasValue <> Value.HasValue) Then
						If Me._Context.OnPersonOptionalUniqueDecimalChanging(Me, Value) AndAlso MyBase.RaiseOptionalUniqueDecimalChangingEvent(Value) Then
							Me._OptionalUniqueDecimal = Value
							Me._Context.OnPersonOptionalUniqueDecimalChanged(Me, oldValue)
							MyBase.RaiseOptionalUniqueDecimalChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("MandatoryUniqueDecimal")> _
			Private _MandatoryUniqueDecimal As Decimal
			Public Overrides Property MandatoryUniqueDecimal() As Decimal
				Get
					Return Me._MandatoryUniqueDecimal
				End Get
				Set(ByVal Value As Decimal)
					Dim oldValue As Decimal = Me._MandatoryUniqueDecimal
					If oldValue <> Value Then
						If Me._Context.OnPersonMandatoryUniqueDecimalChanging(Me, Value) AndAlso MyBase.RaiseMandatoryUniqueDecimalChangingEvent(Value) Then
							Me._MandatoryUniqueDecimal = Value
							Me._Context.OnPersonMandatoryUniqueDecimalChanged(Me, oldValue)
							MyBase.RaiseMandatoryUniqueDecimalChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("MandatoryUniqueString")> _
			Private _MandatoryUniqueString As String
			Public Overrides Property MandatoryUniqueString() As String
				Get
					Return Me._MandatoryUniqueString
				End Get
				Set(ByVal Value As String)
					If CObj(Value) Is Nothing Then
						Throw New ArgumentNullException("value")
					End If
					Dim oldValue As String = Me._MandatoryUniqueString
					If Not (Object.Equals(oldValue, Value)) Then
						If Me._Context.OnPersonMandatoryUniqueStringChanging(Me, Value) AndAlso MyBase.RaiseMandatoryUniqueStringChangingEvent(Value) Then
							Me._MandatoryUniqueString = Value
							Me._Context.OnPersonMandatoryUniqueStringChanged(Me, oldValue)
							MyBase.RaiseMandatoryUniqueStringChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("ValueType1DoesSomethingElseWith")> _
			Private _ValueType1DoesSomethingElseWith As ValueType1
			Public Overrides Property ValueType1DoesSomethingElseWith() As ValueType1
				Get
					Return Me._ValueType1DoesSomethingElseWith
				End Get
				Set(ByVal Value As ValueType1)
					Dim oldValue As ValueType1 = Me._ValueType1DoesSomethingElseWith
					If CObj(oldValue) IsNot Value Then
						If Me._Context.OnPersonValueType1DoesSomethingElseWithChanging(Me, Value) AndAlso MyBase.RaiseValueType1DoesSomethingElseWithChangingEvent(Value) Then
							Me._ValueType1DoesSomethingElseWith = Value
							Me._Context.OnPersonValueType1DoesSomethingElseWithChanged(Me, oldValue)
							MyBase.RaiseValueType1DoesSomethingElseWithChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("MalePerson")> _
			Private _MalePerson As MalePerson
			Public Overrides Property MalePerson() As MalePerson
				Get
					Return Me._MalePerson
				End Get
				Set(ByVal Value As MalePerson)
					Dim oldValue As MalePerson = Me._MalePerson
					If CObj(oldValue) IsNot Value Then
						If Me._Context.OnPersonMalePersonChanging(Me, Value) AndAlso MyBase.RaiseMalePersonChangingEvent(Value) Then
							Me._MalePerson = Value
							Me._Context.OnPersonMalePersonChanged(Me, oldValue)
							MyBase.RaiseMalePersonChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("FemalePerson")> _
			Private _FemalePerson As FemalePerson
			Public Overrides Property FemalePerson() As FemalePerson
				Get
					Return Me._FemalePerson
				End Get
				Set(ByVal Value As FemalePerson)
					Dim oldValue As FemalePerson = Me._FemalePerson
					If CObj(oldValue) IsNot Value Then
						If Me._Context.OnPersonFemalePersonChanging(Me, Value) AndAlso MyBase.RaiseFemalePersonChangingEvent(Value) Then
							Me._FemalePerson = Value
							Me._Context.OnPersonFemalePersonChanged(Me, oldValue)
							MyBase.RaiseFemalePersonChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("ChildPerson")> _
			Private _ChildPerson As ChildPerson
			Public Overrides Property ChildPerson() As ChildPerson
				Get
					Return Me._ChildPerson
				End Get
				Set(ByVal Value As ChildPerson)
					Dim oldValue As ChildPerson = Me._ChildPerson
					If CObj(oldValue) IsNot Value Then
						If Me._Context.OnPersonChildPersonChanging(Me, Value) AndAlso MyBase.RaiseChildPersonChangingEvent(Value) Then
							Me._ChildPerson = Value
							Me._Context.OnPersonChildPersonChanged(Me, oldValue)
							MyBase.RaiseChildPersonChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("Death")> _
			Private _Death As Death
			Public Overrides Property Death() As Death
				Get
					Return Me._Death
				End Get
				Set(ByVal Value As Death)
					Dim oldValue As Death = Me._Death
					If CObj(oldValue) IsNot Value Then
						If Me._Context.OnPersonDeathChanging(Me, Value) AndAlso MyBase.RaiseDeathChangingEvent(Value) Then
							Me._Death = Value
							Me._Context.OnPersonDeathChanged(Me, oldValue)
							MyBase.RaiseDeathChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("PersonDrivesCarAsDrivenByPerson")> _
			Private ReadOnly _PersonDrivesCarAsDrivenByPerson As ICollection(Of PersonDrivesCar)
			Public Overrides ReadOnly Property PersonDrivesCarAsDrivenByPerson() As ICollection(Of PersonDrivesCar)
				Get
					Return Me._PersonDrivesCarAsDrivenByPerson
				End Get
			End Property
			<AccessedThroughPropertyAttribute("PersonBoughtCarFromPersonOnDateAsBuyer")> _
			Private ReadOnly _PersonBoughtCarFromPersonOnDateAsBuyer As ICollection(Of PersonBoughtCarFromPersonOnDate)
			Public Overrides ReadOnly Property PersonBoughtCarFromPersonOnDateAsBuyer() As ICollection(Of PersonBoughtCarFromPersonOnDate)
				Get
					Return Me._PersonBoughtCarFromPersonOnDateAsBuyer
				End Get
			End Property
			<AccessedThroughPropertyAttribute("PersonBoughtCarFromPersonOnDateAsSeller")> _
			Private ReadOnly _PersonBoughtCarFromPersonOnDateAsSeller As ICollection(Of PersonBoughtCarFromPersonOnDate)
			Public Overrides ReadOnly Property PersonBoughtCarFromPersonOnDateAsSeller() As ICollection(Of PersonBoughtCarFromPersonOnDate)
				Get
					Return Me._PersonBoughtCarFromPersonOnDateAsSeller
				End Get
			End Property
			<AccessedThroughPropertyAttribute("PersonHasNickNameAsPerson")> _
			Private ReadOnly _PersonHasNickNameAsPerson As ICollection(Of PersonHasNickName)
			Public Overrides ReadOnly Property PersonHasNickNameAsPerson() As ICollection(Of PersonHasNickName)
				Get
					Return Me._PersonHasNickNameAsPerson
				End Get
			End Property
			<AccessedThroughPropertyAttribute("Task")> _
			Private ReadOnly _Task As ICollection(Of Task)
			Public Overrides ReadOnly Property Task() As ICollection(Of Task)
				Get
					Return Me._Task
				End Get
			End Property
			<AccessedThroughPropertyAttribute("ValueType1DoesSomethingWith")> _
			Private ReadOnly _ValueType1DoesSomethingWith As ICollection(Of ValueType1)
			Public Overrides ReadOnly Property ValueType1DoesSomethingWith() As ICollection(Of ValueType1)
				Get
					Return Me._ValueType1DoesSomethingWith
				End Get
			End Property
		End Class
		#End Region
		#End Region
		#Region "MalePerson"
		Public Function CreateMalePerson(ByVal Person As Person) As MalePerson Implements _
			ISampleModelContext.CreateMalePerson
			If CObj(Person) Is Nothing Then
				Throw New ArgumentNullException("Person")
			End If
			If Not (Me.OnMalePersonPersonChanging(Nothing, Person)) Then
				Throw SampleModelContext.GetConstraintEnforcementFailedException("Person")
			End If
			Return New MalePersonCore(Me, Person)
		End Function
		Private Function OnMalePersonPersonChanging(ByVal instance As MalePerson, ByVal newValue As Person) As Boolean
			If CObj(Me) IsNot newValue.Context Then
				Throw SampleModelContext.GetDifferentContextsException()
			End If
			Return True
		End Function
		Private Overloads Sub OnMalePersonPersonChanged(ByVal instance As MalePerson, ByVal oldValue As Person)
			instance.Person.MalePerson = instance
			If CObj(oldValue) IsNot Nothing Then
				oldValue.MalePerson = Nothing
			End If
		End Sub
		Private Function OnMalePersonChildPersonAdding(ByVal instance As MalePerson, ByVal value As ChildPerson) As Boolean
			If CObj(Me) IsNot value.Context Then
				Throw SampleModelContext.GetDifferentContextsException()
			End If
			Return True
		End Function
		Private Sub OnMalePersonChildPersonAdded(ByVal instance As MalePerson, ByVal value As ChildPerson)
			value.Father = instance
		End Sub
		Private Sub OnMalePersonChildPersonRemoved(ByVal instance As MalePerson, ByVal value As ChildPerson)
			value.Father = Nothing
		End Sub
		Private ReadOnly _MalePersonList As List(Of MalePerson)
		Private ReadOnly _MalePersonReadOnlyCollection As ReadOnlyCollection(Of MalePerson)
		Public ReadOnly Property MalePersonCollection() As ReadOnlyCollection(Of MalePerson) Implements _
			ISampleModelContext.MalePersonCollection
			Get
				Return Me._MalePersonReadOnlyCollection
			End Get
		End Property
		#Region "MalePersonCore"
		<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
		Private NotInheritable Class MalePersonCore
			Inherits MalePerson
			Public Sub New(ByVal context As SampleModelContext, ByVal Person As Person)
				Me._Context = context
				Me._ChildPerson = New ConstraintEnforcementCollection(Of MalePerson, ChildPerson)(Me)
				Me._Person = Person
				context.OnMalePersonPersonChanged(Me, Nothing)
				context._MalePersonList.Add(Me)
			End Sub
			Private ReadOnly _Context As SampleModelContext
			Public Overrides ReadOnly Property Context() As SampleModelContext
				Get
					Return Me._Context
				End Get
			End Property
			<AccessedThroughPropertyAttribute("Person")> _
			Private _Person As Person
			Public Overrides Property Person() As Person
				Get
					Return Me._Person
				End Get
				Set(ByVal Value As Person)
					If CObj(Value) Is Nothing Then
						Throw New ArgumentNullException("value")
					End If
					Dim oldValue As Person = Me._Person
					If CObj(oldValue) IsNot Value Then
						If Me._Context.OnMalePersonPersonChanging(Me, Value) AndAlso MyBase.RaisePersonChangingEvent(Value) Then
							Me._Person = Value
							Me._Context.OnMalePersonPersonChanged(Me, oldValue)
							MyBase.RaisePersonChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("ChildPerson")> _
			Private ReadOnly _ChildPerson As ICollection(Of ChildPerson)
			Public Overrides ReadOnly Property ChildPerson() As ICollection(Of ChildPerson)
				Get
					Return Me._ChildPerson
				End Get
			End Property
		End Class
		#End Region
		#End Region
		#Region "FemalePerson"
		Public Function CreateFemalePerson(ByVal Person As Person) As FemalePerson Implements _
			ISampleModelContext.CreateFemalePerson
			If CObj(Person) Is Nothing Then
				Throw New ArgumentNullException("Person")
			End If
			If Not (Me.OnFemalePersonPersonChanging(Nothing, Person)) Then
				Throw SampleModelContext.GetConstraintEnforcementFailedException("Person")
			End If
			Return New FemalePersonCore(Me, Person)
		End Function
		Private Function OnFemalePersonPersonChanging(ByVal instance As FemalePerson, ByVal newValue As Person) As Boolean
			If CObj(Me) IsNot newValue.Context Then
				Throw SampleModelContext.GetDifferentContextsException()
			End If
			Return True
		End Function
		Private Overloads Sub OnFemalePersonPersonChanged(ByVal instance As FemalePerson, ByVal oldValue As Person)
			instance.Person.FemalePerson = instance
			If CObj(oldValue) IsNot Nothing Then
				oldValue.FemalePerson = Nothing
			End If
		End Sub
		Private Function OnFemalePersonChildPersonAdding(ByVal instance As FemalePerson, ByVal value As ChildPerson) As Boolean
			If CObj(Me) IsNot value.Context Then
				Throw SampleModelContext.GetDifferentContextsException()
			End If
			Return True
		End Function
		Private Sub OnFemalePersonChildPersonAdded(ByVal instance As FemalePerson, ByVal value As ChildPerson)
			value.Mother = instance
		End Sub
		Private Sub OnFemalePersonChildPersonRemoved(ByVal instance As FemalePerson, ByVal value As ChildPerson)
			value.Mother = Nothing
		End Sub
		Private ReadOnly _FemalePersonList As List(Of FemalePerson)
		Private ReadOnly _FemalePersonReadOnlyCollection As ReadOnlyCollection(Of FemalePerson)
		Public ReadOnly Property FemalePersonCollection() As ReadOnlyCollection(Of FemalePerson) Implements _
			ISampleModelContext.FemalePersonCollection
			Get
				Return Me._FemalePersonReadOnlyCollection
			End Get
		End Property
		#Region "FemalePersonCore"
		<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
		Private NotInheritable Class FemalePersonCore
			Inherits FemalePerson
			Public Sub New(ByVal context As SampleModelContext, ByVal Person As Person)
				Me._Context = context
				Me._ChildPerson = New ConstraintEnforcementCollection(Of FemalePerson, ChildPerson)(Me)
				Me._Person = Person
				context.OnFemalePersonPersonChanged(Me, Nothing)
				context._FemalePersonList.Add(Me)
			End Sub
			Private ReadOnly _Context As SampleModelContext
			Public Overrides ReadOnly Property Context() As SampleModelContext
				Get
					Return Me._Context
				End Get
			End Property
			<AccessedThroughPropertyAttribute("Person")> _
			Private _Person As Person
			Public Overrides Property Person() As Person
				Get
					Return Me._Person
				End Get
				Set(ByVal Value As Person)
					If CObj(Value) Is Nothing Then
						Throw New ArgumentNullException("value")
					End If
					Dim oldValue As Person = Me._Person
					If CObj(oldValue) IsNot Value Then
						If Me._Context.OnFemalePersonPersonChanging(Me, Value) AndAlso MyBase.RaisePersonChangingEvent(Value) Then
							Me._Person = Value
							Me._Context.OnFemalePersonPersonChanged(Me, oldValue)
							MyBase.RaisePersonChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("ChildPerson")> _
			Private ReadOnly _ChildPerson As ICollection(Of ChildPerson)
			Public Overrides ReadOnly Property ChildPerson() As ICollection(Of ChildPerson)
				Get
					Return Me._ChildPerson
				End Get
			End Property
		End Class
		#End Region
		#End Region
		#Region "ChildPerson"
		Public Function CreateChildPerson(ByVal BirthOrder_BirthOrder_Nr As Integer, ByVal Father As MalePerson, ByVal Mother As FemalePerson, ByVal Person As Person) As ChildPerson Implements _
			ISampleModelContext.CreateChildPerson
			If CObj(Father) Is Nothing Then
				Throw New ArgumentNullException("Father")
			End If
			If CObj(Mother) Is Nothing Then
				Throw New ArgumentNullException("Mother")
			End If
			If CObj(Person) Is Nothing Then
				Throw New ArgumentNullException("Person")
			End If
			If Not (Me.OnChildPersonBirthOrder_BirthOrder_NrChanging(Nothing, BirthOrder_BirthOrder_Nr)) Then
				Throw SampleModelContext.GetConstraintEnforcementFailedException("BirthOrder_BirthOrder_Nr")
			End If
			If Not (Me.OnChildPersonFatherChanging(Nothing, Father)) Then
				Throw SampleModelContext.GetConstraintEnforcementFailedException("Father")
			End If
			If Not (Me.OnChildPersonMotherChanging(Nothing, Mother)) Then
				Throw SampleModelContext.GetConstraintEnforcementFailedException("Mother")
			End If
			If Not (Me.OnChildPersonPersonChanging(Nothing, Person)) Then
				Throw SampleModelContext.GetConstraintEnforcementFailedException("Person")
			End If
			Return New ChildPersonCore(Me, BirthOrder_BirthOrder_Nr, Father, Mother, Person)
		End Function
		Private Function OnChildPersonBirthOrder_BirthOrder_NrChanging(ByVal instance As ChildPerson, ByVal newValue As Integer) As Boolean
			If CObj(instance) IsNot Nothing Then
				If Not (Me.OnExternalUniquenessConstraint3Changing(instance, Tuple.CreateTuple(Of MalePerson, Integer, FemalePerson)(instance.Father, newValue, instance.Mother))) Then
					Return False
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnChildPersonBirthOrder_BirthOrder_NrChanged(ByVal instance As ChildPerson, ByVal oldValue As Nullable(Of Integer))
			Dim ExternalUniquenessConstraint3OldValueTuple As Tuple(Of MalePerson, Integer, FemalePerson)
			If oldValue.HasValue Then
				ExternalUniquenessConstraint3OldValueTuple = Tuple.CreateTuple(Of MalePerson, Integer, FemalePerson)(instance.Father, oldValue.Value, instance.Mother)
			Else
				ExternalUniquenessConstraint3OldValueTuple = Nothing
			End If
			Me.OnExternalUniquenessConstraint3Changed(instance, ExternalUniquenessConstraint3OldValueTuple, Tuple.CreateTuple(Of MalePerson, Integer, FemalePerson)(instance.Father, instance.BirthOrder_BirthOrder_Nr, instance.Mother))
		End Sub
		Private Function OnChildPersonFatherChanging(ByVal instance As ChildPerson, ByVal newValue As MalePerson) As Boolean
			If CObj(Me) IsNot newValue.Context Then
				Throw SampleModelContext.GetDifferentContextsException()
			End If
			If CObj(instance) IsNot Nothing Then
				If Not (Me.OnExternalUniquenessConstraint3Changing(instance, Tuple.CreateTuple(Of MalePerson, Integer, FemalePerson)(newValue, instance.BirthOrder_BirthOrder_Nr, instance.Mother))) Then
					Return False
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnChildPersonFatherChanged(ByVal instance As ChildPerson, ByVal oldValue As MalePerson)
			instance.Father.ChildPerson.Add(instance)
			Dim ExternalUniquenessConstraint3OldValueTuple As Tuple(Of MalePerson, Integer, FemalePerson)
			If CObj(oldValue) IsNot Nothing Then
				oldValue.ChildPerson.Remove(instance)
				ExternalUniquenessConstraint3OldValueTuple = Tuple.CreateTuple(Of MalePerson, Integer, FemalePerson)(oldValue, instance.BirthOrder_BirthOrder_Nr, instance.Mother)
			Else
				ExternalUniquenessConstraint3OldValueTuple = Nothing
			End If
			Me.OnExternalUniquenessConstraint3Changed(instance, ExternalUniquenessConstraint3OldValueTuple, Tuple.CreateTuple(Of MalePerson, Integer, FemalePerson)(instance.Father, instance.BirthOrder_BirthOrder_Nr, instance.Mother))
		End Sub
		Private Function OnChildPersonMotherChanging(ByVal instance As ChildPerson, ByVal newValue As FemalePerson) As Boolean
			If CObj(Me) IsNot newValue.Context Then
				Throw SampleModelContext.GetDifferentContextsException()
			End If
			If CObj(instance) IsNot Nothing Then
				If Not (Me.OnExternalUniquenessConstraint3Changing(instance, Tuple.CreateTuple(Of MalePerson, Integer, FemalePerson)(instance.Father, instance.BirthOrder_BirthOrder_Nr, newValue))) Then
					Return False
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnChildPersonMotherChanged(ByVal instance As ChildPerson, ByVal oldValue As FemalePerson)
			instance.Mother.ChildPerson.Add(instance)
			Dim ExternalUniquenessConstraint3OldValueTuple As Tuple(Of MalePerson, Integer, FemalePerson)
			If CObj(oldValue) IsNot Nothing Then
				oldValue.ChildPerson.Remove(instance)
				ExternalUniquenessConstraint3OldValueTuple = Tuple.CreateTuple(Of MalePerson, Integer, FemalePerson)(instance.Father, instance.BirthOrder_BirthOrder_Nr, oldValue)
			Else
				ExternalUniquenessConstraint3OldValueTuple = Nothing
			End If
			Me.OnExternalUniquenessConstraint3Changed(instance, ExternalUniquenessConstraint3OldValueTuple, Tuple.CreateTuple(Of MalePerson, Integer, FemalePerson)(instance.Father, instance.BirthOrder_BirthOrder_Nr, instance.Mother))
		End Sub
		Private Function OnChildPersonPersonChanging(ByVal instance As ChildPerson, ByVal newValue As Person) As Boolean
			If CObj(Me) IsNot newValue.Context Then
				Throw SampleModelContext.GetDifferentContextsException()
			End If
			Return True
		End Function
		Private Overloads Sub OnChildPersonPersonChanged(ByVal instance As ChildPerson, ByVal oldValue As Person)
			instance.Person.ChildPerson = instance
			If CObj(oldValue) IsNot Nothing Then
				oldValue.ChildPerson = Nothing
			End If
		End Sub
		Private ReadOnly _ChildPersonList As List(Of ChildPerson)
		Private ReadOnly _ChildPersonReadOnlyCollection As ReadOnlyCollection(Of ChildPerson)
		Public ReadOnly Property ChildPersonCollection() As ReadOnlyCollection(Of ChildPerson) Implements _
			ISampleModelContext.ChildPersonCollection
			Get
				Return Me._ChildPersonReadOnlyCollection
			End Get
		End Property
		#Region "ChildPersonCore"
		<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
		Private NotInheritable Class ChildPersonCore
			Inherits ChildPerson
			Public Sub New(ByVal context As SampleModelContext, ByVal BirthOrder_BirthOrder_Nr As Integer, ByVal Father As MalePerson, ByVal Mother As FemalePerson, ByVal Person As Person)
				Me._Context = context
				Me._BirthOrder_BirthOrder_Nr = BirthOrder_BirthOrder_Nr
				context.OnChildPersonBirthOrder_BirthOrder_NrChanged(Me, Nothing)
				Me._Father = Father
				context.OnChildPersonFatherChanged(Me, Nothing)
				Me._Mother = Mother
				context.OnChildPersonMotherChanged(Me, Nothing)
				Me._Person = Person
				context.OnChildPersonPersonChanged(Me, Nothing)
				context._ChildPersonList.Add(Me)
			End Sub
			Private ReadOnly _Context As SampleModelContext
			Public Overrides ReadOnly Property Context() As SampleModelContext
				Get
					Return Me._Context
				End Get
			End Property
			<AccessedThroughPropertyAttribute("BirthOrder_BirthOrder_Nr")> _
			Private _BirthOrder_BirthOrder_Nr As Integer
			Public Overrides Property BirthOrder_BirthOrder_Nr() As Integer
				Get
					Return Me._BirthOrder_BirthOrder_Nr
				End Get
				Set(ByVal Value As Integer)
					Dim oldValue As Integer = Me._BirthOrder_BirthOrder_Nr
					If oldValue <> Value Then
						If Me._Context.OnChildPersonBirthOrder_BirthOrder_NrChanging(Me, Value) AndAlso MyBase.RaiseBirthOrder_BirthOrder_NrChangingEvent(Value) Then
							Me._BirthOrder_BirthOrder_Nr = Value
							Me._Context.OnChildPersonBirthOrder_BirthOrder_NrChanged(Me, oldValue)
							MyBase.RaiseBirthOrder_BirthOrder_NrChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("Father")> _
			Private _Father As MalePerson
			Public Overrides Property Father() As MalePerson
				Get
					Return Me._Father
				End Get
				Set(ByVal Value As MalePerson)
					If CObj(Value) Is Nothing Then
						Throw New ArgumentNullException("value")
					End If
					Dim oldValue As MalePerson = Me._Father
					If CObj(oldValue) IsNot Value Then
						If Me._Context.OnChildPersonFatherChanging(Me, Value) AndAlso MyBase.RaiseFatherChangingEvent(Value) Then
							Me._Father = Value
							Me._Context.OnChildPersonFatherChanged(Me, oldValue)
							MyBase.RaiseFatherChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("Mother")> _
			Private _Mother As FemalePerson
			Public Overrides Property Mother() As FemalePerson
				Get
					Return Me._Mother
				End Get
				Set(ByVal Value As FemalePerson)
					If CObj(Value) Is Nothing Then
						Throw New ArgumentNullException("value")
					End If
					Dim oldValue As FemalePerson = Me._Mother
					If CObj(oldValue) IsNot Value Then
						If Me._Context.OnChildPersonMotherChanging(Me, Value) AndAlso MyBase.RaiseMotherChangingEvent(Value) Then
							Me._Mother = Value
							Me._Context.OnChildPersonMotherChanged(Me, oldValue)
							MyBase.RaiseMotherChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("Person")> _
			Private _Person As Person
			Public Overrides Property Person() As Person
				Get
					Return Me._Person
				End Get
				Set(ByVal Value As Person)
					If CObj(Value) Is Nothing Then
						Throw New ArgumentNullException("value")
					End If
					Dim oldValue As Person = Me._Person
					If CObj(oldValue) IsNot Value Then
						If Me._Context.OnChildPersonPersonChanging(Me, Value) AndAlso MyBase.RaisePersonChangingEvent(Value) Then
							Me._Person = Value
							Me._Context.OnChildPersonPersonChanged(Me, oldValue)
							MyBase.RaisePersonChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
		End Class
		#End Region
		#End Region
		#Region "Death"
		Public Function CreateDeath(ByVal DeathCause_DeathCause_Type As String, ByVal Person As Person) As Death Implements _
			ISampleModelContext.CreateDeath
			If CObj(DeathCause_DeathCause_Type) Is Nothing Then
				Throw New ArgumentNullException("DeathCause_DeathCause_Type")
			End If
			If CObj(Person) Is Nothing Then
				Throw New ArgumentNullException("Person")
			End If
			If Not (Me.OnDeathDeathCause_DeathCause_TypeChanging(Nothing, DeathCause_DeathCause_Type)) Then
				Throw SampleModelContext.GetConstraintEnforcementFailedException("DeathCause_DeathCause_Type")
			End If
			If Not (Me.OnDeathPersonChanging(Nothing, Person)) Then
				Throw SampleModelContext.GetConstraintEnforcementFailedException("Person")
			End If
			Return New DeathCore(Me, DeathCause_DeathCause_Type, Person)
		End Function
		Private Function OnDeathDate_YMDChanging(ByVal instance As Death, ByVal newValue As Nullable(Of Integer)) As Boolean
			Return True
		End Function
		Private Function OnDeathDeathCause_DeathCause_TypeChanging(ByVal instance As Death, ByVal newValue As String) As Boolean
			Return True
		End Function
		Private Function OnDeathNaturalDeathChanging(ByVal instance As Death, ByVal newValue As NaturalDeath) As Boolean
			If CObj(newValue) IsNot Nothing Then
				If CObj(Me) IsNot newValue.Context Then
					Throw SampleModelContext.GetDifferentContextsException()
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnDeathNaturalDeathChanged(ByVal instance As Death, ByVal oldValue As NaturalDeath)
			If CObj(instance.NaturalDeath) IsNot Nothing Then
				instance.NaturalDeath.Death = instance
			End If
			If CObj(oldValue) IsNot Nothing Then
				oldValue.Death = Nothing
			End If
		End Sub
		Private Function OnDeathUnnaturalDeathChanging(ByVal instance As Death, ByVal newValue As UnnaturalDeath) As Boolean
			If CObj(newValue) IsNot Nothing Then
				If CObj(Me) IsNot newValue.Context Then
					Throw SampleModelContext.GetDifferentContextsException()
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnDeathUnnaturalDeathChanged(ByVal instance As Death, ByVal oldValue As UnnaturalDeath)
			If CObj(instance.UnnaturalDeath) IsNot Nothing Then
				instance.UnnaturalDeath.Death = instance
			End If
			If CObj(oldValue) IsNot Nothing Then
				oldValue.Death = Nothing
			End If
		End Sub
		Private Function OnDeathPersonChanging(ByVal instance As Death, ByVal newValue As Person) As Boolean
			If CObj(Me) IsNot newValue.Context Then
				Throw SampleModelContext.GetDifferentContextsException()
			End If
			Return True
		End Function
		Private Overloads Sub OnDeathPersonChanged(ByVal instance As Death, ByVal oldValue As Person)
			instance.Person.Death = instance
			If CObj(oldValue) IsNot Nothing Then
				oldValue.Death = Nothing
			End If
		End Sub
		Private ReadOnly _DeathList As List(Of Death)
		Private ReadOnly _DeathReadOnlyCollection As ReadOnlyCollection(Of Death)
		Public ReadOnly Property DeathCollection() As ReadOnlyCollection(Of Death) Implements _
			ISampleModelContext.DeathCollection
			Get
				Return Me._DeathReadOnlyCollection
			End Get
		End Property
		#Region "DeathCore"
		<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
		Private NotInheritable Class DeathCore
			Inherits Death
			Public Sub New(ByVal context As SampleModelContext, ByVal DeathCause_DeathCause_Type As String, ByVal Person As Person)
				Me._Context = context
				Me._DeathCause_DeathCause_Type = DeathCause_DeathCause_Type
				Me._Person = Person
				context.OnDeathPersonChanged(Me, Nothing)
				context._DeathList.Add(Me)
			End Sub
			Private ReadOnly _Context As SampleModelContext
			Public Overrides ReadOnly Property Context() As SampleModelContext
				Get
					Return Me._Context
				End Get
			End Property
			<AccessedThroughPropertyAttribute("Date_YMD")> _
			Private _Date_YMD As Nullable(Of Integer)
			Public Overrides Property Date_YMD() As Nullable(Of Integer)
				Get
					Return Me._Date_YMD
				End Get
				Set(ByVal Value As Nullable(Of Integer))
					Dim oldValue As Nullable(Of Integer) = Me._Date_YMD
					If (oldValue.GetValueOrDefault() <> Value.GetValueOrDefault()) OrElse (oldValue.HasValue <> Value.HasValue) Then
						If Me._Context.OnDeathDate_YMDChanging(Me, Value) AndAlso MyBase.RaiseDate_YMDChangingEvent(Value) Then
							Me._Date_YMD = Value
							MyBase.RaiseDate_YMDChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("DeathCause_DeathCause_Type")> _
			Private _DeathCause_DeathCause_Type As String
			Public Overrides Property DeathCause_DeathCause_Type() As String
				Get
					Return Me._DeathCause_DeathCause_Type
				End Get
				Set(ByVal Value As String)
					If CObj(Value) Is Nothing Then
						Throw New ArgumentNullException("value")
					End If
					Dim oldValue As String = Me._DeathCause_DeathCause_Type
					If Not (Object.Equals(oldValue, Value)) Then
						If Me._Context.OnDeathDeathCause_DeathCause_TypeChanging(Me, Value) AndAlso MyBase.RaiseDeathCause_DeathCause_TypeChangingEvent(Value) Then
							Me._DeathCause_DeathCause_Type = Value
							MyBase.RaiseDeathCause_DeathCause_TypeChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("NaturalDeath")> _
			Private _NaturalDeath As NaturalDeath
			Public Overrides Property NaturalDeath() As NaturalDeath
				Get
					Return Me._NaturalDeath
				End Get
				Set(ByVal Value As NaturalDeath)
					Dim oldValue As NaturalDeath = Me._NaturalDeath
					If CObj(oldValue) IsNot Value Then
						If Me._Context.OnDeathNaturalDeathChanging(Me, Value) AndAlso MyBase.RaiseNaturalDeathChangingEvent(Value) Then
							Me._NaturalDeath = Value
							Me._Context.OnDeathNaturalDeathChanged(Me, oldValue)
							MyBase.RaiseNaturalDeathChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("UnnaturalDeath")> _
			Private _UnnaturalDeath As UnnaturalDeath
			Public Overrides Property UnnaturalDeath() As UnnaturalDeath
				Get
					Return Me._UnnaturalDeath
				End Get
				Set(ByVal Value As UnnaturalDeath)
					Dim oldValue As UnnaturalDeath = Me._UnnaturalDeath
					If CObj(oldValue) IsNot Value Then
						If Me._Context.OnDeathUnnaturalDeathChanging(Me, Value) AndAlso MyBase.RaiseUnnaturalDeathChangingEvent(Value) Then
							Me._UnnaturalDeath = Value
							Me._Context.OnDeathUnnaturalDeathChanged(Me, oldValue)
							MyBase.RaiseUnnaturalDeathChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("Person")> _
			Private _Person As Person
			Public Overrides Property Person() As Person
				Get
					Return Me._Person
				End Get
				Set(ByVal Value As Person)
					If CObj(Value) Is Nothing Then
						Throw New ArgumentNullException("value")
					End If
					Dim oldValue As Person = Me._Person
					If CObj(oldValue) IsNot Value Then
						If Me._Context.OnDeathPersonChanging(Me, Value) AndAlso MyBase.RaisePersonChangingEvent(Value) Then
							Me._Person = Value
							Me._Context.OnDeathPersonChanged(Me, oldValue)
							MyBase.RaisePersonChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
		End Class
		#End Region
		#End Region
		#Region "NaturalDeath"
		Public Function CreateNaturalDeath(ByVal Death As Death) As NaturalDeath Implements _
			ISampleModelContext.CreateNaturalDeath
			If CObj(Death) Is Nothing Then
				Throw New ArgumentNullException("Death")
			End If
			If Not (Me.OnNaturalDeathDeathChanging(Nothing, Death)) Then
				Throw SampleModelContext.GetConstraintEnforcementFailedException("Death")
			End If
			Return New NaturalDeathCore(Me, Death)
		End Function
		Private Function OnNaturalDeathNaturalDeathIsFromProstateCancerChanging(ByVal instance As NaturalDeath, ByVal newValue As Nullable(Of Boolean)) As Boolean
			Return True
		End Function
		Private Function OnNaturalDeathDeathChanging(ByVal instance As NaturalDeath, ByVal newValue As Death) As Boolean
			If CObj(Me) IsNot newValue.Context Then
				Throw SampleModelContext.GetDifferentContextsException()
			End If
			Return True
		End Function
		Private Overloads Sub OnNaturalDeathDeathChanged(ByVal instance As NaturalDeath, ByVal oldValue As Death)
			instance.Death.NaturalDeath = instance
			If CObj(oldValue) IsNot Nothing Then
				oldValue.NaturalDeath = Nothing
			End If
		End Sub
		Private ReadOnly _NaturalDeathList As List(Of NaturalDeath)
		Private ReadOnly _NaturalDeathReadOnlyCollection As ReadOnlyCollection(Of NaturalDeath)
		Public ReadOnly Property NaturalDeathCollection() As ReadOnlyCollection(Of NaturalDeath) Implements _
			ISampleModelContext.NaturalDeathCollection
			Get
				Return Me._NaturalDeathReadOnlyCollection
			End Get
		End Property
		#Region "NaturalDeathCore"
		<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
		Private NotInheritable Class NaturalDeathCore
			Inherits NaturalDeath
			Public Sub New(ByVal context As SampleModelContext, ByVal Death As Death)
				Me._Context = context
				Me._Death = Death
				context.OnNaturalDeathDeathChanged(Me, Nothing)
				context._NaturalDeathList.Add(Me)
			End Sub
			Private ReadOnly _Context As SampleModelContext
			Public Overrides ReadOnly Property Context() As SampleModelContext
				Get
					Return Me._Context
				End Get
			End Property
			<AccessedThroughPropertyAttribute("NaturalDeathIsFromProstateCancer")> _
			Private _NaturalDeathIsFromProstateCancer As Nullable(Of Boolean)
			Public Overrides Property NaturalDeathIsFromProstateCancer() As Nullable(Of Boolean)
				Get
					Return Me._NaturalDeathIsFromProstateCancer
				End Get
				Set(ByVal Value As Nullable(Of Boolean))
					Dim oldValue As Nullable(Of Boolean) = Me._NaturalDeathIsFromProstateCancer
					If (oldValue.GetValueOrDefault() <> Value.GetValueOrDefault()) OrElse (oldValue.HasValue <> Value.HasValue) Then
						If Me._Context.OnNaturalDeathNaturalDeathIsFromProstateCancerChanging(Me, Value) AndAlso MyBase.RaiseNaturalDeathIsFromProstateCancerChangingEvent(Value) Then
							Me._NaturalDeathIsFromProstateCancer = Value
							MyBase.RaiseNaturalDeathIsFromProstateCancerChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("Death")> _
			Private _Death As Death
			Public Overrides Property Death() As Death
				Get
					Return Me._Death
				End Get
				Set(ByVal Value As Death)
					If CObj(Value) Is Nothing Then
						Throw New ArgumentNullException("value")
					End If
					Dim oldValue As Death = Me._Death
					If CObj(oldValue) IsNot Value Then
						If Me._Context.OnNaturalDeathDeathChanging(Me, Value) AndAlso MyBase.RaiseDeathChangingEvent(Value) Then
							Me._Death = Value
							Me._Context.OnNaturalDeathDeathChanged(Me, oldValue)
							MyBase.RaiseDeathChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
		End Class
		#End Region
		#End Region
		#Region "UnnaturalDeath"
		Public Function CreateUnnaturalDeath(ByVal Death As Death) As UnnaturalDeath Implements _
			ISampleModelContext.CreateUnnaturalDeath
			If CObj(Death) Is Nothing Then
				Throw New ArgumentNullException("Death")
			End If
			If Not (Me.OnUnnaturalDeathDeathChanging(Nothing, Death)) Then
				Throw SampleModelContext.GetConstraintEnforcementFailedException("Death")
			End If
			Return New UnnaturalDeathCore(Me, Death)
		End Function
		Private Function OnUnnaturalDeathUnnaturalDeathIsViolentChanging(ByVal instance As UnnaturalDeath, ByVal newValue As Nullable(Of Boolean)) As Boolean
			Return True
		End Function
		Private Function OnUnnaturalDeathUnnaturalDeathIsBloodyChanging(ByVal instance As UnnaturalDeath, ByVal newValue As Nullable(Of Boolean)) As Boolean
			Return True
		End Function
		Private Function OnUnnaturalDeathDeathChanging(ByVal instance As UnnaturalDeath, ByVal newValue As Death) As Boolean
			If CObj(Me) IsNot newValue.Context Then
				Throw SampleModelContext.GetDifferentContextsException()
			End If
			Return True
		End Function
		Private Overloads Sub OnUnnaturalDeathDeathChanged(ByVal instance As UnnaturalDeath, ByVal oldValue As Death)
			instance.Death.UnnaturalDeath = instance
			If CObj(oldValue) IsNot Nothing Then
				oldValue.UnnaturalDeath = Nothing
			End If
		End Sub
		Private ReadOnly _UnnaturalDeathList As List(Of UnnaturalDeath)
		Private ReadOnly _UnnaturalDeathReadOnlyCollection As ReadOnlyCollection(Of UnnaturalDeath)
		Public ReadOnly Property UnnaturalDeathCollection() As ReadOnlyCollection(Of UnnaturalDeath) Implements _
			ISampleModelContext.UnnaturalDeathCollection
			Get
				Return Me._UnnaturalDeathReadOnlyCollection
			End Get
		End Property
		#Region "UnnaturalDeathCore"
		<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
		Private NotInheritable Class UnnaturalDeathCore
			Inherits UnnaturalDeath
			Public Sub New(ByVal context As SampleModelContext, ByVal Death As Death)
				Me._Context = context
				Me._Death = Death
				context.OnUnnaturalDeathDeathChanged(Me, Nothing)
				context._UnnaturalDeathList.Add(Me)
			End Sub
			Private ReadOnly _Context As SampleModelContext
			Public Overrides ReadOnly Property Context() As SampleModelContext
				Get
					Return Me._Context
				End Get
			End Property
			<AccessedThroughPropertyAttribute("UnnaturalDeathIsViolent")> _
			Private _UnnaturalDeathIsViolent As Nullable(Of Boolean)
			Public Overrides Property UnnaturalDeathIsViolent() As Nullable(Of Boolean)
				Get
					Return Me._UnnaturalDeathIsViolent
				End Get
				Set(ByVal Value As Nullable(Of Boolean))
					Dim oldValue As Nullable(Of Boolean) = Me._UnnaturalDeathIsViolent
					If (oldValue.GetValueOrDefault() <> Value.GetValueOrDefault()) OrElse (oldValue.HasValue <> Value.HasValue) Then
						If Me._Context.OnUnnaturalDeathUnnaturalDeathIsViolentChanging(Me, Value) AndAlso MyBase.RaiseUnnaturalDeathIsViolentChangingEvent(Value) Then
							Me._UnnaturalDeathIsViolent = Value
							MyBase.RaiseUnnaturalDeathIsViolentChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("UnnaturalDeathIsBloody")> _
			Private _UnnaturalDeathIsBloody As Nullable(Of Boolean)
			Public Overrides Property UnnaturalDeathIsBloody() As Nullable(Of Boolean)
				Get
					Return Me._UnnaturalDeathIsBloody
				End Get
				Set(ByVal Value As Nullable(Of Boolean))
					Dim oldValue As Nullable(Of Boolean) = Me._UnnaturalDeathIsBloody
					If (oldValue.GetValueOrDefault() <> Value.GetValueOrDefault()) OrElse (oldValue.HasValue <> Value.HasValue) Then
						If Me._Context.OnUnnaturalDeathUnnaturalDeathIsBloodyChanging(Me, Value) AndAlso MyBase.RaiseUnnaturalDeathIsBloodyChangingEvent(Value) Then
							Me._UnnaturalDeathIsBloody = Value
							MyBase.RaiseUnnaturalDeathIsBloodyChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("Death")> _
			Private _Death As Death
			Public Overrides Property Death() As Death
				Get
					Return Me._Death
				End Get
				Set(ByVal Value As Death)
					If CObj(Value) Is Nothing Then
						Throw New ArgumentNullException("value")
					End If
					Dim oldValue As Death = Me._Death
					If CObj(oldValue) IsNot Value Then
						If Me._Context.OnUnnaturalDeathDeathChanging(Me, Value) AndAlso MyBase.RaiseDeathChangingEvent(Value) Then
							Me._Death = Value
							Me._Context.OnUnnaturalDeathDeathChanged(Me, oldValue)
							MyBase.RaiseDeathChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
		End Class
		#End Region
		#End Region
		#Region "Task"
		Public Function CreateTask() As Task Implements _
			ISampleModelContext.CreateTask
			Return New TaskCore(Me)
		End Function
		Private Function OnTaskPersonChanging(ByVal instance As Task, ByVal newValue As Person) As Boolean
			If CObj(newValue) IsNot Nothing Then
				If CObj(Me) IsNot newValue.Context Then
					Throw SampleModelContext.GetDifferentContextsException()
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnTaskPersonChanged(ByVal instance As Task, ByVal oldValue As Person)
			If CObj(instance.Person) IsNot Nothing Then
				instance.Person.Task.Add(instance)
			End If
			If CObj(oldValue) IsNot Nothing Then
				oldValue.Task.Remove(instance)
			End If
		End Sub
		Private ReadOnly _TaskList As List(Of Task)
		Private ReadOnly _TaskReadOnlyCollection As ReadOnlyCollection(Of Task)
		Public ReadOnly Property TaskCollection() As ReadOnlyCollection(Of Task) Implements _
			ISampleModelContext.TaskCollection
			Get
				Return Me._TaskReadOnlyCollection
			End Get
		End Property
		#Region "TaskCore"
		<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
		Private NotInheritable Class TaskCore
			Inherits Task
			Public Sub New(ByVal context As SampleModelContext)
				Me._Context = context
				context._TaskList.Add(Me)
			End Sub
			Private ReadOnly _Context As SampleModelContext
			Public Overrides ReadOnly Property Context() As SampleModelContext
				Get
					Return Me._Context
				End Get
			End Property
			<AccessedThroughPropertyAttribute("Person")> _
			Private _Person As Person
			Public Overrides Property Person() As Person
				Get
					Return Me._Person
				End Get
				Set(ByVal Value As Person)
					Dim oldValue As Person = Me._Person
					If CObj(oldValue) IsNot Value Then
						If Me._Context.OnTaskPersonChanging(Me, Value) AndAlso MyBase.RaisePersonChangingEvent(Value) Then
							Me._Person = Value
							Me._Context.OnTaskPersonChanged(Me, oldValue)
							MyBase.RaisePersonChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
		End Class
		#End Region
		#End Region
		#Region "ValueType1"
		Public Function CreateValueType1(ByVal ValueType1Value As Integer) As ValueType1 Implements _
			ISampleModelContext.CreateValueType1
			If Not (Me.OnValueType1ValueType1ValueChanging(Nothing, ValueType1Value)) Then
				Throw SampleModelContext.GetConstraintEnforcementFailedException("ValueType1Value")
			End If
			Return New ValueType1Core(Me, ValueType1Value)
		End Function
		Private Function OnValueType1ValueType1ValueChanging(ByVal instance As ValueType1, ByVal newValue As Integer) As Boolean
			Dim currentInstance As ValueType1
			If Me._ValueType1ValueType1ValueDictionary.TryGetValue(newValue, currentInstance) Then
				If CObj(currentInstance) IsNot instance Then
					Return False
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnValueType1ValueType1ValueChanged(ByVal instance As ValueType1, ByVal oldValue As Nullable(Of Integer))
			Me._ValueType1ValueType1ValueDictionary.Add(instance.ValueType1Value, instance)
			If oldValue.HasValue Then
				Me._ValueType1ValueType1ValueDictionary.Remove(oldValue.Value)
			End If
		End Sub
		Private Function OnValueType1DoesSomethingWithPersonChanging(ByVal instance As ValueType1, ByVal newValue As Person) As Boolean
			If CObj(newValue) IsNot Nothing Then
				If CObj(Me) IsNot newValue.Context Then
					Throw SampleModelContext.GetDifferentContextsException()
				End If
			End If
			Return True
		End Function
		Private Overloads Sub OnValueType1DoesSomethingWithPersonChanged(ByVal instance As ValueType1, ByVal oldValue As Person)
			If CObj(instance.DoesSomethingWithPerson) IsNot Nothing Then
				instance.DoesSomethingWithPerson.ValueType1DoesSomethingWith.Add(instance)
			End If
			If CObj(oldValue) IsNot Nothing Then
				oldValue.ValueType1DoesSomethingWith.Remove(instance)
			End If
		End Sub
		Private Function OnValueType1DoesSomethingElseWithPersonAdding(ByVal instance As ValueType1, ByVal value As Person) As Boolean
			If CObj(Me) IsNot value.Context Then
				Throw SampleModelContext.GetDifferentContextsException()
			End If
			Return True
		End Function
		Private Sub OnValueType1DoesSomethingElseWithPersonAdded(ByVal instance As ValueType1, ByVal value As Person)
			value.ValueType1DoesSomethingElseWith = instance
		End Sub
		Private Sub OnValueType1DoesSomethingElseWithPersonRemoved(ByVal instance As ValueType1, ByVal value As Person)
			value.ValueType1DoesSomethingElseWith = Nothing
		End Sub
		Private ReadOnly _ValueType1List As List(Of ValueType1)
		Private ReadOnly _ValueType1ReadOnlyCollection As ReadOnlyCollection(Of ValueType1)
		Public ReadOnly Property ValueType1Collection() As ReadOnlyCollection(Of ValueType1) Implements _
			ISampleModelContext.ValueType1Collection
			Get
				Return Me._ValueType1ReadOnlyCollection
			End Get
		End Property
		#Region "ValueType1Core"
		<StructLayoutAttribute(LayoutKind.Auto, CharSet:=CharSet.Auto)> _
		Private NotInheritable Class ValueType1Core
			Inherits ValueType1
			Public Sub New(ByVal context As SampleModelContext, ByVal ValueType1Value As Integer)
				Me._Context = context
				Me._DoesSomethingElseWithPerson = New ConstraintEnforcementCollection(Of ValueType1, Person)(Me)
				Me._ValueType1Value = ValueType1Value
				context.OnValueType1ValueType1ValueChanged(Me, Nothing)
				context._ValueType1List.Add(Me)
			End Sub
			Private ReadOnly _Context As SampleModelContext
			Public Overrides ReadOnly Property Context() As SampleModelContext
				Get
					Return Me._Context
				End Get
			End Property
			<AccessedThroughPropertyAttribute("ValueType1Value")> _
			Private _ValueType1Value As Integer
			Public Overrides Property ValueType1Value() As Integer
				Get
					Return Me._ValueType1Value
				End Get
				Set(ByVal Value As Integer)
					Dim oldValue As Integer = Me._ValueType1Value
					If oldValue <> Value Then
						If Me._Context.OnValueType1ValueType1ValueChanging(Me, Value) AndAlso MyBase.RaiseValueType1ValueChangingEvent(Value) Then
							Me._ValueType1Value = Value
							Me._Context.OnValueType1ValueType1ValueChanged(Me, oldValue)
							MyBase.RaiseValueType1ValueChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("DoesSomethingWithPerson")> _
			Private _DoesSomethingWithPerson As Person
			Public Overrides Property DoesSomethingWithPerson() As Person
				Get
					Return Me._DoesSomethingWithPerson
				End Get
				Set(ByVal Value As Person)
					Dim oldValue As Person = Me._DoesSomethingWithPerson
					If CObj(oldValue) IsNot Value Then
						If Me._Context.OnValueType1DoesSomethingWithPersonChanging(Me, Value) AndAlso MyBase.RaiseDoesSomethingWithPersonChangingEvent(Value) Then
							Me._DoesSomethingWithPerson = Value
							Me._Context.OnValueType1DoesSomethingWithPersonChanged(Me, oldValue)
							MyBase.RaiseDoesSomethingWithPersonChangedEvent(oldValue)
						End If
					End If
				End Set
			End Property
			<AccessedThroughPropertyAttribute("DoesSomethingElseWithPerson")> _
			Private ReadOnly _DoesSomethingElseWithPerson As ICollection(Of Person)
			Public Overrides ReadOnly Property DoesSomethingElseWithPerson() As ICollection(Of Person)
				Get
					Return Me._DoesSomethingElseWithPerson
				End Get
			End Property
		End Class
		#End Region
		#End Region
	End Class
	#End Region
End Namespace
