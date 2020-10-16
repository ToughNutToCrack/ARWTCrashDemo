using UnityEngine;

[ExecuteInEditMode]
public class GameRenderer : MonoBehaviour{
    public Vector3 debugBox;
    public bool useDebug;
     
    void Update(){

        Shader.SetGlobalVector("_Center", transform.position);
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Shader.SetGlobalMatrix("_RotationMatrix", rotationMatrix.inverse);
        Shader.SetGlobalVector("_Dimensions", transform.lossyScale);
        
        if(useDebug){
            Shader.SetGlobalVector("_Dimensions", debugBox);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.matrix = rotationMatrix;

        if(useDebug){
            Gizmos.DrawWireCube(Vector3.zero, debugBox);
        }else{
            Gizmos.DrawWireCube (Vector3.zero, transform.lossyScale);
        }
    }
}
