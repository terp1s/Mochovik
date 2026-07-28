using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistance
{
    void LoadSave(Save save);

    void SaveSave(ref Save save);

}
