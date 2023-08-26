using ArtisanELearningSystem.Entities;
using ArtisanELearningSystem.Models;
using ArtisanELearningSystem.Services.Interfaces;

namespace ArtisanELearningSystem.Services
{
    public class RecommendationService : IRecommendationService
    {

        private readonly IStudentService studentService;
        private readonly ICourseService courseService;
        private readonly IEnrollmentService enrollmentService;

        public RecommendationService(IStudentService studentService, ICourseService courseService, IEnrollmentService enrollmentService)
        {
            this.studentService = studentService;
            this.courseService = courseService;
            this.enrollmentService = enrollmentService;
        }

        public List<RecommendedLearningPath> GetPersonalizedLearningPaths(Student user, int numberOfPaths = 5)
        {
            var collaborativeFilteringRecommendations = GetCollaborativeFilteringRecommendations(user);
            var contentBasedFilteringRecommendations = GetContentBasedFilteringRecommendations(user);

            // Merge and sort the recommendations based on scores
            var mergedRecommendations = collaborativeFilteringRecommendations
                .Concat(contentBasedFilteringRecommendations)
                .GroupBy(r => r.Courses[0].Id)
                .Select(g => new RecommendedLearningPath
                {
                    Courses = g.First().Courses,
                    Score = g.Average(r => r.Score)
                })
                .OrderByDescending(r => r.Score)
                .ToList();

            return mergedRecommendations.Take(numberOfPaths).ToList();
        }


        private List<RecommendedLearningPath> GetCollaborativeFilteringRecommendations(Student user)
        {
            var recommendations = new List<RecommendedLearningPath>();

            var allCourses = courseService.GetAllCourses().Result;
            if (allCourses == null)
            {
                // Handle the case when the course data is not available
                return recommendations;
            }

            var allStudents = studentService.GetAllStudents().Result;
            if (allStudents == null)
            {
                // Handle the case when the student data is not available
                return recommendations;
            }

            foreach (var course in allCourses)
            {
                var userCourseIds = user.CourseEnrollments?.Select(e => e.CourseId).ToList();
                if (userCourseIds == null || userCourseIds.Contains(course.Id))
                {
                    // Skip the course if the user has already enrolled in it or if user's courses data is not available
                    continue;
                }

                    var coursesEnrolledByOtherUsers = allStudents
                            .Where(s => s.Id != user.Id)
                            .SelectMany(s => s.CourseEnrollments ?? Enumerable.Empty<CourseEnrollment>())
                            .Where(e => e.CourseId == course.Id && e.Course != null)
                            .Select(e => e.Course)
                            .ToList();


                if (coursesEnrolledByOtherUsers.Any())
                {
                    var score = CalculateCollaborativeFilteringScore(user, coursesEnrolledByOtherUsers);
                    recommendations.Add(new RecommendedLearningPath
                    {
                        Courses = new List<Course> { course },
                        Score = score
                    });
                }
            }

            return recommendations;
        }

        private double CalculateCollaborativeFilteringScore(Student user, List<Course> coursesEnrolledByOtherUsers)
        {
            // Calculate collaborative filtering score based on user similarities and course enrollments
            // You can use different similarity metrics, such as cosine similarity or Jaccard similarity
            // For simplicity, let's use a basic weighted sum of similarities
            double score = 0.0;
            foreach (var otherUser in studentService.GetAllStudents().Result.Where(s => s.Id != user.Id))
            {
                double similarity = CalculateUserSimilarity(user, otherUser);
                double enrollmentWeight = coursesEnrolledByOtherUsers.Count(c => otherUser.CourseEnrollments.Any(e => e.CourseId == c.Id));
                score += similarity * enrollmentWeight;
            }

            return score;
        }

        private double CalculateUserSimilarity(Student user1, Student user2)
        {
            // Convert user interests and learning objectives to vectors
            var interestsVector1 = GetFeatureVector(user1.Interests);
            var interestsVector2 = GetFeatureVector(user2.Interests);

            var learningObjectivesVector1 = GetFeatureVector(user1.LearningObjectives);
            var learningObjectivesVector2 = GetFeatureVector(user2.LearningObjectives);

            // Calculate cosine similarity for interests and learning objectives
            double interestsSimilarity = CosineSimilarity(interestsVector1, interestsVector2);
            double learningObjectivesSimilarity = CosineSimilarity(learningObjectivesVector1, learningObjectivesVector2);

            // Combine the two similarity scores using a weighted average or any other method as needed
            double similarity = (interestsSimilarity + learningObjectivesSimilarity) / 2.0;

            return similarity;
        }

        private double[] GetFeatureVector(string features)
        {
            // Process the features (e.g., interests or learning objectives) and convert them to a vector
            // For example, you can tokenize the features, count the occurrences, and normalize the vector

            // For simplicity, let's assume the features are separated by commas and we count occurrences
            var featureList = features?.ToLower()?.Split(", ") ?? Enumerable.Empty<string>();
            var uniqueFeatures = featureList.Distinct().ToList();
            double[] vector = new double[uniqueFeatures.Count];

            foreach (var feature in featureList)
            {
                int index = uniqueFeatures.IndexOf(feature);
                vector[index] += 1; // Count the occurrences of each feature
            }

            // Normalize the vector to have values between 0 and 1
            double sumOfSquares = vector.Sum(x => x * x);
            double norm = Math.Sqrt(sumOfSquares);
            vector = vector.Select(x => x / norm).ToArray();

            return vector;
        }

