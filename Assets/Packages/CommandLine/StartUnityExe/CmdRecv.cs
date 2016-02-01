using UnityEngine;
using System.Collections;

namespace CmdEditor
{
    public class CmdRecv : MonoBehaviour {

	    // Use this for initialization
	    void Start () {
            Debug.Log( CommandLineReader.GetCommandLine() );
            Console.GetInstance().Log(CommandLineReader.GetCommandLine());
            Console.GetInstance().Log(CommandLineReader.GetCustomArgument("Language"));
            Console.GetInstance().Log(CommandLineReader.GetCustomArgument("Version"));
	    }
	
	    // Update is called once per frame
	    void Update () {
	
	    }
    }

}
