using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class Character :  MonoBehaviour, IBalancable
{
    public event EventHandler Changed;

    private void OnChanged()
    {
        if (Changed != null)
        {
            Changed(this, EventArgs.Empty);
        }
    }

    #region Members
    private int _MaxHealth;
    public int MaxHealth{
        get
        {
            return _MaxHealth;
        }
        set
        {
            _MaxHealth = value;
            OnChanged();
        }
    }

    private int _Damage;
    public int Damage
    {
        get
        {
            return _Damage;
        }
        set
        {
            _Damage = value;
            OnChanged();
        }
    }

    private float _Speed;
    public float Speed
    {
        get
        {
            return _Speed;
        }
        set
        {
            _Speed = value;
            OnChanged();
        }
    }

    private string _Name;
    public string Name
    {
        get
        {
            return _Name;
        }
        set
        {
            _Name = value;
            OnChanged();
        }
    }

    private int _Level;
    public int Level
    {
        get
        {
            return _Level;
        }
        set
        {
            _Level = value;
            OnChanged();
        }
    }

    private string _Class;
    public string Class
    {
        get
        {
            return _Class;
        }
        set
        {
            _Class = value;
            OnChanged();
        }
    }

    private float _Experience;
    public float Experience
    {
        get
        {
            return _Experience;
        }
        set
        {
            _Experience = value;
            OnChanged();
        }
    }
    #endregion

    void Awake()
    {
        _MaxHealth = 100;
        _Damage = 10;
        _Speed = 5.4f;
        _Name = "genericCharacter123";
        _Level = 1;
        _Class = "Warrior";
        _Experience = 10.5f;
    }

    void Start()
    {
        Root.Instance.bdata.GetBalancing(this, this.GetType());
    }
}