        private double CosineSimilarity(double[] vector1, double[] vector2)
        {
            // Calculate the cosine similarity between two vectors
            double dotProduct = vector1.Zip(vector2, (a, b) => a * b).Sum();
            double magnitude1 = Math.Sqrt(vector1.Sum(x => x * x));
            double magnitude2 = Math.Sqrt(vector2.Sum(x => x * x));

            // Handle the case when either magnitude is zero to avoid division by zero
            if (magnitude1 == 0 || magnitude2 == 0)
            {
                return 0.0;
            }

            double similarity = dotProduct / (magnitude1 * magnitude2);
            return similarity;
        }


        private List<RecommendedLearningPath> GetContentBasedFilteringRecommendations(Student user)
        {
            // Implement content-based filtering to recommend courses based on user interests and learning objectives
            // For simplicity, let's assume a basic score for each course based on the user's interests and learning objectives
            var recommendations = new List<RecommendedLearningPath>();
            foreach (var course in courseService.GetAllCourses().Result)
            {
                if (!user.CourseEnrollments.Any(e => e.CourseId == course.Id))
                {
                    double score = CalculateContentBasedFilteringScore(user, course);
                    recommendations.Add(new RecommendedLearningPath
                    {
                        Courses = new List<Course> { course },
                        Score = score
                    });
                }
            }

            return recommendations;
        }

        private double CalculateContentBasedFilteringScore(Student user, Course course)
        {
            // Step 1: Prepare user and course vectors based on interests and learning objectives

            // Convert user interests and learning objectives to a single string representation
            string userPreferences = user.Interests + " " + user.LearningObjectives;

            // Tokenize the user preferences into individual terms (words)
            string[] userTerms = userPreferences.Split(' ');

            // Convert course interests and learning objectives to a single string representation
            string courseContent = course.Tags + " " + course.LearningOutcomes;

            // Tokenize the course content into individual terms (words)
            string[] courseTerms = courseContent.Split(' ');

            // Step 2: Calculate term frequency (TF) for user and course vectors

            // Count the occurrences of each term in user preferences
            Dictionary<string, int> userTermFrequency = new Dictionary<string, int>();
            foreach (string term in userTerms)
            {
                if (userTermFrequency.ContainsKey(term))
                    userTermFrequency[term]++;
                else
                    userTermFrequency[term] = 1;
            }

            // Count the occurrences of each term in course content
            Dictionary<string, int> courseTermFrequency = new Dictionary<string, int>();
            foreach (string term in courseTerms)
            {
                if (courseTermFrequency.ContainsKey(term))
                    courseTermFrequency[term]++;
                else
                    courseTermFrequency[term] = 1;
            }

            // Step 3: Calculate the term frequency-inverse document frequency (TF-IDF) for user and course vectors

            // Calculate the maximum term frequency in user preferences
            int maxUserTermFrequency = userTermFrequency.Values.Max();

            // Calculate the maximum term frequency in course content
            int maxCourseTermFrequency = courseTermFrequency.Values.Max();

            // Calculate the inverse document frequency (IDF) for each term in user preferences
            Dictionary<string, double> userInverseDocumentFrequency = new Dictionary<string, double>();
            foreach (string term in userTermFrequency.Keys)
            {
                double idf = Math.Log((double)maxUserTermFrequency / (userTermFrequency[term] + 1));
                userInverseDocumentFrequency[term] = idf;
            }

            // Calculate the inverse document frequency (IDF) for each term in course content
            Dictionary<string, double> courseInverseDocumentFrequency = new Dictionary<string, double>();
            foreach (string term in courseTermFrequency.Keys)
            {
                double idf = Math.Log((double)maxCourseTermFrequency / (courseTermFrequency[term] + 1));
                courseInverseDocumentFrequency[term] = idf;
            }

            // Step 4: Calculate the content-based filtering score using Cosine Similarity

            double numerator = 0.0;
            double userVectorMagnitude = 0.0;
            double courseVectorMagnitude = 0.0;

            // Calculate the numerator (dot product) of the Cosine Similarity formula
            foreach (string term in userTermFrequency.Keys)
            {
                if (courseTermFrequency.ContainsKey(term))
                {
                    double tfidfUser = userTermFrequency[term] * userInverseDocumentFrequency[term];
                    double tfidfCourse = courseTermFrequency[term] * courseInverseDocumentFrequency[term];
                    numerator += tfidfUser * tfidfCourse;
                }
            }

            // Calculate the magnitude of the user vector
            foreach (double tfidf in userTermFrequency.Values)
            {
                userVectorMagnitude += Math.Pow(tfidf, 2);
            }
            userVectorMagnitude = Math.Sqrt(userVectorMagnitude);

            // Calculate the magnitude of the course vector
            foreach (double tfidf in courseTermFrequency.Values)
            {
                courseVectorMagnitude += Math.Pow(tfidf, 2);
            }
            courseVectorMagnitude = Math.Sqrt(courseVectorMagnitude);

            // Calculate the Cosine Similarity score
            double score = numerator / (userVectorMagnitude * courseVectorMagnitude);

            // Return the content-based filtering score for the course
            return score;
        }

    }
}
