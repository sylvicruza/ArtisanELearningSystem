using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtisanELearningSystem.Entities;
using ArtisanELearningSystem.Services.Interfaces;
using ArtisanELearningSystem.Models;
using ArtisanELearningSystem.Exceptions;
using Microsoft.AspNetCore.Http;


namespace ArtisanELearningSystem.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseService courseService;
        private readonly ILoginService loginService;
        private readonly IProgressTrackingService progressTrackingService;
        private readonly IDiscussionService discussionService;
        private readonly IEnrollmentService enrollmentService;
        private readonly IQuizService quizService;
        public CourseController(ICourseService _courseService, ILoginService loginService, IProgressTrackingService progressTrackingService, IDiscussionService discussionService,IEnrollmentService enrollmentService,IQuizService quizService)
        {
            courseService = _courseService;
            this.loginService = loginService;
            this.progressTrackingService = progressTrackingService;
            this.discussionService = discussionService; 
            this.enrollmentService = enrollmentService;
            this.quizService = quizService;
        }

        // GET: Course
        public async Task<IActionResult> Index(string? response, string? type)
        {
            if (type == "success" && !string.IsNullOrEmpty(response))
            {
                ViewBag.Message = response;
            }
            else if (type == "failure" && !string.IsNullOrEmpty(response))
            {
                ViewBag.Failure = response;
            }
            return View(await courseService.GetAllCourses());
        }

        // GET: Course/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            var course = await courseService.GetCourseId(id);

            return View(course);
        }

        // GET: Course/DetailsView/5
        public async Task<IActionResult> CourseDetailview(int? id)
        {

            var course = await courseService.GetCourseId(id);

            if (course.ViewCount == null)
            {
                course.ViewCount = 1;
            }
            else
            {
                // Increment the view count
                course.ViewCount++;
            }
            await courseService.UpdateCourse(course);
            ViewData["EnrolledCourses"] = enrollmentService.GetCourseEnrollmentByCourseId(course.Id).Result;
            ViewData["Ratings"] = courseService.GetRating(course.Id).Result;
            ViewData["Lectures"] = courseService.GetLectureByCourseId(course.Id).Result;
            ViewData["Quizes"] = quizService.GetQuizzesForCourseAsync(course.Id).Result;
           
           
           


            return View(course);

        }

        [HttpPost]
        public async Task<IActionResult> EvaluateAnswers(int quizId, List<int> answerIds)
        {
            Quiz quiz = await quizService.GetQuizByIdAsync(quizId);

            if (quiz == null)
            {
                return NotFound(); // Quiz not found
            }

            List<string> feedback = await quizService.EvaluateQuizAsync(quiz, answerIds);

            return PartialView("_FeedbackPartial", feedback);
        }


        [HttpPost]
        public async Task<IActionResult> SubmitRating(int CourseId, int Rating, string Comment)
        {
            var course = await courseService.GetCourseId(CourseId);

            // Calculate the new rating based on the existing ratings and the new ratingValue
            double newRating = CalculateNewRating(course, Rating);

            // Update the course's average rating
            course.AverageRating = newRating;

            // Add the new CourseRating entry with the student's rating and comment
            await AddCourseRating(CourseId, Rating, Comment);

            // Update the changes to the database
            await courseService.UpdateCourse(course);

            // Redirect back to the course details page or wherever you want to go after submitting the rating
            return RedirectToAction("CourseDetailview", new { id = course.Id });
        }


        private double CalculateNewRating(Course course, int newRatingValue)
        {
            int totalRatings = course.TotalRatings ?? 0;
            double existingRatingSum = (course.AverageRating ?? 0) * totalRatings;

            // Calculate the new total ratings and rating sum
            totalRatings += 1;
            double newRatingSum = existingRatingSum + newRatingValue;

            // Calculate the new average rating
            double newRating = newRatingSum / totalRatings;

            return newRating;
        }

        public async Task AddCourseRating(int courseId, int rating, string comment)
        {
            // Get the current student (you need to get the student ID based on the logged-in user)
            var email = User?.Identity?.Name;
            var currentUserId = (await loginService.GetLoggedInUser<Student>(email));
            var studentId = currentUserId.Id;

            // Create a new CourseRating entry with the provided rating, comment, and student ID
            var courseRating = new CourseRating
            {
                CourseId = courseId,
                StudentId = studentId,
                Rating = rating,
                Comment = comment,
                DateCreated = DateTime.UtcNow
            };

            // Add the new CourseRating entry to the database
            
            await courseService.SubmitRating(courseRating);
        }

        public async Task UpdateCourse(Course course)
        {

            await courseService.UpdateCourse(course);
        }



        // GET: Course/ShoppingCart/5
        public async Task<IActionResult> ShoppingCart(int? id)
        {

            var course = await courseService.GetCourseId(id);

            return View(course);

        }

        // GET: Course/Create
        public IActionResult Create()
        {
            /* ViewData["CurriculumId"] = new SelectList(_context.Set<Curriculum>(), "Id", "Id");
             ViewData["InstructorId"] = new SelectList(_context.Instructor, "Id", "Id");*/
            return View();
        }

        // POST: Course/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var email = User.Identity?.Name;
                    model.CreatedBy = email;
                    model.Poster = await SaveFileAsync(model.File, "videos");
                    await courseService.CreateCourse(model);

                    var successMessage = "Course created successfully";
                    return RedirectToAction("Courses", "Instructor", new { response = successMessage, type = "success" });
                }
                catch (DuplicateFoundException ex)
                {

                    ViewBag.Failure = ex.Message;
                    return View(model);
                }


            }
            var validationErrorMessage = "An error occurred creating course";
            ViewBag.Failure = validationErrorMessage;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLecture(CombinedViewModel model)
        {
           
                try
                {
                    // Save the file path in the model to store it in the database
                    model.Lecture.URL = await SaveFileAsync(model.Lecture.VideoFile,"videos");
                    model.Lecture.Poster = await SaveFileAsync(model.Lecture.PosterFile, "posters");
                    model.Lecture.InstrustorId = loginService.GetLoggedInUser<Instructor>(User.Identity.Name).Result.Id;


                    await courseService.CreateLecture(model.Lecture);

                    return HandleLectureCreationResponse("Lecture added successfully", "success");
                }
                catch (DuplicateFoundException ex)
                {
                    return HandleLectureCreationResponse(ex.Message, "failure");
                }
                catch (Exception ex)
                {
                    return HandleLectureCreationResponse("An error occurred creating lecture: " + ex.Message, "failure");
                }
            
          
        }

        private IActionResult HandleLectureCreationResponse(string message, string messageType)
        {
            return RedirectToAction("Courses", "Instructor", new { response = message, type = messageType });
        }



        private async Task<string?> SaveFileAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            // Get the filename and file extension
            var fileName = Path.GetFileName(file.FileName);
            var fileExtension = Path.GetExtension(fileName);

            // Generate a unique filename
            var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;

            // Save the file to the server
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // Return the unique filename to be stored in the database
            return uniqueFileName;
        }

        // Action to display course details and progress tracking
        public async Task<IActionResult> CourseDetails(int courseId)
        {
            // Fetch the current user ID (You might have an authentication system in place)
            var email = User?.Identity?.Name;
            var currentUserId = (await loginService.GetLoggedInUser<Student>(email))?.StudentId;

            var course = await courseService.GetCourseByCourseId(courseId);
            if (course == null)
            {
                return NotFound();
            }

            var userProgress = await progressTrackingService.GetUserProgress(currentUserId, courseId);

            var viewModel = new CourseDetailsViewModel
            {
                Course = course,
                UserProgress = userProgress
            };

            return View(viewModel);
        }


        // Action to mark a course element (lesson, exercise, or module) as completed
        [HttpPost]
        public async Task<IActionResult> MarkElementCompleted(int courseId, int? lessonId, int? exerciseId, int? moduleId)
        {
            // Fetch the current user ID (You might have an authentication system in place)
            var email = User?.Identity?.Name;
            var currentUserId = loginService.GetLoggedInUser<Student>(email)?.Result?.StudentId;

            await progressTrackingService.MarkCourseElementCompleted(currentUserId, courseId, lessonId, exerciseId, moduleId);
            return RedirectToAction("CourseDetails", new { courseId });
        }

        // Action to display course discussions
        public async Task<IActionResult> Discussion(int courseId)
        {
            var discussions = await discussionService.GetDiscussionsForCourse(courseId);
            return View(discussions);
        }

        [HttpPost]
        public async Task<IActionResult> AddPost(CourseDiscussionViewModel model)
        {
            if (!string.IsNullOrEmpty(model.NewPostContent))
            {
                var email = User?.Identity?.Name;
                var currentUserId = loginService.GetLoggedInUser<Student>(email).Result;

                var newPost = new Discussion
                {
                    CourseId = model.CourseId,
                    UserId = currentUserId?.StudentId,
                    Content = model.NewPostContent,
                    Timestamp = DateTime.Now
                };

                await discussionService.AddNewPost(newPost);
            }

            // Redirect back to the discussion page
            return RedirectToAction("Messages","Student", new { response = "Comment added" });
        }

        // GET: Course/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || courseService.GetCourse(id) == null)
            {
                return NotFound();
            }

            var course = await courseService.GetCourseId(id);
            if (course == null)
            {
                return NotFound();
            }
            /* ViewData["CurriculumId"] = new SelectList(_context.Set<Curriculum>(), "Id", "Id", course.CurriculumId);
             ViewData["InstructorId"] = new SelectList(_context.Instructor, "Id", "Id", course.InstructorId);*/
            return View(course);
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,LearningOutcomes,Requirements,Level,Category,CurriculumId,Price,Discount,IsLoginRequired,IsPublished,InstructorId,DateCreated")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await courseService.UpdateCourse(course);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!courseService.CourseExists(course.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            /*   ViewData["CurriculumId"] = new SelectList(_context.Set<Curriculum>(), "Id", "Id", course.CurriculumId);
               ViewData["InstructorId"] = new SelectList(_context.Instructor, "Id", "Id", course.InstructorId);*/
            return View(course);
        }

        // GET: Course/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || courseService.GetCourse(id) == null)
            {
                return NotFound();
            }

            var course = await courseService.GetCourseByCourseId(id);

            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (courseService.GetCourse(id) == null)
            {
                return Problem("Entity set 'ArtisanELearningSystemContext.Course'  is null.");
            }
            var course = await courseService.GetCourseId(id);
            if (course != null)
            {
                await courseService.DeleteCourse(id);
            }


            return RedirectToAction(nameof(Index));
        }


        // GET: Course/AddQuiz/{courseId}
        public IActionResult AddQuiz(int courseId)
        {
            var newQuiz = new Quiz { CourseId = courseId };
            return View(newQuiz);
        }

        // POST: Course/AddQuiz/{courseId}
        [HttpPost]
        public async Task<IActionResult> AddQuiz(CombinedViewModel quizModel)
        {


            try
            {
                var options = quizModel.Quiz.Options;
                // Create the list of questions and options based on the form input
                var quiz = new Quiz();
                List<Question> questions = new List<Question>();

                var question = new Question
                {
                    Title = quizModel.Quiz.QuestionTitle,
                    Type = quizModel.Quiz.Type,
                    Image = quizModel.Quiz.Image,
                    Score = quizModel.Quiz.Score,
                    Options = new List<Options>()
                };

                // Add options to the question

                var option = new Options
                {
                    Title = quizModel.Quiz.OptionTitle,
                    IsCorrectAnswer = quizModel.Quiz.IsCorrectAnswer,
                };

                question.Options.Add(option);
                questions.Add(question);


                quiz.Questions = questions;
                quiz.CourseId = quizModel.Quiz.CourseId;
                quiz.Title = quizModel.Quiz.Title;
                quiz.Description = quizModel.Quiz.Description;





                await quizService.CreateQuizAsync(quiz);
                return RedirectToAction("CourseDetailview", "Course", new { id = quiz.CourseId });
            }
            catch (Exception ex)
            {


                return HandleLectureCreationResponse(ex.Message, "failure");
            }


        }
        /*
                [HttpPost]
                public async Task<IActionResult> AddQuiz(CombinedViewModel quizModel)
                {
                    try
                    {
                        var quiz = new Quiz
                        {
                            Title = quizModel.Quiz.Title,
                            Description = quizModel.Quiz.Description,
                            CourseId = quizModel.Quiz.CourseId,
                            Questions = new List<Question>()
                        };

                        foreach (var questionViewModel in quizModel.Quiz.Questions)
                        {
                            var question = new Question
                            {
                                Title = questionViewModel.Title,
                                Type = questionViewModel.Type,
                                Image = questionViewModel.Image,
                                Score = questionViewModel.Score,
                                Options = new List<Options>()
                            };

                            foreach (var optionViewModel in questionViewModel.Options)
                            {
                                var option = new Options
                                {
                                    Title = optionViewModel.Title,
                                    IsCorrectAnswer = optionViewModel.IsCorrectAnswer
                                };
                                question.Options.Add(option);
                            }

                            quiz.Questions.Add(question);
                        }

                        await quizService.CreateQuizAsync(quiz);

                        return RedirectToAction("CourseDetailview", "Course", new { id = quiz.CourseId });
                    }
                    catch (Exception ex)
                    {
                        // Handle the exception and return an appropriate response
                        return HandleLectureCreationResponse(ex.Message, "failure");
                    }
                }
        */

        // API endpoint to get courses by category
        [HttpGet("api/courses/{category}")]
        public async Task<IActionResult> GetCoursesByCategory(string category)
        {
            try
            {
                var courses = await courseService.GetCoursesByCategory(category);
                return Ok(courses); // Return courses as JSON response
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
