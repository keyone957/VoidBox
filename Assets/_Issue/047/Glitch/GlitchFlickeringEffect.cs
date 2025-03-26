using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GlitchFlickeringEffect : MonoBehaviour
{
    public Material glitchMaterial;  // 쉐이더가 적용된 Material을 에디터에서 할당해야 합니다.

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (glitchMaterial != null)
        {
            Graphics.Blit(src, dest, glitchMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);  // Material이 없으면 원본 이미지를 그대로 출력합니다.
        }
    }
}
