using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistanceLessons.Models
{
    public partial class DataEntitiesManager
    {
        public List<Answer> GetAnswerList()
        {
            return _db.Answers.ToList<Answer>();
        }

        public List<Answer> TestAnswers(Guid TestId)
        {
            return (from answers in _db.Answers
                    where answers.TestId == TestId
                    select answers).ToList();
        }

        public List<Answer> TestTrueAnswers(Guid TestId)
        {
            return (from answers in _db.Answers
                    where answers.TestId == TestId && answers.Valid==true
                    select answers).ToList();
        }

        public Answer Answer(Guid AnswerId)
        {
            return (from answer in _db.Answers
                    where answer.AnswerId == AnswerId
                    select answer).FirstOrDefault();
        }

        public void UpdateAnswers(List<Answer> Answers)
        {
            foreach (Answer answer in Answers)
            {
                Answer tmp = Answer(answer.AnswerId);
                tmp.Answer1 = answer.Answer1;
                tmp.Valid = answer.Valid;
            }
            Save();
        }

        public void AddAnswer(Answer answer)
        {
            if (answer != null)
            {
                _db.AddToAnswers(answer);
                Save();
            }
        }

        public void DeleteAnswer(Guid AnswerId)
        {
            var answer = _db.Answers.SingleOrDefault(c => c.AnswerId == AnswerId);
            if (answer != null)
            {
                _db.DeleteObject(answer);
                _db.SaveChanges();
            }
        }

        public bool IsAnswer(Guid id)
        {
            return (from answer in _db.Answers
                    where answer.AnswerId == id
                    select answer).FirstOrDefault() == null ? false : true;
        }
    }
}