Feature: DeleteAPost

A short summary of the feature

@post
Scenario:  Delete A Post Succesfully
	Given I am logged in as Deleter
	Given the post "MyPost2" exists in the system
	When the post is deleted
	Then the post is deleted succesfully
