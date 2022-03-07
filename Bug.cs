namespace ticketsV3 {
    public class Bug : Ticket {
        public string Severity { get; set; }

        public override string entry() {
            return $"{ticketID},{summary},{status},{priority},{submitter},{assigned},{string.Join("|", peopleWatching)},{Severity}";
        }
    }
}