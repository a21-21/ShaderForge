using UnityEngine;

namespace ShaderForge {

	[System.Serializable]
	public class SFN_Hue : SF_Node {

		public SFN_Hue() {
		}

		public override void Initialize() {
			base.Initialize( "Hue" );
			base.showColor = true;
			UseLowerReadonlyValues( true );
			connectors = new SF_NodeConnector[]{
				SF_NodeConnector.Create( this, "OUT", "", ConType.cOutput, ValueType.VTv3, false ),
				SF_NodeConnector.Create( this, "IN", "", ConType.cInput, ValueType.VTv1, false ).SetRequired( true )};
		}

		public override int GetEvaluatedComponentCount() {
			return 3;
		}

		public override bool IsUniformOutput() {

			if(GetInputIsConnected("IN")){
				return GetInputData( "IN" ).uniform;
			}
			return true;
		}

		public override string Evaluate( OutChannel channel = OutChannel.All ) {
			string v = GetConnectorByStringID( "IN" ).TryEvaluate();
			return "saturate(3.0*abs(1.0-2.0*frac("+v+"+float3(1.0,-1.0/3.0,1.0/3.0)))-1)";
		}

		static Vector3 offsets = new Vector3(1f,-1f/3f, 1f/3f);

		public override float NodeOperator( int x, int y, int c ) {
			if(c == 3)
				return 1f;
			float v = GetInputData( "IN", x, y, c );
			float o = offsets[c];
			return Mathf.Clamp01(3 * Mathf.Abs(1-2*Frac(v+o)) - 1);
		}

		float Frac( float x ) {
			return x - Mathf.Floor( x );
			
		}

		public override void RefreshValue() {
			RefreshValue( 1, 1 );
		}

	}
}