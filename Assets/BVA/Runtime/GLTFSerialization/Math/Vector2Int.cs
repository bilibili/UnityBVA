using System;

namespace GLTF.Math
{
	public struct Vector2Int : IEquatable<Vector2Int>
	{
		public int X { get; set; }
		public int Y { get; set; }
	
		public Vector2Int(int x, int y)
		{
			X = x;
			Y = y;
		}

		public Vector2Int(Vector2Int other)
		{
			X = other.X;
			Y = other.Y;
		}

		public bool Equals(Vector2Int other)
		{
			return X.Equals(other.X) && Y.Equals(other.Y);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is Vector2Int && Equals((Vector2Int) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (X.GetHashCode() * 397) ^ Y.GetHashCode();
			}
		}

		public static bool operator ==(Vector2Int left, Vector2Int right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Vector2Int left, Vector2Int right)
		{
			return !left.Equals(right);
		}
	}
}
