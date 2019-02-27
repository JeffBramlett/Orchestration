using System;
using System.Collections.Generic;
using System.Text;
using EquationSolver;
using EquationSolver.Dto;

namespace Common.Orchestration
{
    public class Orchestration
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
                _variables = _variables ?? new VariableProvider();
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
        #endregion

        #region Ctors and Dtors
        public Orchestration()
        {
        }

        #endregion

        #region Publics
        public void AddOrchestrator<T>(IOrchestrator<T> orchestrator)
        {
            Type typeAsKey = typeof(T);
            if (!_orchestrators.ContainsKey(typeAsKey))
            {
                _orchestrators.Add(typeAsKey, orchestrator);
                orchestrator.Start();
            }
            else
            {
                // TODO:  Handle case
            }
        }

        public void ScheduleItem<T>(T item, DateTime start, TimeSpan interval, TimeSpan duration)
        {
            Type typeAsKey = typeof(T);
            if (_orchestrators.ContainsKey(typeAsKey))
            {
                IOrchestrator<T> orch = _orchestrators[typeAsKey] as IOrchestrator<T>;
                orch.ScheduleItem(item, start, interval, duration);
            }
            else
            {
                throw new ArgumentOutOfRangeException("Cannot find Orchestrator for item");
            }
        }
        #endregion

        #region Privates
        #endregion
    }
}
