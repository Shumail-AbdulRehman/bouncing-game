using UnityEngine;

// A compact bottom toolbar for comparing two real Rigidbody experiments.
public class ExperimentPanel : MonoBehaviour
{
    public DropExperiment[] experiments;
    bool stopped;
    GUIStyle heading, text, control;

    void OnGUI()
    {
        if (heading == null)
        {
            heading = new GUIStyle(GUI.skin.label) { fontSize = 21, fontStyle = FontStyle.Bold };
            text = new GUIStyle(GUI.skin.label) { fontSize = 13 };
            control = new GUIStyle(GUI.skin.button) { fontSize = 14 };
            heading.normal.textColor = new Color(1, 0.89f, 0.62f);
            text.normal.textColor = Color.white;
        }
        float factor = Mathf.Min(Screen.width / 1200f, Screen.height / 750f);
        GUI.matrix = Matrix4x4.Scale(new Vector3(factor, factor, 1));
        float width = Screen.width / factor;
        float top = Screen.height / factor - 106;
        GUI.color = new Color(0.12f, 0.18f, 0.22f, 0.96f);
        GUI.DrawTexture(new Rect(18, top, width - 36, 88), Texture2D.whiteTexture);
        GUI.color = Color.white;
        GUI.Label(new Rect(36, top + 10, 230, 30), "CRESCENT PARK", heading);
        GUI.Label(new Rect(36, top + 44, 230, 24), "Two balls. Two bounce settings.", text);
        for (int i = 0; i < experiments.Length; i++)
        {
            var ball = experiments[i];
            float x = 290 + i * 270;
            GUI.Label(new Rect(x, top + 14, 260, 25), ball.displayName, text);
            GUI.Label(new Rect(x, top + 43, 260, 25), $"Height {ball.Height:F2} m  |  Impacts {ball.contactCount}", text);
        }
        if (GUI.Button(new Rect(width - 286, top + 22, 122, 40), "Release again", control))
        {
            stopped = false;
            Time.timeScale = 1;
            foreach (var ball in experiments) ball.ReleaseAgain();
        }
        if (GUI.Button(new Rect(width - 152, top + 22, 116, 40), stopped ? "Continue" : "Freeze", control))
        {
            stopped = !stopped;
            Time.timeScale = stopped ? 0 : 1;
        }
    }

    void OnDisable() { Time.timeScale = 1; }
}
