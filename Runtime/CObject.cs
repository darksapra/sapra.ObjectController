using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
namespace sapra.ObjectController
{
[RequireComponent(typeof(Rigidbody))]
public class CObject : MonoBehaviour
{
    [SerializeField] private string ComponentsNamespace = "sapra.ObjectController";

    [Tooltip("Enables Continuous check of new components")]
    public bool continuosCheck = true;
    public static float TimeScale = 1f;

    [HideInInspector] public Rigidbody rb;
    private List<AbstractModule> modules = new List<AbstractModule>();
    public ActiveModule activeModule = new ActiveModule();
    public PassiveModule passiveModule = new PassiveModule();
    public StatModule statModule = new StatModule();

    private InputValueHolder inputHolder;
    private InputValues _input{get{
    if(inputHolder)
        return inputHolder.input;
    else
        return null;}}
    [HideInInspector]
    [SerializeField] private bool onlyEnabled = true;
    [HideInInspector] public Vector3 gravityDirection;
    [HideInInspector] public float gravityMultiplier;

    private void Start() {
        InitializeObject(true);
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        gravityDirection = Physics.gravity.normalized;
        gravityMultiplier = Physics.gravity.magnitude;
        inputHolder = GetComponent<InputValueHolder>();
        onlyEnabled = true;
    }
    public void InitializeObject(bool forcedRestart)
    {
        addModules();
        if(forcedRestart)
        {
            foreach(AbstractModule module in modules)
            {
                module.SleepComponents(this);
            }
        }
        foreach(AbstractModule module in modules)
        {
            module.InitializeComponents(this);
        }
    }
    public void SwitchTo(bool showEnabled)
    {
        onlyEnabled = showEnabled;
        foreach(AbstractModule module in modules)
            module.onlyEnabled = showEnabled;
    }
    public T RequestComponent<T>(bool required) where T : Component
    {
        T requested = GetComponent<T>();
        if(requested == null && required)        
            requested = this.gameObject.AddComponent<T>();
        return requested;
    }
    public AbstractModule FindModule(Type module)
    {
        foreach(AbstractModule moduleFound in modules)
        {
            if(moduleFound.GetType().IsEquivalentTo(module))
            {
                return moduleFound;
            }
        }
        return null;
    }
    void FixedUpdate()
    {      
        Time.timeScale = TimeScale;
        statModule.Run(continuosCheck);
        passiveModule.Run(PassivePriority.FirstOfAll, _input, continuosCheck);
        passiveModule.Run(PassivePriority.BeforeActive, _input, continuosCheck);
        activeModule.Run(_input, continuosCheck);
        passiveModule.Run(PassivePriority.AfterActive, _input, continuosCheck);
        passiveModule.Run(PassivePriority.LastOne, _input, continuosCheck);
    }
    private void LateUpdate() {
        passiveModule.RunLate(_input);   
    }
    private void OnAnimatorIK(int layerIndex) {
    }
    public void GetAllComponents()
    {
        addModules();
        foreach(AbstractModule module in modules)
        {
            module.GetAllComponents(ComponentsNamespace);
        }
    }
    private void addModules()
    {
        if(!modules.Contains(statModule))
            modules.Add(statModule);
        if(!modules.Contains(passiveModule))
            modules.Add(passiveModule);
        if(!modules.Contains(activeModule))
            modules.Add(activeModule);
    }
    }
}

