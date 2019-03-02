using System;
using System.Collections.Generic;
using System.Text;
using EquationSolver;
using EquationSolver.Dto;

namespace Common.Orchestration
{
    /// <summary>
    /// Executing class as a container/manager for Orchestrators.
    /// Allows for the start/stop of Orchestrators by direct calls or by equation execution.
    /// <remarks>
    /// To start Orchestrator use equations to set the variable named the same as the Orchestrator.Name to true
    /// To stop Orchestrator use equations to set the variable named the same as the Orchestrator.Name to false
    /// </remarks>
    /// </summary>
    public partial class Orchestration : IOrchestration
    {
        #region Fields
        private VariableProvider _variables;
        private EquationProject _equationProject;
        private IEquationSolver _solver;
        private Dictionary<Type, object> _orchestrators;
        #endregion

        #region Properties
        private VariableProvider Variables
        {
            get
            {
                if (_variables == null)
                {
                    _variables = new VariableProvider();
                    _variables.VariableValueChanged += VariableValueChanged;
                }

                return _variables;
            }
        }

        private IEquationSolver Solver
        {
            get
            {
                if (_solver == null)
                {
                    _solver = EquationSolverFactory.Instance.CreateEquationSolver(Project, Variables);
                }
                return _solver;
            }
        }

        private EquationProject Project
        {
            get
            {
                if (_equationProject == null)
                {
                    _equationProject = new EquationProject()
                    {
                        Variables = new List<Variable>(),
                        Functions = new List<Function>(),
                        Tables = new List<Table>(),
                        Equations = new List<Equation>(),
                        Audit = new AuditInfo()
                        {
                            CreatedBy = Environment.UserName,
                            CreatedOn = DateTime.Now,
                            ModifiedBy = Environment.UserName,
                            ModifiedOn = DateTime.Now
                        },
                        Settings = new SolverSettings()
                        {
                            CalculationMethod = CalculationMethods.Decimal
                        },
                        Title = "Orchestration Project"
                    };
                }

                return _equationProject;
            }
        }

        private Dictionary<Type, object> Orchestrators
        {
            get
            {
                _orchestrators = _orchestrators ?? new Dictionary<Type, object>();
                return _orchestrators;
            }
        }
        #endregion

        #region Ctors and Dtors
        /// <summary>
        /// Default Ctor
        /// </summary>
        /// <param name="project">supply the equation project (optional)</param>
        public Orchestration(EquationProject project = null)
        {
            _equationProject = project;
        }

        #endregion

        #region Publics for Orchestrator
        /// <summary>
        /// Add or Change Orchestrator
        /// </summary>
        /// <typeparam name="T">the orchestrator type</typeparam>
        /// <param name="orchestrator">the Orchestrator to add/change</param>
        public void SetOrchestrator<T>(IOrchestrator<T> orchestrator)
        {
            if(string.IsNullOrEmpty(orchestrator.Name))
                throw new ArgumentNullException("Orchestator.Name", "Orchestator name cannot be empty or null");

            Type typKey = typeof(T);

            if (!Orchestrators.ContainsKey(typKey))
            {
                Orchestrators.Add(typKey, orchestrator);
                Variables.SetVariable(orchestrator.Name, true);
                orchestrator.Start();
            }
            else
            {
                var  orch = Orchestrators[typKey] as IOrchestrator<T>;
                orch.Stop();
                orch.Dispose();

                Orchestrators[typKey] = orchestrator;
            }
        }

        /// <summary>
        /// Remove Orchestrators by Type
        /// </summary>
        /// <typeparam name="T">The type to remove from the Orchestration</typeparam>
        public void RemoveOrchestrator<T>()
        {
            Type typKey = typeof(T);

            if (Orchestrators.ContainsKey(typKey))
            {
                var name = ((IOrchestrator<T>) Orchestrators[typKey]).Name;
                Orchestrators.Remove(typKey);
                Variables.RemoveVariable(name);
            }
        }

        /// <summary>
        /// Start Orchestrators by type
        /// </summary>
        /// <typeparam name="T">The type of Orchestrators to start</typeparam>
        public void StartOrchestrator<T>()
        {
            Type typKey = typeof(T);

            if (Orchestrators.ContainsKey(typKey))
            {
                var orch = Orchestrators[typKey] as IOrchestrator<T>;
                orch.Start();
            }
        }

        /// <summary>
        /// Start many Orchestrators that match the types array
        /// </summary>
        /// <param name="types">the array of types to start</param>
        public void StartOrchestratorsByType(params Type[] types)
        {
            if (types != null && types.Length > 0)
            {
                foreach (var type in types)
                {
                    if (Orchestrators.ContainsKey(type))
                    {
                        var orch = Orchestrators[type] as IOrchestrateBase;
                        orch.Start();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Stop many Orchestrators that match the types array
        /// </summary>
        /// <param name="types">the array of types to stop</param>
        public void StopOrchestratorsByType(params Type[] types)
        {
            if (types != null && types.Length > 0)
            {
                foreach (var type in types)
                {
                    if (Orchestrators.ContainsKey(type))
                    {
                        var orch = Orchestrators[type] as IOrchestrateBase;
                        orch.Stop();
                        Variables.RemoveVariable(orch.Name);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Stop Orchestrators for a type
        /// </summary>
        /// <typeparam name="T">The type for Orchestrators</typeparam>
        public void StopOrchestrator<T>()
        {
            Type typKey = typeof(T);

            if (Orchestrators.ContainsKey(typKey))
            {
                var orch = Orchestrators[typKey] as IOrchestrator<T>;
                orch.Stop();
            }

        }

        /// <summary>
        /// Starts an Orchestrator identified by logical name
        /// </summary>
        /// <param name="name">the name of the Orchestor to start</param>
        public void StartOrchestratorByName(string name)
        {
            var orchestrators = Orchestrators.Values;
            foreach (var orchestrator in orchestrators)
            {
                if (((IOrchestrateBase)orchestrator).Name == name)
                {
                    ((IOrchestrateBase)orchestrator).Start();
                    break;
                }
            }
        }

        /// <summary>
        /// Stop and Orchestrator identified by logical name
        /// </summary>
        /// <param name="name">the name of the Orchestrator to stop</param>
        public void StopOrchestratorByName(string name)
        {
            var orchestrators = Orchestrators.Values;
            foreach (var orchestrator in orchestrators)
            {
                if (((IOrchestrateBase)orchestrator).Name == name)
                {
                    ((IOrchestrateBase)orchestrator).Stop();
                    break;
                }
            }
        }
        #endregion

        #region Publics for Solver
        /// <summary>
        /// Sets the variable value (if the variable has a trigger the associated Equations will also execute)
        /// </summary>
        /// <param name="variableName">The variable name</param>
        /// <param name="value">the variable value</param>
        public void SetVariable(string variableName, string value)
        {
            Variables.SetVariable(variableName, value);
        }

        /// <summary>
        /// Execute the equations
        /// </summary>
        public void SolveEquations()
        {
            Solver.SolveEquations();
        }
        #endregion

        #region Events
        private void VariableValueChanged(string variableName)
        {
            if (Variables[variableName].BoolValue)
            {
                StartOrchestratorByName(variableName);
            }
            else
            {
                StopOrchestratorByName(variableName);
            }
        }
        #endregion
    }
}
