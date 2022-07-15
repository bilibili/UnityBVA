using System;

namespace GLTF.Math
{
	public struct Vector3Int : IEquatable<Vector3Int>
	{
		public static readonly Vector3Int Zero = new Vector3Int(0, 0, 0);
		public static readonly Vector3Int One = new Vector3Int(1, 1, 1);

		public int X { get; set; }
		public int Y { get; set; }
		public int Z { get; set; }
		
		public Vector3Int(int x, int y, int z)
		{
			X = x;
			Y = y;
			Z = z;
		}
		

		public Vector3Int(Vector3Int other)
		{
			X = other.X;
			Y = other.Y;
			Z = other.Z;
		}

		public bool Equals(Vector3Int other)
		{
			return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is Vector3Int && Equals((Vector3Int) obj);
		}

		public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Vector3Int left, Vector3Int right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Vector3Int left, Vector3Int right)
		{
			return !left.Equals(right);
		}
	}
}
