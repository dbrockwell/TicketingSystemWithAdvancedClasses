namespace ticketsV3 {
    public class Bug : Ticket {
        public string severity { get; set; }

        public override string entry() {
            return $"{ticketID},{summary},{status},{priority},{submitter},{assigned},{string.Join("|", peopleWatching)},{severity}";
        }
    }
}