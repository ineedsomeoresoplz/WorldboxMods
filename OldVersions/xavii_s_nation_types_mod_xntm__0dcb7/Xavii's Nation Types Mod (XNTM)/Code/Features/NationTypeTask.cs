using System.Linq;
using ai.behaviours;
using XNTM.Code.Utils;

namespace XNTM.Code.Features
{
    public static class NationTypeTask
    {
        private const string TaskId = "xntm_government_check";

        public static void Register()
        {
            var tasks = AssetManager.tasks_kingdom;
            var jobs = AssetManager.job_kingdom;
            if (tasks == null || jobs == null)
                return;

            BehaviourTaskKingdom task = tasks.get(TaskId);
            if (task == null)
            {
                task = new BehaviourTaskKingdom
                {
                    id = TaskId,
                    single_interval = 10f,
                    single_interval_random = 4f
                };
                task.addBeh(new NationTypeCheckAction());
                tasks.add(task);
            }

            var job = jobs.get("kingdom");
            if (job != null && job.tasks.All(t => t.id != TaskId))
                job.tasks.Insert(0, new TaskContainer<BehaviourKingdomCondition, Kingdom> { id = TaskId });
        }
    }

    public sealed class NationTypeCheckAction : BehaviourActionKingdom
    {
        public override void setupErrorChecks()
        {
            base.setupErrorChecks();
            uses_kingdoms = true;
            uses_cities = true;
            uses_cultures = true;
            uses_religions = true;
            uses_clans = true;
        }

        public override BehResult execute(Kingdom pObject)
        {
            NationTypeManager.TickAuto(pObject);
            return BehResult.Continue;
        }
    }
}
