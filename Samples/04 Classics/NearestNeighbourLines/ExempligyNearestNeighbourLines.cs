/*
	Copyright © Carl Emil Carlsen 2023-2024
	http://cec.dk
*/

using UnityEngine;
using static Plot;

namespace PlotExamples
{
	[ExecuteInEditMode]
	public class ExempligyNearestNeighbourLines : MonoBehaviour
	{
		public bool showLines = false;
		public Color backgroundColor = Color.black;

		Vector2[] _pos, _vel;

		RenderTexture _rt;

		const int width = 3840;
		const int height = 2160;
		const int count = 16;
		const float strokeWidth = 2; // Px
		const float distMin = 300; // Px
		const float distMax = 800; // Px


		void OnEnable()
		{
			_rt = new RenderTexture( width, height, 16, RenderTextureFormat.ARGB32, mipCount: 4 ){
				useMipMap = true
			};
			ClearRenderTextureNow( _rt, backgroundColor );

			_pos = new Vector2[ count ];
			_vel = new Vector2[ count ];
			for( int i = 0; i < count; i++ ) {
				_pos[ i ] = new Vector2( Random.value * width, Random.value * height );
				_vel[ i ] = Random.insideUnitCircle.normalized; // Px
			}
		}


		void OnDisable()
		{
			if( _rt ) _rt.Release();
			_rt = null;
		}


		void Update()
		{
			MoveAndWrapPositions();
			if( showLines ) ClearRenderTextureNow( _rt, backgroundColor );
			DrawToRenderTexture();
			DrawRenderTexture();
		}


		void MoveAndWrapPositions()
		{
			for( int i = 0; i < count; i++ )
			{
				_pos[ i ] += _vel[ i ];
				_pos[ i ].Set(
					Mathf.Repeat( _pos[ i ].x+distMax, width + distMax*2 )-distMax, 
					Mathf.Repeat( _pos[ i ].y+distMax, height + distMax*2 )-distMax
				);
			}
		}


		void DrawToRenderTexture()
		{
			PushCanvasAndStyle();
			BeginDrawNowToRenderTexture( _rt, Plot.Space.Pixels );

			SetBlend( Blend.TransparentAdditive );
			SetStrokeWidth( strokeWidth );
			float distMinSqr = distMin * distMin;
			float distMaxSqr = distMax * distMax;
			for( int ia = 0; ia < count; ia++ ) {
				for( int ib = 0; ib < ia; ib++ ) {
					float distSqr = ( _pos[ ib ] - _pos[ ia ] ).sqrMagnitude;
					if( distSqr < distMaxSqr ){
						SetStrokeColor( Color.white, Mathf.InverseLerp( distMaxSqr, distMinSqr, distSqr ) * 0.01f );
						DrawLineNow( _pos[ ia ], _pos[ ib ] );
					}
				}
			}

			EndDrawNowToRenderTexture();
			PopCanvasAndStyle();
		}


		void DrawRenderTexture()
		{
			PushCanvasAndStyle();

			SetNoStroke();
			SetFillColor( backgroundColor );
			SetFillTexture( _rt );
			DrawRect( 0f, 0f, width / (float) height, 1f );

			PopCanvasAndStyle();
		}
	}
}