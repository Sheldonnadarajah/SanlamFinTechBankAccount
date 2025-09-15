# SanlamFinTechBankAccount
Bank account withdrawal code improvement exercise


## Approach

- Given this code, I would be deeply concerned.
- I would ask if this code is live?
- I would ask for any documentation around process ,use case or business flow.
- I would check if there are any unit / integration tests and write tests to meet the business flow.
- I would then start refactoring taking into consideration the SOLID principals.

## Details of Refactoring

- Separation of concerns: Split controller, service, repository, and event publishing.
- Dependency injection: Use constructor injection instead of field injection.
- Transactions: Ensure withdrawal and event publishing are consistent (rollback if DB fails).
- Error handling & robustness: Handle nulls, DB errors, SNS failures, idempotency concerns.
- Maintainability: Use constants, config properties, logging, and DTOs instead of manual JSON formatting.
- Observability: Add logs and traceability for auditing withdrawals.
- Flexibility: Make SNS publishing pluggable (interface-based), allowing future replacements (e.g., Kafka, RabbitMQ).
- Testability: Decouple persistence and messaging for unit testing with mocks.
- Data correctness: Use optimistic locking to avoid race conditions.

## Recommendations

- Validate input parameters (e.g., check for negative withdrawal amounts).
- Use transactions to ensure atomicity of balance updates and event publishing.
- Handle exceptions for database and SNS operations gracefully.
- Log all withdrawal attempts and failures for audit purposes.
- Consider returning structured JSON responses.
